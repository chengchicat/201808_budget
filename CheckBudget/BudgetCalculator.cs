using System;
using System.Collections.Generic;

namespace CheckBudget
{
    public class BudgetCalculator
    {
        public List<Budget> _budgets { get; set; }

        public BudgetCalculator()
        {
        }
        public BudgetCalculator(List<Budget> budgets)
        {
            _budgets = budgets;
        }

        public Decimal TotalAmount(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                throw new YouShallNotPassException();
            }
            if (endDate.Month == startDate.Month)
            {
                if (_budgets.Find(c => c.Month == startDate.ToString("yyyyMM")) == null)
                {
                    return Decimal.Zero;

                }
                else
                {
                    return (MonthTotalAmount(startDate)
                                        / DateTime.DaysInMonth(startDate.Year, startDate.Month))
                                        *((endDate - startDate).Days+1);
                }
                
            }
            else
            {
                Decimal StartAmount = (MonthTotalAmount(startDate)
                                        / DateTime.DaysInMonth(startDate.Year, startDate.Month))
                                        * AfterDays(startDate);
                Decimal EndAmount = (MonthTotalAmount(endDate)
                                        / DateTime.DaysInMonth(endDate.Year, endDate.Month))
                                        * PreDays(endDate);
                Decimal MiddleAmount = 0;

                for (int i = 1 ; i < (endDate.Month - startDate.Month) + 12 * (endDate.Year - startDate.Year); i++)
                {
                    MiddleAmount += MonthTotalAmount(startDate.AddMonths(i));
                }
                return StartAmount + MiddleAmount + EndAmount;
            }
        }

        private int AfterDays(DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month) - date.Day + 1;
        }
        private int PreDays(DateTime date)
        {
            return date.Day;
        }

        private Decimal MonthTotalAmount(DateTime date)
        {
            if (_budgets.Find(c => c.Month == date.ToString("yyyyMM")) == null)
                return Decimal.Zero;
            return _budgets.Find(c => c.Month == date.ToString("yyyyMM")).BudgetPerMonth;
        }
       

    }

    public class YouShallNotPassException : Exception
    {
    }
}