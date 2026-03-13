using Microsoft.Extensions.Logging;
using SportowyHub.Models;
using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.PhoneVerification;

internal class PhoneVerificationService(IRequestProvider requestProvider, ITokenProvider authService, ILogger<PhoneVerificationService> logger) : IPhoneVerificationService
{
    public async Task<Result<PhoneVerificationRequestResponse>> RequestVerificationAsync(string phone, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PostAsync<PhoneVerificationRequestRequest, PhoneVerificationRequestResponse>(
                "/api/private/phone-verification/request",
                new PhoneVerificationRequestRequest(phone),
                token,
                ct: ct);
            return Result<PhoneVerificationRequestResponse>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to request phone verification");
            return Result<PhoneVerificationRequestResponse>.Failure(ex.Message);
        }
    }

    public async Task<Result<PhoneVerificationResult>> VerifyCodeAsync(string code, CancellationToken ct = default)
    {
        try
        {
            var token = await authService.GetTokenAsync() ?? "";
            var response = await requestProvider.PostAsync<PhoneVerificationVerifyRequest, PhoneVerificationResult>(
                "/api/private/phone-verification/verify",
                new PhoneVerificationVerifyRequest(code),
                token,
                ct: ct);
            return Result<PhoneVerificationResult>.Success(response);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to verify phone code");
            return Result<PhoneVerificationResult>.Failure(ex.Message);
        }
    }
}
