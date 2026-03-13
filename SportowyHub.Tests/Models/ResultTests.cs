using FluentAssertions;
using SportowyHub.Models;

namespace SportowyHub.Tests.Models;

public class ResultTests
{
    [Fact]
    public void Success_SetsIsSuccessTrue_AndData()
    {
        var result = Result<string>.Success("hello");

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be("hello");
        result.ErrorMessage.Should().BeNull();
        result.FieldErrors.Should().BeNull();
        result.ErrorCode.Should().BeNull();
    }

    [Fact]
    public void Failure_SetsIsSuccessFalse_AndErrorMessage()
    {
        var result = Result<string>.Failure("Something went wrong");

        result.IsSuccess.Should().BeFalse();
        result.Data.Should().BeNull();
        result.ErrorMessage.Should().Be("Something went wrong");
        result.FieldErrors.Should().BeNull();
        result.ErrorCode.Should().BeNull();
    }

    [Fact]
    public void Failure_WithFieldErrors_SetsFieldErrors()
    {
        var fieldErrors = new Dictionary<string, string>
        {
            ["email"] = "Invalid email",
            ["phone"] = "Phone is required"
        };

        var result = Result<int>.Failure("Validation failed", fieldErrors);

        result.IsSuccess.Should().BeFalse();
        result.FieldErrors.Should().BeEquivalentTo(fieldErrors);
    }

    [Fact]
    public void Failure_WithErrorCode_SetsErrorCode()
    {
        var result = Result<bool>.Failure("Not verified", errorCode: "EMAIL_NOT_VERIFIED");

        result.IsSuccess.Should().BeFalse();
        result.ErrorCode.Should().Be("EMAIL_NOT_VERIFIED");
    }

    [Fact]
    public void Failure_WithAllParameters_SetsAllProperties()
    {
        var fieldErrors = new Dictionary<string, string> { ["name"] = "Required" };

        var result = Result<object>.Failure("Error", fieldErrors, "VALIDATION_ERROR");

        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Error");
        result.FieldErrors.Should().ContainKey("name");
        result.ErrorCode.Should().Be("VALIDATION_ERROR");
    }

    [Fact]
    public void Success_WithNullableType_AllowsNullData()
    {
        var result = Result<string?>.Success(null);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Fact]
    public void Success_WithValueType_SetsData()
    {
        var result = Result<int>.Success(42);

        result.IsSuccess.Should().BeTrue();
        result.Data.Should().Be(42);
    }

    [Fact]
    public void Result_IsRecord_SupportsEquality()
    {
        var result1 = Result<string>.Success("test");
        var result2 = Result<string>.Success("test");

        result1.Should().Be(result2);
    }

    [Fact]
    public void Result_IsRecord_DifferentValuesAreNotEqual()
    {
        var result1 = Result<string>.Success("a");
        var result2 = Result<string>.Success("b");

        result1.Should().NotBe(result2);
    }
}
