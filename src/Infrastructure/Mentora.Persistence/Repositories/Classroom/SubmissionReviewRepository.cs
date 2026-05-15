using Mentora.Application.Interfaces.Repositories.Classroom;
using Mentora.Domain.Entities.Classroom;
using Microsoft.EntityFrameworkCore;

namespace Mentora.Persistence.Repositories.Classroom;

public class SubmissionReviewRepository : ISubmissionReviewRepository
{
    private readonly ApplicationDbContext _context;

    public SubmissionReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SubmissionReview?> GetBySubmissionIdAsync(int submissionId) =>
        await _context.SubmissionReviews
            .FirstOrDefaultAsync(r => r.SubmissionId == submissionId);

    public async Task CreateAsync(SubmissionReview review)
    {
        await _context.SubmissionReviews.AddAsync(review);
    }

    public Task UpdateAsync(SubmissionReview review)
    {
        _context.SubmissionReviews.Update(review);
        return Task.CompletedTask;
    }
}