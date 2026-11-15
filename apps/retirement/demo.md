# Retirement Planner in Forth

- How I did it with Claude and a spreadsheet
- 


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

