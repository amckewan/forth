( Investment Accounts )

require utils.fs

\ An account has these fields:
\ 0. balance (current balance)
\ 1. starting-balance (for taxes & RMDs)
\ 2. withdrawals (for IRA/RRIF taxable income )

: account ( <name> -- )  create  0 ,  0 ,  0 , ;

: >balance            ;
: >starting-balance   cell+ ;
: >withdrawawals      2 cells + ;

: balance           ( account -- balance )      >balance @ ;
: starting-balance  ( account -- balance )      >starting-balance @ ;
: withdrawals       ( account -- withdrawals )  >withdrawawals @ ;

\ Set starting balance and clear withdrawals, balance not affected
: start ( account -- )
    dup balance  over >starting-balance !  ( save starting balance )
    >withdrawawals off ( clear withdrawals ) ;
 
: open-account ( amount account -- )
    tuck >balance !  start ;

: deposit ( amount account -- )
    >balance +! ;

: withdraw ( amount account -- )
    over negate over >balance +! ( update balance )
    dup balance 0< abort" Account overdrawn!"
    >withdrawawals +! ( update withdrawals ) ;

: grow ( return account -- )
    dup balance  rot %  swap deposit ;
