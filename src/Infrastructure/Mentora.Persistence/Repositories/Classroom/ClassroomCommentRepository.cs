using Mentora.Application.Interfaces.Repositories.Classroom;
using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Classroom;

public class ClassroomCommentRepository : IClassroomCommentRepository
{
    private readonly ApplicationDbContext _context;

    public ClassroomCommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ClassroomComment?> GetByIdAsync(int commentId) =>
        await _context.ClassroomComments
            .Include(c => c.Author)
                .ThenInclude(u => u.MentorProfile)
            .Include(c => c.Author)
                .ThenInclude(u => u.MenteeProfile)
            .Include(c => c.Likes)
            .Include(c => c.Replies.Where(r => !r.IsDeleted))
                .ThenInclude(r => r.Author)
                    .ThenInclude(u => u.MentorProfile)
            .Include(c => c.Replies.Where(r => !r.IsDeleted))
                .ThenInclude(r => r.Author)
                    .ThenInclude(u => u.MenteeProfile)
            .Include(c => c.Replies.Where(r => !r.IsDeleted))
                .ThenInclude(r => r.Likes)
            .FirstOrDefaultAsync(c => c.CommentId == commentId && !c.IsDeleted);

    public async Task<List<ClassroomComment>> GetByPostIdAsync(int postId) =>
        await _context.ClassroomComments
            .Where(c => c.PostId == postId && !c.IsDeleted && c.ParentCommentId == null)
            .Include(c => c.Author)
                .ThenInclude(u => u.MentorProfile)
            .Include(c => c.Author)
                .ThenInclude(u => u.MenteeProfile)
            .Include(c => c.Likes)
            .Include(c => c.Replies.Where(r => !r.IsDeleted))
                .ThenInclude(r => r.Author)
                    .ThenInclude(u => u.MentorProfile)
            .Include(c => c.Replies.Where(r => !r.IsDeleted))
                .ThenInclude(r => r.Author)
                    .ThenInclude(u => u.MenteeProfile)
            .Include(c => c.Replies.Where(r => !r.IsDeleted))
                .ThenInclude(r => r.Likes)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync();

    public async Task CreateAsync(ClassroomComment comment) =>
        await _context.ClassroomComments.AddAsync(comment);

    public Task UpdateAsync(ClassroomComment comment)
    {
        _context.ClassroomComments.Update(comment);
        return Task.CompletedTask;
    }

    public async Task<ClassroomCommentLike?> GetLikeAsync(int commentId, Guid userId) =>
        await _context.ClassroomCommentLikes
            .FirstOrDefaultAsync(l => l.CommentId == commentId && l.UserId == userId);

    public async Task CreateLikeAsync(ClassroomCommentLike like) =>
        await _context.ClassroomCommentLikes.AddAsync(like);

    public Task DeleteLikeAsync(ClassroomCommentLike like)
    {
        _context.ClassroomCommentLikes.Remove(like);
        return Task.CompletedTask;
    }
}