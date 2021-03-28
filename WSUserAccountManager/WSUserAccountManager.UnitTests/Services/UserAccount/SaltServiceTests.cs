using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Database.Entities;
using WSUserAccountManager.Services.UserAccount;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq.Expressions;

namespace WSUserAccountManager.UnitTests.Services.UserAccount
{
    [ExcludeFromCodeCoverage]
    public class SaltServiceTests
    {
        private readonly Mock<IRepository<Salt>> _repositoryMock;

        private readonly SaltService _sut;

        public SaltServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Salt>>();

            _sut = new SaltService(
                    _repositoryMock.Object
                );
        }

        #region Create

        [Fact]
        public async Task Create_WithDefaultUserAcctId_Should_ReturnNull() 
        {
            // Act
            var result = await _sut.Create(default(int));

            // Assert
            result.Should().BeNull();

            _repositoryMock.Verify(m => m.Save(It.IsAny<Salt>()), Times.Never);
        }

        [Fact]
        public async Task Create_Successfully()
        {
            // Arrange
            var userAcctId = 1;
            _repositoryMock.Setup(m => m.Save(It.IsAny<Salt>()))
                .ReturnsAsync(new Salt());

            // Act
            var result = await _sut.Create(userAcctId);

            // Assert
            result.Should().NotBeNull();

            _repositoryMock.Verify(m => m.Save(It.IsAny<Salt>()));
        }

        #endregion

        #region GetValidSalt

        [Fact]
        public async Task GetValidSalt_WithNoSaltRecord_Should_ReturnNull()
        {
            // Arrange
            _repositoryMock.Setup(m => m.GetAll(
                    It.IsAny<Expression<Func<Salt, bool>>>()))
                .ReturnsAsync(new List<Salt>());

            // Act
            var result = await _sut.GetValidSalt("Test User");

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetValidSalt_Should_ReturnValidSalt()
        {
            // Arrange
            _repositoryMock.Setup(m => m.GetAll(
                    It.IsAny<Expression<Func<Salt, bool>>>()))
                .ReturnsAsync(new List<Salt>() { 
                    new Salt() { CreatedTime = DateTime.Now.AddSeconds(300) } 
                });

            // Act
            var result = await _sut.GetValidSalt("Test User");

            // Assert
            result.Should().NotBeNull();
        }

        #endregion

    }
}
