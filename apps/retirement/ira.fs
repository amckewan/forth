( IRA Required Minimum Distribution )
( SECURE 2.0 Act, my RMDs start at 75 )

require test/ttester.fs

( RMD table, starting at age 75, good through 95 )
( each element is the factor*10, we must take balance/factor each year )
( for example, at 75 I have to take 1/24.6th of the previous year's balance )
create ira-rmds
    ( 75 ) 246 , 237 , 229 , 220 , 211 , ( 80 ) 202 , 194 , 185 , 177 , 168 ,
    ( 85 ) 160 , 152 , 144 , 137 , 129 , ( 90 ) 122 , 115 , 108 , 101 ,  95 ,
    ( 95 ) 89 ,

: ira-rmd ( prior-year-balance age -- rmd )
    95 min  75 -  dup 0< if  2drop 0 exit  then
    cells ira-rmds + @  10 swap */ ;
