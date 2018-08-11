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
            else
            {
                Decimal total = 0;

                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    total += (MonthTotalAmount(date) / DateTime.DaysInMonth(date.Year, date.Month));
                }
                return total;
            }         
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