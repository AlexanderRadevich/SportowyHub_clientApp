using SportowyHub.Models;
using SportowyHub.Models.Api;

namespace SportowyHub.Services.Auth;

public interface IProfileService
{
    Task<Result<UserProfile>> GetProfileAsync(CancellationToken ct = default);
    Task<Result<UpdateProfileResponse>> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken ct = default);
    Task<Result<AvatarResponse>> UploadAvatarAsync(Stream imageStream, string fileName, CancellationToken ct = default);
    Task<Result<bool>> DeleteAvatarAsync(CancellationToken ct = default);
}
