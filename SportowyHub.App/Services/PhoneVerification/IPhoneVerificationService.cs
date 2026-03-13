using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.PhoneVerification;

public interface IPhoneVerificationService
{
    Task<Result<PhoneVerificationRequestResponse>> RequestVerificationAsync(string phone, CancellationToken ct = default);
    Task<Result<PhoneVerificationResult>> VerifyCodeAsync(string code, CancellationToken ct = default);
}
