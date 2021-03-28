using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;
using WSUserAccountManager.Abstractions;
using WSUserAccountManager.Database.Entities;
using WSUserAccountManager.Services.UserAccount;
using Xunit;

namespace WSUserAccountManager.UnitTests.Services.UserAccount
{
    [ExcludeFromCodeCoverage]
    public class SessionServiceTests
    {
        private readonly Mock<IRepository<Session>> _repositoryMock;

        private readonly SessionService _sut;

        public SessionServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Session>>();

            _sut = new SessionService(
                    _repositoryMock.Object
                );
        }

        #region Create

        [Fact]
        public async Task Create_WithDefaultUserAcctId_Should_ReturnNull()
        {
            // Act
            var result = await _sut.Create(default(int), 300);

            // Assert
            result.Should().BeNull();

            _repositoryMock.Verify(m => m.Save(It.IsAny<Session>()), Times.Never);
        }

        [Fact]
        public async Task Create_Successfully()
        {
            // Arrange
            var userAcctId = 1;
            _repositoryMock.Setup(m => m.Save(It.IsAny<Session>()))
                .ReturnsAsync(new Session());

            // Act
            var result = await _sut.Create(userAcctId, 300);

            // Assert
            result.Should().NotBeNull();

            _repositoryMock.Verify(m => m.Save(It.IsAny<Session>()));
        }

        #endregion
    }
}
