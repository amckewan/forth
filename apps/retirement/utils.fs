\ Financial Utilities

\ Percentages are represented as units of 1/10,000 (1234 = 12.34%)
: % ( n1 % -- n2 )  10000 */ ;

\ Convert monthly pension to annual
: monthly ( monthly -- annual )  12 * ;

\ Print string right-justified.
: type.r ( a n w -- )  over - spaces  type ;

\ Use %. and %.r to print percentages with two decimal places.
: (%)  ( % -- a n )  dup abs  0 <#  '%' hold # # '.' hold #s  rot sign  #> ;
: %.   ( % -- )      (%) type space ;
: %.r  ( % w -- )    >r (%) r> type.r ;

\ Print amounts with commas (no $ sign)
: ($) ( amount -- a n )  dup abs 0
    <#  0 1 do ( i = counter for adding commas )
        #  over 0= if ( no more digits ) leave then
        i 3 mod 0= if ',' hold then
    loop rot sign #> ;
: $.  ( n -- )    ($) type space ;
: $.r ( n w -- )  >r ($) r> type.r ;
