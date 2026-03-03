using SportowyHub.Models.Api;
using SportowyHub.Services.Api;
using SportowyHub.Services.Auth;

namespace SportowyHub.Services.PhoneVerification;

internal class PhoneVerificationService(IRequestProvider requestProvider, IAuthService authService) : IPhoneVerificationService
{
    public async Task<PhoneVerificationRequestResponse> RequestVerificationAsync(string phone, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PostAsync<PhoneVerificationRequestRequest, PhoneVerificationRequestResponse>(
            "/api/private/phone-verification/request",
            new PhoneVerificationRequestRequest(phone),
            token,
            ct: ct);
    }

    public async Task<PhoneVerificationResult> VerifyCodeAsync(string code, CancellationToken ct = default)
    {
        var token = await authService.GetTokenAsync() ?? "";
        return await requestProvider.PostAsync<PhoneVerificationVerifyRequest, PhoneVerificationResult>(
            "/api/private/phone-verification/verify",
            new PhoneVerificationVerifyRequest(code),
            token,
            ct: ct);
    }
}
