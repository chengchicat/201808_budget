using System.Collections.Generic;

namespace CheckBudget
{
    public interface IBudgetRepo<T>
    {
        List<Budget> GetAll();
    }
}