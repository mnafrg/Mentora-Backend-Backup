using Mentora.Domain.Entities.Classroom;

namespace Mentora.Application.Interfaces.Repositories.Classroom;

public interface ISubmissionReviewRepository
{
    Task<SubmissionReview?> GetBySubmissionIdAsync(int submissionId);
    Task CreateAsync(SubmissionReview review);
    Task UpdateAsync(SubmissionReview review);
}