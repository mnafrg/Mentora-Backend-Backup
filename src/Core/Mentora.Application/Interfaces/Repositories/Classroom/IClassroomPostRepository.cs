using Mentora.Domain.Entities.Classroom;

namespace Mentora.Application.Interfaces.Repositories.Classroom;

public interface IClassroomPostRepository
{
    Task<ClassroomPost?> GetByIdAsync(int postId);
    Task<(int ClassroomId, Guid AuthorId)?> GetOwnershipAsync(int postId);

    /// Feed: pinned posts first, then newest. Supports pagination.
    Task<(List<ClassroomPost> Posts, int Total)> GetFeedAsync(
        int classroomId, int skip, int take);

    Task CreateAsync(ClassroomPost post);
    Task UpdateAsync(ClassroomPost post);

    // Likes
    Task<ClassroomPostLike?> GetLikeAsync(int postId, Guid userId);
    Task CreateLikeAsync(ClassroomPostLike like);
    Task DeleteLikeAsync(ClassroomPostLike like);
}