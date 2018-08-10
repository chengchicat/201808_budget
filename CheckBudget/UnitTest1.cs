using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace CheckBudget
{
    [TestClass]
    public class UnitTest1
    {
        private IBudgetRepo<Budget> repo = Substitute.For<IBudgetRepo<Budget>>();

        private List<Budget> budgets = new List<Budget>();
        /*
        [TestInitialize]
        public void TestInit()
        {
            budgets.Add(new Budget { Month = "201801", BudgetPerMonth = 310 });
            budgets.Add(new Budget { Month = "201802", BudgetPerMonth = 140 });
            budgets.Add(new Budget { Month = "201804", BudgetPerMonth = 180 });
            budgets.Add(new Budget { Month = "201805", BudgetPerMonth = 310 });
            budgets.Add(new Budget { Month = "201806", BudgetPerMonth = 310 });
            budgets.Add(new Budget { Month = "201807", BudgetPerMonth = 310 });
        }
        */
        [TestMethod]
        public void 沒預算()
        {
            budgets = new List<Budget>();
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = Decimal.Zero;
            DateTime startDate = new DateTime(2018, 6, 1);
            DateTime endDate = new DateTime(2018, 6, 1);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void 同一天()
        {
            budgets = new List<Budget>();
            budgets.Add(new Budget() { Month = "201802", BudgetPerMonth = 140 });
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = 5;
            DateTime startDate = new DateTime(2018, 2, 1);
            DateTime endDate = new DateTime(2018, 2, 1);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void 完整一個月()
        {
            budgets = new List<Budget>();
            budgets.Add(new Budget() { Month = "201804", BudgetPerMonth = 240 });
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = 240;
            DateTime startDate = new DateTime(2018, 4, 1);
            DateTime endDate = new DateTime(2018, 4, 30);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void 部分一個月()
        {
            budgets = new List<Budget>();
            budgets.Add(new Budget() { Month = "201804", BudgetPerMonth = 240 });
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = 56;
            DateTime startDate = new DateTime(2018, 4, 3);
            DateTime endDate = new DateTime(2018, 4, 9);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void 跨兩個月_都有錢()
        {
            budgets = new List<Budget>();
            budgets.Add(new Budget() { Month = "201805", BudgetPerMonth = 155 });
            budgets.Add(new Budget() { Month = "201806", BudgetPerMonth = 300 });
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = 15;
            DateTime startDate = new DateTime(2018, 5, 31);
            DateTime endDate = new DateTime(2018, 6, 1);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }
        [TestMethod]
        public void 跨三個月_中間沒有錢()
        {
            budgets = new List<Budget>();
            budgets.Add(new Budget() { Month = "201805", BudgetPerMonth = 155 });
            budgets.Add(new Budget() { Month = "201807", BudgetPerMonth = 310 });
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = 15;
            DateTime startDate = new DateTime(2018, 5, 31);
            DateTime endDate = new DateTime(2018, 7, 1);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void 跨三個月_中間也有錢()
        {
            budgets = new List<Budget>();
            budgets.Add(new Budget() { Month = "201805", BudgetPerMonth = 155 });
            budgets.Add(new Budget() { Month = "201806", BudgetPerMonth = 120 });
            budgets.Add(new Budget() { Month = "201807", BudgetPerMonth = 310 });
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = 135;
            DateTime startDate = new DateTime(2018, 5, 31);
            DateTime endDate = new DateTime(2018, 7, 1);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void 跨年跨三個月_中間也有錢()
        {
            budgets = new List<Budget>();
            budgets.Add(new Budget() { Month = "201712", BudgetPerMonth = 155 });
            budgets.Add(new Budget() { Month = "201801", BudgetPerMonth = 124 });
            budgets.Add(new Budget() { Month = "201802", BudgetPerMonth = 280 });
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = 139;
            DateTime startDate = new DateTime(2017, 12, 31);
            DateTime endDate = new DateTime(2018, 2, 1);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void 跨年跨15個月_中間也有錢()
        {
            budgets = new List<Budget>();
            budgets.Add(new Budget() { Month = "201612", BudgetPerMonth = 155 });
            budgets.Add(new Budget() { Month = "201701", BudgetPerMonth = 124 });
            budgets.Add(new Budget() { Month = "201802", BudgetPerMonth = 280 });
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = 139;
            DateTime startDate = new DateTime(2016, 12, 31);
            DateTime endDate = new DateTime(2018, 2, 1);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void 跨年跨15個月_中間也有錢_part2()
        {
            budgets = new List<Budget>();
            budgets.Add(new Budget() { Month = "201612", BudgetPerMonth = 155 });
            budgets.Add(new Budget() { Month = "201701", BudgetPerMonth = 124 });
            budgets.Add(new Budget() { Month = "201705", BudgetPerMonth = 186 });
            budgets.Add(new Budget() { Month = "201802", BudgetPerMonth = 280 });
            BudgetCalculator calculator = new BudgetCalculator(budgets);

            var expect = 325;
            DateTime startDate = new DateTime(2016, 12, 31);
            DateTime endDate = new DateTime(2018, 2, 1);
            var actual = calculator.TotalAmount(startDate, endDate);
            Assert.AreEqual(expect, actual);
        }

        [TestMethod]
        public void 例外狀態()
        {
            budgets = new List<Budget>();

            BudgetCalculator calculator = new BudgetCalculator(budgets);

            DateTime startDate = new DateTime(2018, 2, 1);
            DateTime endDate = new DateTime(2016, 12, 31);

            

            Action action = () => { var actual = calculator.TotalAmount(startDate, endDate); };
            action.Should().Throw<YouShallNotPassException>();


        }





    }
}
