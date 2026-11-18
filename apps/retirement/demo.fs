( Demo retirement plan )

include plan.fs

\ Plan runs from ages 66 through 90
66 to age-start
90 to age-end

\ Starting financial assets
300000 to cash-start    \ taxable investments
500000 to rsp-start     \ tax-free growth, taxed on withdrawal
200000 to tfsa-start    \ fully tax free

\ Tax owed for the first year
5000 to tax-owed-start

\ Government pension, $2,500/month at age 70
: my-pension ( age -- income )  70 >=  2500 monthly and ;
' my-pension is pension

\ RSP required minimum distributions
' rrif-minimum is rmd

\ RSP drawdown amount, before RMDs kick in
0 to rsp-drawdown

\ Annual TFSA contribution, up to tfsa-limit
0 to tfsa-contribution


include opt.fs ( for demo )
