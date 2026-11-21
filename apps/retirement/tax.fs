\ Tax calculator

require utils.fs

\ Canada tax constants for 2026
95000 constant oas-threshold    \ max income before OAS clawback
 7000 constant tfsa-limit       \ annual contribution limit

\ Calculate tax from a table of income/rate pairs
: bracket ( bracket income -- tax )
    over 2 cells + @ min  over @ - 0 max  swap cell+ @ % ;
: calc-tax ( 0 income rates #entries -- tax )
    2* cells bounds do  ( tax income -- )
        i over bracket
        rot + swap
    2 cells +loop  drop ;


\ === Canada federal tax ===

create fed-rates ( 2026 )
  ( income   rate )
         0 , 1400 ,
     58523 , 2050 ,
    117045 , 2600 ,
    181440 , 2900 ,
    258482 , 3300 ,
    999999 ,

16452 1400 % constant fed-bpa-credit ( ok to 177k )

: fed-tax ( income -- tax )
    0 swap fed-rates 5 calc-tax
    fed-bpa-credit - 0 max ;


\ === Ontario tax ===

create ont-rates ( 2026 )
  ( income   rate )
         0 ,  505 ,
     53891 ,  915 ,
    107785 , 1116 ,
    150000 , 1216 ,
    220000 , 1316 ,
    999999 ,

12989 505 % constant ont-bpa-credit

: ont-basic-tax ( income -- basic-tax )
    0 swap ont-rates 5 calc-tax
    ont-bpa-credit - 0 max ;

: surtax ( basic-tax -- surtax )
    0
    over 5818 > if  over 5818 - 2000 % +  then
    over 7446 > if  over 7446 - 3600 % +  then
    nip ;

: ont-tax ( income -- tax )
    ont-basic-tax dup surtax + ;


\ === Combined Canadian tax ===

: ca-tax ( income -- tax )
    dup fed-tax  swap ont-tax + ;


\ === Print tax table ===

: effective-rate ( income -- eff-rate )
    dup ca-tax  10000 rot */ ;

: marginal-rate ( income -- rate )
    dup 10000 + ca-tax  swap ca-tax - ;

: .taxes-title ( -- )
    cr ."     Income      Tax    EffRate   MargRate"
    cr ."    --------   -------  -------   --------" ;

: .taxes ( -- )  .taxes-title
    210000 40000 do  cr
        i 10 $.r
        i ca-tax 10 $.r
        i effective-rate 10 %.r
        i marginal-rate 10 %.r
    10000 +loop ;


\ === US federal tax === 

create us-rates ( 2026 )
  ( income   rate )
         0 ,  1000 , \ 10% up to  12,400
     12401 ,  1200 , \ 12% up to  50,400
     50401 ,  2200 , \ 22% up to 105,700
    105701 ,  2400 , \ 24% up to 201,775
    201776 ,  3200 ,
    256226 ,  3500 ,
    384351 ,  3700 ,
    999999 ,

16100 constant us-std-exception

: us-tax ( income -- tax )
    us-std-exception -
    0 swap us-rates 7 calc-tax ;

: us-marginal-rate ( income -- rate )
    us-rates  begin  2dup 2 cells + @ 1- > while  2 cells +  repeat
    nip cell+ @ ;

: .us-taxes ( -- )  .taxes-title
    160000 40000 do
        cr i dup 10 $.r
        i us-tax dup 10 $.r
        effective-rate 10 %.r
        i us-marginal-rate 10 %.r
    10000 +loop ;
