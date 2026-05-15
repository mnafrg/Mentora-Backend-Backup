using Mentora.Application.Interfaces.Repositories.Classroom;
using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Classroom;

public class ClassroomPostRepository : IClassroomPostRepository
{
    private readonly ApplicationDbContext _context;

    public ClassroomPostRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ClassroomPost?> GetByIdAsync(int postId) =>
        await _context.ClassroomPosts
            .Include(p => p.Author)
                .ThenInclude(u => u.MentorProfile)
            .Include(p => p.Author)
                .ThenInclude(u => u.MenteeProfile)
            .Include(p => p.Likes)
            .Include(p => p.Comments.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.Author)
                    .ThenInclude(u => u.MentorProfile)
            .Include(p => p.Comments.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.Author)
                    .ThenInclude(u => u.MenteeProfile)
            .Include(p => p.Comments.Where(c => !c.IsDeleted))
                .ThenInclude(c => c.Likes)
            .FirstOrDefaultAsync(p => p.PostId == postId && !p.IsDeleted);

    public async Task<(int ClassroomId, Guid AuthorId)?> GetOwnershipAsync(int postId)
    {
        var post = await _context.ClassroomPosts
            .Where(p => p.PostId == postId && !p.IsDeleted)
            .Select(p => new { p.ClassroomId, p.AuthorId })
            .FirstOrDefaultAsync();

        return post == null ? null : (post.ClassroomId, post.AuthorId);
    }

    public async Task<(List<ClassroomPost> Posts, int Total)> GetFeedAsync(
        int classroomId, int skip, int take)
    {
        var query = _context.ClassroomPosts
            .Where(p => p.ClassroomId == classroomId && !p.IsDeleted)
            .Include(p => p.Author)
                .ThenInclude(u => u.MentorProfile)
            .Include(p => p.Author)
                .ThenInclude(u => u.MenteeProfile)
            .Include(p => p.Likes)
            .Include(p => p.Comments.Where(c => !c.IsDeleted && c.ParentCommentId == null))
                .ThenInclude(c => c.Author)
                    .ThenInclude(u => u.MentorProfile)
            .Include(p => p.Comments.Where(c => !c.IsDeleted && c.ParentCommentId == null))
                .ThenInclude(c => c.Author)
                    .ThenInclude(u => u.MenteeProfile)
            .Include(p => p.Comments.Where(c => !c.IsDeleted && c.ParentCommentId == null))
                .ThenInclude(c => c.Likes)
            // Pinned posts first, then newest
            .OrderByDescending(p => p.IsPinned)
            .ThenByDescending(p => p.CreatedAt);

        var total = await query.CountAsync();
        var posts  = await query.Skip(skip).Take(take).ToListAsync();

        return (posts, total);
    }

    public async Task CreateAsync(ClassroomPost post) =>
        await _context.ClassroomPosts.AddAsync(post);

    public Task UpdateAsync(ClassroomPost post)
    {
        _context.ClassroomPosts.Update(post);
        return Task.CompletedTask;
    }

    public async Task<ClassroomPostLike?> GetLikeAsync(int postId, Guid userId) =>
        await _context.ClassroomPostLikes
            .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

    public async Task CreateLikeAsync(ClassroomPostLike like) =>
        await _context.ClassroomPostLikes.AddAsync(like);

    public Task DeleteLikeAsync(ClassroomPostLike like)
    {
        _context.ClassroomPostLikes.Remove(like);
        return Task.CompletedTask;
    }
}