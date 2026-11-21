( Retirement Financial Planner )

[defined] empty [if] empty [then] marker empty
warnings off decimal

require account.fs          \ Account management
require ira.fs              \ IRA RMD rules
require rrif.fs             \ RRIF RMD rules
require tax.fs              \ Fed+Ont taxes : ca-tax ( income -- tax )

\ === Input parameters, not changed during the simulation. ===

require assumptions.fs      \ Financial assumptions

66 value age-start
90 value age-end

\ Starting ASSETS
 300000 value cash-start
 500000 value rsp-start
 200000 value tfsa-start

\ Pension by age
defer pension ( age -- income )
defer taxable-pension ( age -- income )
' pension is taxable-pension

: default-pension ( age -- income )  70 >=  2000 monthly and ;
' default-pension is pension

\ RSP required minimum distributions
defer rmd ( balance age -- rmd )
' rrif-minimum is rmd

\ RSP drawdown strategy, before RMDs kick in.
\ 0=none, -1=up to OAS, +n=fixed amount
0 value rsp-drawdown

\ Annual TFSA contribution, up to tfsa-limit
0 value tfsa-contribution

\ Tax owed for the first year
5000 value tax-owed-start


\ === Model data, updated during the simulation. ===

variable age        \ current age

account cash        \ taxable cash account
account rsp         \ tax-free growth, taxable withdrawals
account tfsa        \ tax-free growth and withdrawals

\ In this model, we calculate taxes at the end of the year and pay
\ them at the start of the next year. This handles the case where
\ paying taxes triggers a taxable RSP withdrawl.

variable tax-owed       \ total tax owed
variable total-tax-paid \ total tax paid over the the whole plan

variable spending       \ actual yearly spending, may be less than requested
variable total-spending \ total spending for the plan

\ Initialize data at start of simulation
: init ( -- )
    age-start age !
    cash-start cash open-account
    rsp-start  rsp  open-account
    tfsa-start tfsa open-account
    tax-owed-start tax-owed !
    total-tax-paid off
    spending off  total-spending off ;


\ Use 10-char columns for reports
10 value width
: $..  width $.r ;
: %..  width %.r ;
: hdr ( a n -- )  width type.r ;

: .init
    cr ." Initial Cash: " cash-start $..
    cr ." Initial RSP:  " rsp-start $..
    cr ." Initial TFSA: " tfsa-start $..
    cr ." RSP Drawdown: " rsp-drawdown $..
    cr ." TFSA Contrib: " tfsa-contribution $.. ;


\ === Spending ===

: assets ( -- amount ) \ total assets available to spend
    cash balance   rsp balance +   tfsa balance + ;

: .assets ( -- )
    cash balance $..  rsp balance $..  tfsa balance $..  assets $.. ;

\ Spend cash first, then RSP, then TFSA.
\ Abort if we don't have enough money.
: spend ( amount -- )
    dup cash balance min  dup cash withdraw  - ( remaining )
    ?dup if ( out of cash, withdraw from RSP )
        dup rsp balance min  dup rsp withdraw  - ( remaining )
        ?dup if ( out of RSP, withdraw from TFSA )
            tfsa withdraw ( aborts when bankrupt )
        then
    then ;

\ Spend amount if we have it, leave actual value in `spending` ( yearly )
: maybe-spend ( amount -- )
    assets min  dup spending !  dup total-spending +!  spend ;


\ === Taxes ===

: taxable-investment-income ( -- income ) 
    cash starting-balance  taxable-return % ;

\ Non-RSP taxable income is used to optimize RSP withdrawals to avoid OAS clawback.
: non-rsp-taxable-income ( -- income )
    age @ taxable-pension  taxable-investment-income + ;

: taxable-income ( -- income )
    non-rsp-taxable-income  rsp withdrawals + ;

: tax-payable ( -- tax )
    taxable-income ca-tax ;

: calculate-taxes ( -- )  \ Calculate taxes to be paid next year
    tax-payable  tax-owed ! ;

: pay-taxes ( -- )  \ pay last year's taxes
    tax-owed @  dup spend  total-tax-paid +!  tax-owed off ;


\ === Yearly activites ===

: deposit-pension ( -- )  \ deposit pension into cash account
    age @ pension  cash deposit ;

: grow-investments ( -- )
    real-return   dup cash grow   dup rsp grow   tfsa grow ;

\ Make TFSA contribution from cash (January)
: make-tfsa-contribution ( -- )
    tfsa-contribution  tfsa-limit min  cash balance min
    dup spend  tfsa deposit ;

\ Make RSP withdrawal to cash (December)
: rsp-withdrawal ( -- amount )
    rsp-drawdown ( default amount )
    dup 0< if  drop  oas-threshold non-rsp-taxable-income -  then
    rsp starting-balance age @ rmd max ( at least RMD )
    rsp withdrawals - ( subtract any withdrawals aready done )
    0 max  rsp balance min ;

: make-rsp-withdrawal ( -- ) \ deposit into cash
    rsp-withdrawal  dup rsp withdraw  cash deposit ;


\ =========================================
\ A year in the life...

: year ( spending -- )
    rsp start                   \ start RSP for RMD/taxes
    deposit-pension             \ deposit pension (it will get spent)
    pay-taxes                   \ pay last year's taxes
    maybe-spend                 \ spend, if we have it
    make-tfsa-contribution      \ optional TFSA contribution
    cash start                  \ remember starting balance for taxes
    ( ... the year goes by ... )
    grow-investments            \ grow investment accounts
    make-rsp-withdrawal         \ make RSP withdrawal (affects taxes)
    calculate-taxes ;           \ to be paid next year


\ === Print data at year end ===

: sep  ."  - " ;

: .title ( -- )  cr ." Age"
    s" RspWithd"    hdr
    s" Pension"     hdr
    s" InvInc"      hdr
    s" TotalInc"    hdr
    s" Tax"         hdr
    s" Rate"        hdr
    s" NetInc"      hdr
    s" Spending"    hdr
    sep
    s" Cash EOY"    hdr
    s" RSP EOY"     hdr
    s" TFSA EOY"    hdr
    s" Total EOY"   hdr ;

: real-investment-income ( -- income )
    cash starting-balance  real-return % ;

: total-income ( -- income )
    rsp withdrawals   age @ pension +   real-investment-income + ;

: after-tax-income ( -- income ) \ real income minus taxes paid
    taxable-income  tax-owed @ - ;

: .year ( -- )
    cr age @ .
    rsp withdrawals $..
    age @ pension $..
    taxable-investment-income $..
    taxable-income $..
    tax-owed @ $..
    taxable-income effective-rate %..
    after-tax-income $..
    spending @ $..
    sep .assets ;


\ === Simulate the plan ===

: run ( spending -- ) \ run plan, no output
    init  age-end 1+ age-start do   i age !   dup year   loop drop ;

: .end
    cr ." Total spending: " total-spending @ $..
    cr ." Total tax paid: " total-tax-paid @ $..
    cr ." Final assets:   " assets $.. ;

: plan ( spending -- )
    init .title
    cr 8 width * 3 + spaces sep .assets
    age-end 1+ age-start do
        i age !  dup year  .year 
    loop drop .end ;

: r  1000 * run .end ;
: p  1000 * plan ;

