using Mentora.Application.DTOs.Common;

namespace Mentora.Application.Interfaces.Services.Community;

public interface ICommunityShareService
{
    Task<ApiResponse<string>> SharePostAsync(Guid postId, Guid userId);
}
