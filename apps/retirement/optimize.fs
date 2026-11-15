( Optimize RSP drawdown and TFSA contributions. )

0 value max-assets
0 value max-rsp-drawdown
0 value max-tfsa-contribution

: .results
    cr ." RSP: " max-rsp-drawdown $.  ." TFSA: " max-tfsa-contribution $.
    ." Assets: "  max-assets $. ;

: save
    max-rsp-drawdown to rsp-drawdown
    max-tfsa-contribution to tfsa-contribution ;

: try ( spending tfsa-contrib rsp-drawdown -- )
    to rsp-drawdown  to tfsa-contribution  run
    assets max-assets > if
        assets            to max-assets
        rsp-drawdown      to max-rsp-drawdown
        tfsa-contribution to max-tfsa-contribution
    then ;

: optimize ( spending - )
    0 to max-assets
    50001 0 do
        tfsa-limit 1+ 0 do
            dup i j try
        1000 +loop
    1000 +loop
    drop .results save ;

: opt  1000 * optimize ;
