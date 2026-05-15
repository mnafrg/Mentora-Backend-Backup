using Mentora.Domain.Entities.Classroom;

namespace Mentora.Application.Interfaces.Repositories.Classroom;

public interface IClassroomCommentRepository
{
    Task<ClassroomComment?> GetByIdAsync(int commentId);

    /// All top-level comments for a post, ordered oldest-first, with replies nested.
    Task<List<ClassroomComment>> GetByPostIdAsync(int postId);

    Task CreateAsync(ClassroomComment comment);
    Task UpdateAsync(ClassroomComment comment);

    // Likes
    Task<ClassroomCommentLike?> GetLikeAsync(int commentId, Guid userId);
    Task CreateLikeAsync(ClassroomCommentLike like);
    Task DeleteLikeAsync(ClassroomCommentLike like);
}