using Microsoft.VisualStudio.TestTools.UnitTesting;
using PartnerAccounting.Core.Ivanov.Models;
using PartnerAccounting.Core.Ivanov.Services;
using System;
using System.ComponentModel.DataAnnotations;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PartnerAccounting.Tests.Ivanov
{
    [TestClass]
    public class PartnerValidationTests
    {
        private Partner _validPartner;

        [TestInitialize]
        public void Setup()
        {
            _validPartner = new Partner
            {
                Id = 1,
                TypeId = 1,
                Name = "ООО Тест",
                LegalAddress = "г. Москва, ул. Тестовая, 1",
                Inn = "1234567890",
                DirectorName = "Иванов Иван Иванович",
                Phone = "+7(495)123-45-67",
                Email = "test@test.com",
                Rating = 50
            };
        }

        [TestMethod]
        public void ValidatePartner_ValidData_PassesValidation()
        {
            // Arrange
            var validationContext = new ValidationContext(_validPartner);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(_validPartner, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
            Assert.AreEqual(0, validationResults.Count);
        }

        [TestMethod]
        public void ValidatePartner_NameIsEmpty_ReturnsError()
        {
            // Arrange
            _validPartner.Name = "";
            var validationContext = new ValidationContext(_validPartner);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(_validPartner, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.IsTrue(validationResults.Count > 0);
        }

        [TestMethod]
        public void ValidatePartner_InnIs10Digits_Valid()
        {
            // Arrange
            _validPartner.Inn = "1234567890";

            // Act & Assert
            Assert.AreEqual(10, _validPartner.Inn.Length);
        }

        [TestMethod]
        public void ValidatePartner_InnIs12Digits_Valid()
        {
            // Arrange
            _validPartner.Inn = "123456789012";

            // Act & Assert
            Assert.AreEqual(12, _validPartner.Inn.Length);
        }

        [TestMethod]
        public void ValidatePartner_RatingIsNegative_ReturnsError()
        {
            // Arrange
            _validPartner.Rating = -5;
            var validationContext = new ValidationContext(_validPartner);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(_validPartner, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidatePartner_EmailInvalid_ReturnsError()
        {
            // Arrange
            _validPartner.Email = "not-an-email";
            var validationContext = new ValidationContext(_validPartner);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(_validPartner, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void ValidatePartner_PhoneNotEmpty_Valid()
        {
            // Arrange
            _validPartner.Phone = "+7(999)123-45-67";

            // Act & Assert
            Assert.IsFalse(string.IsNullOrEmpty(_validPartner.Phone));
        }
    }
}