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
: try-rsp ( spendingK -- )  1000 * to rsp-drawdown  60 p ;
10000 try-rsp
20000 try-rsp
30000 try-rsp
40000 try-rsp

: try-tfsa ( spendingK -- )  1000 * to tfsa-contribution  60 p ;
1000 try-tfsa
3000 try-tfsa
6000 try-tfsa

```

We can run try all combinations with `optimize.fs`.

