( Assumptions )

require utils.fs

\ The model assumes a constant USD to CAD exchange rate.
13000 ( 1.3000 ) constant usd>cad

: usd ( usd -- cad )  usd>cad % ;


450 value nominal-return     \ 4.5% annual return on 40/60 portfolio
250 value inflation          \ 2.5% annual inflation rate (30-yr avg)

\ Real return is nominal investment return after inflation.
: real-return ( -- return )  nominal-return inflation - ;

\ Taxable return for Canada est. 70% of nominal return for 40/60 split.
: taxable-return ( -- return )  nominal-return 7000 % ;
