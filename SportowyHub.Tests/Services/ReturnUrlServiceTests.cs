using FluentAssertions;
using SportowyHub.Services.Navigation;

namespace SportowyHub.Tests.Services;

public class ReturnUrlServiceTests
{
    private readonly ReturnUrlService _sut = new();

    [Fact]
    public void HasReturnUrl_WhenNoUrlSet_ReturnsFalse()
    {
        _sut.HasReturnUrl.Should().BeFalse();
    }

    [Fact]
    public void HasReturnUrl_AfterSetReturnUrl_ReturnsTrue()
    {
        _sut.SetReturnUrl("//home");

        _sut.HasReturnUrl.Should().BeTrue();
    }

    [Fact]
    public void ConsumeReturnUrl_WhenUrlSet_ReturnsUrlAndClears()
    {
        _sut.SetReturnUrl("//favorites");

        var result = _sut.ConsumeReturnUrl();

        result.Should().Be("//favorites");
        _sut.HasReturnUrl.Should().BeFalse();
    }

    [Fact]
    public void ConsumeReturnUrl_WhenNoUrlSet_ReturnsNull()
    {
        var result = _sut.ConsumeReturnUrl();

        result.Should().BeNull();
    }

    [Fact]
    public void ConsumeReturnUrl_CalledTwice_SecondCallReturnsNull()
    {
        _sut.SetReturnUrl("//profile");

        _sut.ConsumeReturnUrl();
        var secondResult = _sut.ConsumeReturnUrl();

        secondResult.Should().BeNull();
    }

    [Fact]
    public void SetReturnUrl_CalledTwice_OverwritesPreviousValue()
    {
        _sut.SetReturnUrl("//home");
        _sut.SetReturnUrl("//favorites");

        var result = _sut.ConsumeReturnUrl();

        result.Should().Be("//favorites");
    }
}
