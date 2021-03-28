using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WSUserAccountManager.Services.Cryptography;
using Xunit;

namespace WSUserAccountManager.UnitTests.Cryptography
{
    [ExcludeFromCodeCoverage]
    public class HashFunctionTests
    {
        private readonly HashFunction _sut;

        public HashFunctionTests()
        {
            _sut = new HashFunction();
        }

        [Fact]
        public void GetHashValue_WithEmptySecretKey_Should_ReturnEmptyString()
        {
            // Act
            var result = _sut.GetHashValue(string.Empty, string.Empty);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetHashValue_WithEmptyMessage_Should_ReturnEmptyString()
        {
            // Act
            var result = _sut.GetHashValue("superSecretKey", string.Empty);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetHashValue_Should_ReturnValue()
        {
            // Act
            var result = _sut.GetHashValue("superSecretKey", "message test");

            // Assert
            result.Should().NotBeNullOrEmpty();
        }
    }
}
