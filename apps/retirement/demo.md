<style>
  body { font-size: 18px; }
  pre, code { font-size: 0.85em; }
</style>   

# Retirement Planner in Forth

When I retired last year, I did my financial forecast in a spreadsheet
with help from Claude. I have since replaced that with a Forth application
that gives me more control and a better interface.
I will demonstrate using it to help decide how much to spend each year,
and when to start withdrawing from retirement savings.

## Nominal vs. Real investment returns
The **nominal** return is the return you see in financial statements.
For example, over the last 20 years the S&P 500 has returned about 10% annually.
The **real** return is the nominal return adjusted for inflation.
For example, a 4.5% nominal return with 2.5% inflation yields a 2% real return.

All numbers in the plan are in real dollars.
So $1,000 at age 90 is worth about the same as $1,000 today.
Inflation is factored in when we grow our investments.

# What goes in to the plan?
The plan takes two kinds of data,
things I can change, like how much I spend,
and things I can not, such as taxes and inflation.

Fixed values:
- Assumptions: investment return, inflation, tax rates, etc.
- Financial assets:
  - **Cash** - taxable investments
  - **RSP** - tax-deferred retirement savings (IRA/401K)
  - **TFSA** - tax-free savings account (Roth IRA)
- RSP age-based withdrawal rules
- Pension income (although I can adjust starting age)

Things I can change include:
- Annual spending
- RSP drawdown before RMDs
- TFSA contributions

The goal is to maximize spending without running out of money
before the plan ends.

[Assumptions](assumptions.fs)

[Financial data](demo.fs)

# Running the plan
The starting parameters are in `demo.fs`.
1. Initial financial assets
2. Government pension
3. RSP drawdown and required withdrawals method
4. TFSA contributions

Run the plan with `50 p`. We can see how we live off our cash
before the pension and required RSP withdrawls kick in.


# How much can I spend?
Run the plan with 60 and 70K spending levels.
How much is my final net worth? Am I comfortable with that?

# Optimizing
I can optimize my RSP withdrawals and TFSA contributions to reduce
taxes and increase final net worth.

Try:
```
60 value goal
: try-rsp ( rspdraw -- )  1000 * to rsp-drawdown  goal p ;
10 try-rsp
20 try-rsp
30 try-rsp
40 try-rsp

: try-tfsa ( tfsa -- )  1000 * to tfsa-contribution  goal p ;
1 try-tfsa
3 try-tfsa
6 try-tfsa

```

We can run try all combinations with `optimize.fs`.

60 opt

