using Microsoft.VisualStudio.TestTools.UnitTesting;
using PartnerAccounting.Core.Ivanov.Models;
using System;
using System.Collections.Generic;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PartnerAccounting.Tests.Ivanov
{
    [TestClass]
    public class DiscountCalculatorTests
    {
        private Partner _partner;

        [TestInitialize]
        public void Setup()
        {
            _partner = new Partner
            {
                Id = 1,
                Name = "Тестовый партнер",
                TypeId = 1,
                Inn = "1234567890",
                DirectorName = "Иванов И.И.",
                Phone = "+7(123)456-78-90",
                Email = "test@test.com",
                Rating = 50
            };
        }

        [TestMethod]
        public void CalculateDiscount_SalesLessThan10000_Returns0()
        {
            // Arrange
            decimal sales = 5000;

            // Act
            int discount = _partner.CalculateDiscount(sales);

            // Assert
            Assert.AreEqual(0, discount, "При сумме продаж менее 10 000 руб. скидка должна быть 0%");
        }

        [TestMethod]
        public void CalculateDiscount_SalesExactly10000_Returns5()
        {
            // Arrange
            decimal sales = 10000;

            // Act
            int discount = _partner.CalculateDiscount(sales);

            // Assert
            Assert.AreEqual(5, discount, "При сумме продаж 10 000 руб. скидка должна быть 5%");
        }

        [TestMethod]
        public void CalculateDiscount_SalesBetween10000And50000_Returns5()
        {
            // Arrange
            decimal sales = 25000;

            // Act
            int discount = _partner.CalculateDiscount(sales);

            // Assert
            Assert.AreEqual(5, discount, "При сумме продаж от 10 000 до 50 000 руб. скидка должна быть 5%");
        }

        [TestMethod]
        public void CalculateDiscount_SalesExactly50000_Returns10()
        {
            // Arrange
            decimal sales = 50000;

            // Act
            int discount = _partner.CalculateDiscount(sales);

            // Assert
            Assert.AreEqual(10, discount, "При сумме продаж 50 000 руб. скидка должна быть 10%");
        }

        [TestMethod]
        public void CalculateDiscount_SalesBetween50000And300000_Returns10()
        {
            // Arrange
            decimal sales = 150000;

            // Act
            int discount = _partner.CalculateDiscount(sales);

            // Assert
            Assert.AreEqual(10, discount, "При сумме продаж от 50 000 до 300 000 руб. скидка должна быть 10%");
        }

        [TestMethod]
        public void CalculateDiscount_SalesExactly300000_Returns15()
        {
            // Arrange
            decimal sales = 300000;

            // Act
            int discount = _partner.CalculateDiscount(sales);

            // Assert
            Assert.AreEqual(15, discount, "При сумме продаж 300 000 руб. скидка должна быть 15%");
        }

        [TestMethod]
        public void CalculateDiscount_SalesMoreThan300000_Returns15()
        {
            // Arrange
            decimal sales = 500000;

            // Act
            int discount = _partner.CalculateDiscount(sales);

            // Assert
            Assert.AreEqual(15, discount, "При сумме продаж более 300 000 руб. скидка должна быть 15%");
        }

        [TestMethod]
        public void CalculateDiscount_SalesZero_Returns0()
        {
            // Arrange
            decimal sales = 0;

            // Act
            int discount = _partner.CalculateDiscount(sales);

            // Assert
            Assert.AreEqual(0, discount, "При отсутствии продаж скидка должна быть 0%");
        }

        [TestMethod]
        public void CalculateDiscount_SalesNegative_Returns0()
        {
            // Arrange
            decimal sales = -1000;

            // Act
            int discount = _partner.CalculateDiscount(sales);

            // Assert
            Assert.AreEqual(0, discount, "При отрицательной сумме скидка должна быть 0%");
        }

        [TestMethod]
        public void DiscountDisplay_WithSalesHistory_ReturnsCorrectString()
        {
            // Arrange
            var partner = new Partner
            {
                Id = 1,
                Name = "Тестовый партнер",
                SalesHistory = new List<SaleHistory>
                {
                    new SaleHistory { TotalAmount = 60000 },
                    new SaleHistory { TotalAmount = 40000 }
                }
            };

            // Act
            string discountDisplay = partner.DiscountDisplay;

            // Assert
            Assert.AreEqual("10%", discountDisplay);
        }

        [TestMethod]
        public void DiscountDisplay_WithoutSalesHistory_Returns0()
        {
            // Arrange
            var partner = new Partner
            {
                Id = 1,
                Name = "Тестовый партнер",
                SalesHistory = new List<SaleHistory>()
            };

            // Act
            string discountDisplay = partner.DiscountDisplay;

            // Assert
            Assert.AreEqual("0%", discountDisplay);
        }

        [TestMethod]
        public void DiscountDisplay_WithNullSalesHistory_Returns0()
        {
            // Arrange
            var partner = new Partner
            {
                Id = 1,
                Name = "Тестовый партнер",
                SalesHistory = null
            };

            // Act
            string discountDisplay = partner.DiscountDisplay;

            // Assert
            Assert.AreEqual("0%", discountDisplay);
        }
    }
}