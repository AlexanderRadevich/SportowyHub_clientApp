using System.Globalization;
using FluentAssertions;
using SportowyHub.Converters;

namespace SportowyHub.Tests.Converters;

public class ViewCountFormatConverterTests
{
    private readonly ViewCountFormatConverter _sut = new();

    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(42, "42")]
    [InlineData(999, "999")]
    [InlineData(1000, "1k")]
    [InlineData(1234, "1.2k")]
    [InlineData(1900, "1.9k")]
    [InlineData(10000, "10k")]
    [InlineData(999950, "1M")]
    [InlineData(999999, "1M")]
    [InlineData(1000000, "1M")]
    [InlineData(1500000, "1.5M")]
    [InlineData(10000000, "10M")]
    public void Convert_FormatsCorrectly(int input, string expected)
    {
        var result = _sut.Convert(input, typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().Be(expected);
    }

    [Fact]
    public void Convert_WhenNull_ReturnsZero()
    {
        var result = _sut.Convert(null, typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().Be("0");
    }

    [Fact]
    public void Convert_WhenNotInt_ReturnsZero()
    {
        var result = _sut.Convert("abc", typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().Be("0");
    }

    [Fact]
    public void Convert_WhenNegative_ReturnsZero()
    {
        var result = _sut.Convert(-5, typeof(string), null, CultureInfo.InvariantCulture);

        result.Should().Be("0");
    }
}
