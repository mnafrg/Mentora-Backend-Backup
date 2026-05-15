using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Repositories.Interactions;
using Mentora.Application.Interfaces.Repositories.Classroom;
using Mentora.Application.Interfaces.Repositories.Community;
namespace Mentora.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IMenteeProfileRepository MenteeProfiles { get; }
        IMentorProfileRepository MentorProfiles { get; }
        IEmailVerificationTokenRepository EmailVerificationTokens { get; }
        ILookupRepository Lookups { get; }
        IRegistrationSessionRepository RegistrationSessions { get; } 

        IRefreshTokenRepository RefreshTokens { get; }
        IPasswordResetTokenRepository PasswordResetTokens { get; }
        IProgramRepository Programs { get; }
        IRoadmapRepository Roadmaps { get; }
        IRoadmapPhaseRepository RoadmapPhases { get; }
        IRoadmapTopicRepository Topics { get; }
        IMaterialRepository Materials { get; }
        IRoadmapTaskRepository Tasks { get; }
        ISavedRoadmapRepository SavedRoadmaps { get; }
        IFeedbackRepository Feedbacks { get; }
        IMentorshipRepository Mentorships { get; }
        IPostLikeRepository PostLikes { get; }
         ISavedPostRepository SavedPosts { get; }
        ISharedPostRepository SharedPosts { get; }
        ICommentRepository Comments { get; }
        IApplicationRepository MentorshipApplications { get; }
        IMentorshipRequirementRepository MentorshipRequirements { get; }
        IMenteeInterestRepository MenteeInterests { get; }  


        // Profile related repositories
        IAchievementRepository Achievements { get; }
        IUserEducationRepository UserEducations { get; }
        IProfileLinkRepository ProfileLinks { get; }
        IFollowRepository Follows { get; }
        IReportedItemRepository ReportedItems { get; }
        IReportRepository Reports { get; }
        IUserBanRepository UserBans  { get; }
        IUserWarningRepository UserWarnings { get; }
        
        // Classroom related repositories
        IClassroomRepository Classrooms { get; }
        ISessionRepository Sessions { get; }
       
        IClassroomPostRepository ClassroomPosts { get; }
        IClassroomCommentRepository ClassroomComments { get; }
        
        ITaskSubmissionRepository TaskSubmissions { get; }
        ISubmissionReviewRepository SubmissionReviews { get; }
        IMaterialCompletionRepository MaterialCompletions { get; }

        // Community related repositories
        ICommunityRepository Communities { get; }
        ICommunityMemberRepository CommunityMembers { get; }
        ICommunityPostRepository CommunityPosts { get; }
        ICommunityCommentRepository CommunityComments { get; }
        ICommunityPostLikeRepository CommunityPostLikes { get; }
        ICommunityPostShareRepository CommunityPostShares { get; }
        ICommunityPostSaveRepository CommunityPostSaves { get; }
        ICommunityReportRepository CommunityReports { get; }
    
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}