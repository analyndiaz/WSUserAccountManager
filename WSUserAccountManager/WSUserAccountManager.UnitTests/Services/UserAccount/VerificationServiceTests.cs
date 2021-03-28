using Moq;
using System.Diagnostics.CodeAnalysis;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Database.Entities;
using WSUserAccountManager.Services.UserAccount;
using Xunit;
using System.Threading.Tasks;
using System;
using FluentAssertions;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace WSUserAccountManager.UnitTests.Services.UserAccount
{
    [ExcludeFromCodeCoverage]
    public class VerificationServiceTests
    {
        private readonly Mock<IRepository<VerificationCode>> _repositoryMock;

        private readonly VerificationService _sut;

        public VerificationServiceTests()
        {
            _repositoryMock = new Mock<IRepository<VerificationCode>>();

            _sut = new VerificationService(
                    _repositoryMock.Object
                );
        }

        #region SaveCode

        [Fact]
        public void SaveCode_WithNullUserAccount_Should_ThrowException()
        {
            // Act
            Func<Task> act = () => _sut.SaveCode(1, null as Models.UserAccount);

            // Assert
            act.Should().Throw<ArgumentNullException>("userAccount");
        }

        [Fact]
        public async Task SaveCode_Successfully()
        {
            // Arrange
            _repositoryMock.Setup(m => m.Save(It.IsAny<VerificationCode>()))
               .ReturnsAsync(new VerificationCode());

            // Act
            await _sut.SaveCode(1, new Models.UserAccount());

            // Assert
            _repositoryMock.Verify(m => m.Save(It.IsAny<VerificationCode>()));
        }

        #endregion

        #region Verify

        [Fact]
        public async Task Verify_WithExceedingExistingCodes_Should_ReturnFalse()
        {
            // Arrange
            _repositoryMock.Setup(m => m.GetAll(
                    It.IsAny<Expression<Func<VerificationCode, bool>>>()))
                .ReturnsAsync(GetExceedingCodes());

            // Act
            var result = await _sut.Verify(new Database.Entities.UserAccount());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Verify_WithNullSavedCode_Should_ReturnFalse()
        {
            // Arrange
            _repositoryMock.Setup(m => m.GetAll(
                    It.IsAny<Expression<Func<VerificationCode, bool>>>()))
                .ReturnsAsync(new List<VerificationCode>());
            _repositoryMock.Setup(m => m.Save(It.IsAny<VerificationCode>()))
              .ReturnsAsync(null as VerificationCode);

            // Act
            var result = await _sut.Verify(new Database.Entities.UserAccount());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Verify_WithSavedValidCode_Should_ReturnTrue()
        {
            // Arrange
            _repositoryMock.Setup(m => m.GetAll(
                    It.IsAny<Expression<Func<VerificationCode, bool>>>()))
                .ReturnsAsync(new List<VerificationCode>());
            _repositoryMock.Setup(m => m.Save(It.IsAny<VerificationCode>()))
              .ReturnsAsync(new VerificationCode());

            // Act
            var result = await _sut.Verify(new Database.Entities.UserAccount());

            // Assert
            result.Should().BeTrue();
        }

        #endregion

        #region MockData

        private List<VerificationCode> GetExceedingCodes()
        {
            var verificationCodes = new List<VerificationCode>();

            verificationCodes.Add(new VerificationCode() { VerificationCodeId = 1});
            verificationCodes.Add(new VerificationCode() { VerificationCodeId = 2 });
            verificationCodes.Add(new VerificationCode() { VerificationCodeId = 3 });
            verificationCodes.Add(new VerificationCode() { VerificationCodeId = 4 });
            verificationCodes.Add(new VerificationCode() { VerificationCodeId = 5 });

            return verificationCodes;
        }


        #endregion
    }
}
