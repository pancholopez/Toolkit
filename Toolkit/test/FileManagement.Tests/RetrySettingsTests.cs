using System;
using FileManagement.Core;
using Xunit;

namespace FileManagement.Tests
{
    public class RetrySettingsTests
    {
        [Fact]
        public void CanCreateValidInstance()
        {
            var limit = new Random().Next(100);
            var elapsed = new Random().Next(100);

            var settings = new RetrySettings(limit, elapsed);

            Assert.Equal(limit, settings.Limit);
            Assert.Equal(elapsed, settings.ElapsedMilliseconds);
        }

        [Fact]
        public void NegativeLimit_ThrowsException()
        {
            var error = Assert.Throws<ArgumentOutOfRangeException>(() => new RetrySettings(-1, 0));
            Assert.Equal("limit",error.ParamName);
        }

        [Fact]
        public void NegativeElapsedMilliseconds_ThrowsException()
        {
            var error = Assert.Throws<ArgumentOutOfRangeException>(() => new RetrySettings(0, -1));
            Assert.Equal("elapsedMilliseconds", error.ParamName);
        }
    }
}