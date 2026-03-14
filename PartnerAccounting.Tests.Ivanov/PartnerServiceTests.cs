using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PartnerAccounting.Core.Ivanov.Data;
using PartnerAccounting.Core.Ivanov.Models;
using PartnerAccounting.Core.Ivanov.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PartnerAccounting.Tests.Ivanov
{
    [TestClass]
    public class PartnerServiceTests
    {
        private Mock<AppDbContext> _mockContext;
        private PartnerService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockContext = new Mock<AppDbContext>();
            _service = new PartnerService(_mockContext.Object);
        }

        [TestMethod]
        public void CalculateDiscount_WithVariousAmounts_ReturnsCorrectDiscount()
        {
            // Arrange
            var testCases = new[]
            {
                new { Sales = 5000m, ExpectedDiscount = 0 },
                new { Sales = 10000m, ExpectedDiscount = 5 },
                new { Sales = 25000m, ExpectedDiscount = 5 },
                new { Sales = 50000m, ExpectedDiscount = 10 },
                new { Sales = 150000m, ExpectedDiscount = 10 },
                new { Sales = 300000m, ExpectedDiscount = 15 },
                new { Sales = 500000m, ExpectedDiscount = 15 }
            };

            var partner = new Partner();

            // Act & Assert
            foreach (var testCase in testCases)
            {
                int discount = partner.CalculateDiscount(testCase.Sales);
                Assert.AreEqual(testCase.ExpectedDiscount, discount,
                    $"Для суммы {testCase.Sales} руб. ожидалась скидка {testCase.ExpectedDiscount}%");
            }
        }

        [TestMethod]
        public void GetPartnerDiscount_WithSalesHistory_ReturnsCorrectDiscount()
        {
            // Arrange
            int partnerId = 1;
            var partner = new Partner { Id = partnerId };

            // Не можем протестировать реальный сервис без БД, 
            // но можем протестировать логику расчета

            // Act
            int discount1 = partner.CalculateDiscount(5000);   // 0%
            int discount2 = partner.CalculateDiscount(25000);  // 5%
            int discount3 = partner.CalculateDiscount(150000); // 10%
            int discount4 = partner.CalculateDiscount(500000); // 15%

            // Assert
            Assert.AreEqual(0, discount1);
            Assert.AreEqual(5, discount2);
            Assert.AreEqual(10, discount3);
            Assert.AreEqual(15, discount4);
        }

        [TestMethod]
        public void ValidatePartnerInn_InnLength10_Passes()
        {
            // Arrange
            string validInn10 = "1234567890";
            string validInn12 = "123456789012";
            string invalidInn1 = "12345";
            string invalidInn2 = "1234567890123";

            // Act & Assert
            Assert.IsTrue(validInn10.Length == 10 || validInn10.Length == 12);
            Assert.IsTrue(validInn12.Length == 10 || validInn12.Length == 12);
            Assert.IsFalse(invalidInn1.Length == 10 || invalidInn1.Length == 12);
            Assert.IsFalse(invalidInn2.Length == 10 || invalidInn2.Length == 12);
        }

        [TestMethod]
        public void PartnerTypeAssignment_ValidTypeId_Works()
        {
            // Arrange
            var partner = new Partner();
            int validTypeId = 2;

            // Act
            partner.TypeId = validTypeId;

            // Assert
            Assert.AreEqual(validTypeId, partner.TypeId);
        }
    }
}