namespace SportowyHub.Models.Api;

public record PhoneVerificationRequestRequest(string Phone);

public record PhoneVerificationRequestResponse(string Message, string ExpiresAt);

public record PhoneVerificationVerifyRequest(string Code);

public record PhoneVerificationResult(string Message, string TrustLevel);
