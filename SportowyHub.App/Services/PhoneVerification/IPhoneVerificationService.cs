using SportowyHub.Models.Api;

namespace SportowyHub.Services.PhoneVerification;

public interface IPhoneVerificationService
{
    Task<PhoneVerificationRequestResponse> RequestVerificationAsync(string phone, CancellationToken ct = default);
    Task<PhoneVerificationResult> VerifyCodeAsync(string code, CancellationToken ct = default);
}
