using FluentValidation;
using FluentValidation.AspNetCore;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Application.Interfaces.Repositories.Classroom;
using Mentora.Application.Interfaces.Services;
using Mentora.Application.Interfaces.Services.Classroom;
using Mentora.Application.Services;
using Mentora.Application.Services.Classroom;
using Mentora.Application.Interfaces.Services.Community;
using Mentora.Application.Services.Community;
using Mentora.Application.Services.DashboardServices;
using Mentora.Application.Validators;
using Mentora.Infrastructure.Configuration;
using Mentora.Infrastructure.Services;
using Mentora.Persistence;
using Mentora.Persistence.Repositories;
using Mentora.Persistence.Repositories.Classroom;
using Mentora.Application.Interfaces.Repositories.Community;
using Mentora.Persistence.Repositories.Community;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;




//System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
var builder = WebApplication.CreateBuilder(args);
// Add Configuration
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add Repositories and Unit of Work
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMenteeProfileRepository, MenteeProfileRepository>();
builder.Services.AddScoped<IMentorProfileRepository, MentorProfileRepository>();
builder.Services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
builder.Services.AddScoped<ILookupRepository, LookupRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
builder.Services.AddScoped<IProgramRepository, ProgramRepository>();
builder.Services.AddScoped<IRoadmapRepository, RoadmapRepository>();
builder.Services.AddScoped<IRoadmapPhaseRepository, RoadmapPhaseRepository>();
builder.Services.AddScoped<IRoadmapTopicRepository, RoadmapTopicRepository>();
builder.Services.AddScoped<IRoadmapTaskRepository, RoadmapTaskRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<ISavedRoadmapRepository, SavedRoadmapRepository>();

builder.Services.AddScoped<IMentorshipRepository, MentorshipRepository>();
builder.Services.AddScoped<IPostLikeRepository, PostLikeRepository>();
builder.Services.AddScoped<ISavedPostRepository, SavedPostRepository>();
builder.Services.AddScoped<ISharedPostRepository, SharedPostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IMentorshipRequirementRepository, MentorshipRequirementRepository>();
builder.Services.AddScoped<IMenteeInterestRepository, MenteeInterestRepository>();
builder.Services.AddScoped<IProfileLinkRepository, ProfileLinkRepository>();
builder.Services.AddScoped<IClassroomRepository, ClassroomRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IClassroomPostRepository, ClassroomPostRepository>();
builder.Services.AddScoped<IClassroomCommentRepository, ClassroomCommentRepository>();
builder.Services.AddScoped<ITaskSubmissionRepository, TaskSubmissionRepository>();
builder.Services.AddScoped<ISubmissionReviewRepository, SubmissionReviewRepository>();
builder.Services.AddScoped<IMaterialCompletionRepository, MaterialCompletionRepository>();

// Community Repositories
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();
builder.Services.AddScoped<ICommunityMemberRepository, CommunityMemberRepository>();
builder.Services.AddScoped<ICommunityPostRepository, CommunityPostRepository>();
builder.Services.AddScoped<ICommunityCommentRepository, CommunityCommentRepository>();
builder.Services.AddScoped<ICommunityPostLikeRepository, CommunityPostLikeRepository>();
builder.Services.AddScoped<ICommunityPostShareRepository, CommunityPostShareRepository>();
builder.Services.AddScoped<ICommunityPostSaveRepository, CommunityPostSaveRepository>();
builder.Services.AddScoped<ICommunityReportRepository, CommunityReportRepository>();

// Add Application Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IMentorDashboardServices, MentorDashboardServices>();
builder.Services.AddScoped<IMenteeDashboardServices, MenteeDashboardServices>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IProgramMentorService, ProgramMentorService>();
builder.Services.AddScoped<IProgramMenteeService, ProgramMenteeService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IApplicantsService, ApplicantsService>();
builder.Services.AddScoped<IRoadmapService, RoadmapService>();
builder.Services.AddScoped<IExploreService, ExploreService>();







builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IClassroomFeedService, ClassroomFeedService>();
builder.Services.AddScoped<ITaskSubmissionService, TaskSubmissionService>();
builder.Services.AddScoped<IMaterialCompletionService, MaterialCompletionService>();
builder.Services.AddScoped<IMentorClassroomDashboardService, MentorClassroomDashboardService>();

// Community Services
builder.Services.AddScoped<ICommunityService, CommunityService>();
builder.Services.AddScoped<IMembershipService, MembershipService>();
builder.Services.AddScoped<ICommunityPostService, CommunityPostService>();
builder.Services.AddScoped<ICommunityCommentService, CommunityCommentService>();
builder.Services.AddScoped<ICommunityLikeService, CommunityLikeService>();
builder.Services.AddScoped<ICommunitySaveService, CommunitySaveService>();
builder.Services.AddScoped<ICommunityShareService, CommunityShareService>();
builder.Services.AddScoped<ICommunityReportService, CommunityReportService>();

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterInitialRequestValidator>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };

})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:client_id"];
    options.ClientSecret = builder.Configuration["Authentication:Google:client_secret"];
})
.AddGitHub(options =>
{
    options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];
});

// Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MenteeOnly", policy => policy.RequireRole("Mentee"));
    options.AddPolicy("MentorOnly", policy => policy.RequireRole("Mentor"));
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(builder.Configuration["CorsOrigins"]?.Split(',') ?? new[] { "http://localhost:3000" })
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Add Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure static files for serving uploaded files
builder.Services.AddDirectoryBrowser();


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "Mentorship Platform API";
        options.Theme = ScalarTheme.Moon;
    });
}

// Enable static files (for serving uploaded files)

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();

// Ensure database is created (for development)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    
    // Create uploads folder
    var webRoot = app.Environment.WebRootPath ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
    if (!Directory.Exists(webRoot))
    {
        Directory.CreateDirectory(webRoot);
    }
    
    var uploadsPath = Path.Combine(webRoot, "uploads");
    Directory.CreateDirectory(Path.Combine(uploadsPath, "cvs"));
    Directory.CreateDirectory(Path.Combine(uploadsPath, "profile-pictures"));
    Directory.CreateDirectory(Path.Combine(uploadsPath, "programs"));
    Directory.CreateDirectory(Path.Combine(uploadsPath, "tasks"));
}

app.Run();