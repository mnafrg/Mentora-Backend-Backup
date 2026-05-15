
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Repositories.Classroom;
using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Application.Interfaces.Repositories.Interactions;
using Mentora.Persistence.Repositories;
using Mentora.Persistence.Repositories.Classroom;
using Mentora.Persistence.Repositories.Community;
using Mentora.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;


namespace Mentora.Persistence;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        MenteeProfiles = new MenteeProfileRepository(_context);
        MentorProfiles = new MentorProfileRepository(_context);
        EmailVerificationTokens = new EmailVerificationTokenRepository(_context);
        RefreshTokens = new RefreshTokenRepository(_context);
        PasswordResetTokens = new PasswordResetTokenRepository(_context);
        Lookups = new LookupRepository(_context);


        // Profile related repositories
        Achievements = new AchievementRepository(_context);
        UserEducations = new UserEducationRepository(_context);
        ProfileLinks = new ProfileLinkRepository(_context);
        Follows = new FollowRepository(_context);

        // Report related repositories
        ReportedItems = new ReportedItemRepository(_context);
        Reports = new ReportRepository(_context);
        UserBans = new UserBanRepository(_context);
        UserWarnings = new UserWarningRepository(_context);


        RegistrationSessions = new RegistrationSessionRepository(_context);

        Roadmaps = new RoadmapRepository(_context);
        RoadmapPhases = new RoadmapPhaseRepository(_context);
        Topics = new RoadmapTopicRepository(_context);
        Materials = new MaterialRepository(_context);
        Tasks = new RoadmapTaskRepository(_context);
        SavedRoadmaps = new SavedRoadmapRepository(_context);

        Programs = new ProgramRepository(_context);
        Feedbacks = new FeedbackRepository(_context);
        Mentorships = new MentorshipRepository(_context);
        PostLikes = new PostLikeRepository(_context);
        SavedPosts = new SavedPostRepository(_context);
        SharedPosts = new SharedPostRepository(_context);
        Comments = new CommentRepository(_context);
        MentorshipApplications = new ApplicationRepository(_context);
        MentorshipRequirements = new MentorshipRequirementRepository(_context);
        MenteeInterests = new MenteeInterestRepository(_context);
        ProfileLinks = new ProfileLinkRepository(_context);

        // Classroom related repositories
        Classrooms = new ClassroomRepository(_context);
        Sessions = new SessionRepository(_context);

        ClassroomPosts    = new ClassroomPostRepository(_context);
        ClassroomComments = new ClassroomCommentRepository(_context);

        TaskSubmissions   = new TaskSubmissionRepository(_context);
        SubmissionReviews = new SubmissionReviewRepository(_context);

        MaterialCompletions = new MaterialCompletionRepository(_context);

        // Community related repositories
        Communities = new CommunityRepository(_context);
        CommunityMembers = new CommunityMemberRepository(_context);
        CommunityPosts = new CommunityPostRepository(_context);
        CommunityComments = new CommunityCommentRepository(_context);
        CommunityPostLikes = new CommunityPostLikeRepository(_context);
        CommunityPostShares = new CommunityPostShareRepository(_context);
        CommunityPostSaves = new CommunityPostSaveRepository(_context);
        CommunityReports = new CommunityReportRepository(_context);
    }

    public IUserRepository Users { get; }
    public IMenteeProfileRepository MenteeProfiles { get; }
    public IMentorProfileRepository MentorProfiles { get; }
    public IEmailVerificationTokenRepository EmailVerificationTokens { get; }
    public IRefreshTokenRepository RefreshTokens { get; }
    public IPasswordResetTokenRepository PasswordResetTokens { get; }
    public ILookupRepository Lookups { get; }
    public IRegistrationSessionRepository RegistrationSessions { get; }


    // Profile related repositories
    public IAchievementRepository Achievements { get; }
    public IUserEducationRepository UserEducations { get; }
    public IProfileLinkRepository ProfileLinks { get; }
    public IFollowRepository Follows { get; }
    public IReportedItemRepository ReportedItems { get; }
    public IReportRepository Reports { get; }
    public IUserBanRepository UserBans { get; }
    public IUserWarningRepository UserWarnings { get; }
 

    public IRoadmapRepository Roadmaps { get; }
    public IRoadmapPhaseRepository RoadmapPhases { get; }
    public IRoadmapTopicRepository Topics { get; }
     
    public IMaterialRepository Materials { get; }
    public IRoadmapTaskRepository Tasks { get; }
     public ISavedRoadmapRepository SavedRoadmaps { get; }



    public IProgramRepository Programs { get; }
    public IFeedbackRepository Feedbacks { get; }
    public IMentorshipRepository Mentorships { get; }
    public IPostLikeRepository PostLikes { get; }
    public ISavedPostRepository SavedPosts { get; }
    public ISharedPostRepository SharedPosts { get; }
    public ICommentRepository Comments { get; }
    public IApplicationRepository MentorshipApplications { get; }
    public IMentorshipRequirementRepository MentorshipRequirements { get; }
    public IMenteeInterestRepository MenteeInterests { get; }
    
    // ClassRoom 
    public IClassroomRepository Classrooms  { get; }
    public ISessionRepository Sessions { get; }

    public IClassroomPostRepository ClassroomPosts { get; }
    public IClassroomCommentRepository ClassroomComments { get; }

    public ITaskSubmissionRepository TaskSubmissions { get; }
    public ISubmissionReviewRepository SubmissionReviews { get; }
    public IMaterialCompletionRepository MaterialCompletions { get; }

    // Community related repositories
    public ICommunityRepository Communities { get; }
    public ICommunityMemberRepository CommunityMembers { get; }
    public ICommunityPostRepository CommunityPosts { get; }
    public ICommunityCommentRepository CommunityComments { get; }
    public ICommunityPostLikeRepository CommunityPostLikes { get; }
    public ICommunityPostShareRepository CommunityPostShares { get; }
    public ICommunityPostSaveRepository CommunityPostSaves { get; }
    public ICommunityReportRepository CommunityReports { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}