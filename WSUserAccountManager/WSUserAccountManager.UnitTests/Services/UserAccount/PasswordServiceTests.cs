using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Database.Entities;
using WSUserAccountManager.Services.UserAccount;
using Xunit;

namespace WSUserAccountManager.UnitTests.Services.UserAccount
{
    [ExcludeFromCodeCoverage]
    public class PasswordServiceTests
    {
        private readonly Mock<IHashFunction> _hashFunctionMock;
        private readonly Mock<IRepository<Password>> _repositoryMock;
        private readonly Mock<ISaltService> _saltServiceMock;

        private readonly PasswordService _sut;

        public PasswordServiceTests()
        {
            _hashFunctionMock = new Mock<IHashFunction>();
            _repositoryMock = new Mock<IRepository<Password>>();
            _saltServiceMock = new Mock<ISaltService>();

            _sut = new PasswordService(
                    _hashFunctionMock.Object,
                    _repositoryMock.Object,
                    _saltServiceMock.Object
                );
        }

        #region GetChallenge

        [Fact]
        public async Task GetChallenge_WithNullValidSalt_Should_ReturnEmptyString()
        {
            // Arrange
            var userName = "Test";

            _saltServiceMock.Setup(m => m.GetValidSalt(userName))
                .ReturnsAsync(null as Salt);

            // Act
            var result = await _sut.GetChallenge(userName, true);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetChallenge_WithValidSalt_Should_ReturnHashValue()
        {
            // Arrange
            var userName = "Test";
            var retHashValue = "hash123";

            _saltServiceMock.Setup(m => m.GetValidSalt(userName))
                .ReturnsAsync(new Salt() { Value = "salt123"});

            _repositoryMock.Setup(m => m.GetAll(It.IsAny<Expression<Func<Password, bool>>>()))
                .ReturnsAsync(new List<Password>()
                {
                    new Password()
                    {
                        Value = "password123"
                    }
                });

            _hashFunctionMock.Setup(m => m.GetHashValue(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(retHashValue);

            // Act
            var result = await _sut.GetChallenge(userName, true);

            // Assert
            result.Should().Be(retHashValue);
        }

        #endregion

        #region Save

        [Fact]
        public void Save_WithNullUserAccount_Should_ThrowNullException()
        {
            // Act
            Func<Task> act = () => _sut.Save(1, null as Models.UserAccount);

            // Assert
            act.Should().Throw<ArgumentNullException>("userAccount");
        }

        [Fact]
        public async Task Save_WithValidInputs_Should_SaveSuccessfully()
        {
            // Arrange
            var userAcct = new Models.UserAccount()
            {
               Password = "pass123",
               SecondaryPassword = "pass456"
            };

            _hashFunctionMock
               .Setup(m => m.GetHashValue(It.IsAny<string>(), It.IsAny<string>()))
               .Returns((string secretKey, string pass) => pass);

            // Act
            await _sut.Save(1, userAcct);

            // Assert
            _repositoryMock.Verify(m => m.Save(It.Is<Password>(p => p.Value == userAcct.Password)), Times.Once);
            _repositoryMock.Verify(m => m.Save(It.Is<Password>(p => p.Value == userAcct.SecondaryPassword)), Times.Once);
        }

        #endregion
    }
}
