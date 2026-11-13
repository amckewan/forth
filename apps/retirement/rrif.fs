( RRIF minimum withdrawal )

require utils.fs

create rrif-table ( CRA 2026 )
( age   percentage )
( 72 )  540 , ( 73 )  553 , ( 74 )  567 , ( 75 )  582 , ( 76 )  598 ,
( 77 )  617 , ( 78 )  636 , ( 79 )  658 , ( 80 )  682 , ( 81 )  708 ,
( 82 )  738 , ( 83 )  771 , ( 84 )  808 , ( 85 )  851 , ( 86 )  899 ,
( 87 )  955 , ( 88 ) 1021 , ( 89 ) 1099 , ( 90 ) 1192 , ( 91 ) 1306 ,
( 92 ) 1449 , ( 93 ) 1634 , ( 94 ) 1879 , ( 95 ) 2000 ,

: rrif-minimum ( balance age -- amount )
   95 min  72 -  dup 0< if  2drop 0  exit  then
   cells rrif-table + @   % ;
