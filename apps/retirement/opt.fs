( Optimize RSP drawdown and TFSA contributions. )

0 value max-assets
0 value max-rsp-drawdown
0 value max-tfsa-contribution

: .max
    ." RSP: " max-rsp-drawdown $.
    ." TFSA: " max-tfsa-contribution $.
    ." Assets: " max-assets $. ;

: save
    max-rsp-drawdown to rsp-drawdown
    max-tfsa-contribution to tfsa-contribution ;

: max?
    assets max-assets > if
        assets            to max-assets
        rsp-drawdown      to max-rsp-drawdown
        tfsa-contribution to max-tfsa-contribution
    then ;

: try ( spending rsp-drawdown tfsa-contrib -- )
    to tfsa-contribution  to rsp-drawdown  run  max? ;

: try-tfsa ( spending rsp -- )
    tfsa-limit 1+ 0 do  2dup i try  1000 +loop  2drop ;

: try-rsp ( spending -- )
    100001 0 do  dup i try-tfsa  1000 +loop  drop ;

: optimize ( spending - )
    0 to max-assets  try-rsp  save  .max ;

: opt  1000 * optimize ;
