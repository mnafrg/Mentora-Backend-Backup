

using DocumentFormat.OpenXml.Spreadsheet;
using Mentora.Application.DTOs.Applicants;
using Mentora.Application.DTOs.Application;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Programs;
using Mentora.Application.DTOs.Roadmap;
using Mentora.Application.Helpers;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Services
{
    public class ProgramMenteeService : IProgramMenteeService

    {
        private readonly IUnitOfWork _unitOfWork;



        public ProgramMenteeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<ProgramViewDto>> GetProgramViewAsync(int programId)
        {
            var programs = await _unitOfWork.Programs.GetAllAsync(
                p => p.ProgramId == programId && p.ProgramPostStatus == ProgramPostStatus.Published,
                p => p.MentorProfile.User,
                p => p.Domain,
                p => p.SubDomain,
                p => p.Likes,
                p => p.Comments,
                p => p.Shares,
                p => p.SavedByUsers,
                p => p.Applications);

            var program = programs.FirstOrDefault();

            if (program == null)
                return ApiResponse<ProgramViewDto>.ErrorResponse("Program not found");

            RoadmapDetailsDto? roadmapDto = null;
            if (program.RoadmapId.HasValue)
            {
                var roadmap = await _unitOfWork.Roadmaps.GetByIdWithFullHierarchyAsync(program.RoadmapId.Value);

                if (roadmap != null && roadmap.Status == RoadmapStatus.Published) 
                {
                  
                       roadmapDto = new RoadmapDetailsDto
                    {

                        RoadmapId = roadmap.RoadmapId,
                        Title = roadmap.Title,
                        Description = roadmap.Description,
                        Duration = roadmap.Duration,
                        TargetLevelFrom = roadmap.TargetLevelFrom,
                        TargetLevelTo = roadmap.TargetLevelTo,
                        SkillDomainId = roadmap.SkillDomainId,
                        SubDomainId = roadmap.SubDomainId,
                        TechnologyIds = roadmap.RoadmapTechnologies.Select(rt => rt.TechnologyId).ToList(),
                        Phases = roadmap.Phases.Select(p => new PhaseDto
                        {
                            PhaseId = p.RoadmapPhaseId,
                            Title = p.Title,
                            Summary = p.Summary,

                            Topics = p.Topics.Select(t => new TopicDto
                            {
                                Id = t.TopicId,
                                Title = t.Title,
                                Summary = t.Summary,

                                Materials = t.Materials.Select(m => new MaterialDto
                                {
                                    Id = m.MaterialId,
                                    Title = m.Title,
                                    Url = m.Url,
                                    MaterialType = m.MaterialType
                                }).ToList(),
                                Tasks = t.Tasks.Select(task => new TaskDto
                                {
                                    Id = task.TaskId,
                                    Title = task.Title,
                                    Description = task.Description,
                                    DeadLine = task.DeadLine,
                                    AttachmentUrl = task.TaskAttachmentUrl
                                }).ToList(),
                            }).ToList()
                        }).ToList()
                    };
                }
            } 
            var response = new ProgramViewDto
            {
                ProgramId = program.ProgramId,
                Title = program.Title,
                Description = program.Description,
                TargetLevel = program.TargetLevel.ToString(),
                Duration = program.Duration,
                DomainName =program.Domain.Name,
                SubDomainName = program.SubDomain.Name,
            
                MenteesCount = program.Applications.Count,
                LikesCount = program.Likes.Count,
                CommentsCount = program.Comments.Count,
                MentorProfileId =program.MentorProfileId,
                MentorName = program.MentorProfile.User.FirstName + " " + program.MentorProfile.User.LastName,
                ProfilePictureUrl = program.MentorProfile.ProfilePictureUrl,
                Roadmap = roadmapDto
            };

            return ApiResponse<ProgramViewDto>.SuccessResponse(response, "Program retrieved successfully");
        }
        public async Task<ApiResponse<IEnumerable<ProgramViewDto>>> GetSavedProgramsAsync(Guid userId)
        {
            var savedPosts = await _unitOfWork.SavedPosts.GetAllAsync(
                s => s.UserId == userId,
                s => s.Program.Domain,
                s => s.Program.SubDomain,
                s => s.Program.MentorProfile.User
            );

            var response = savedPosts
                .Where(s => s.Program != null &&
                            s.Program.ProgramPostStatus == ProgramPostStatus.Published)
                .Select(s => new ProgramViewDto
                {
                    ProgramId = s.Program.ProgramId,
                    Title = s.Program.Title,
                    Description = s.Program.Description,
                    TargetLevel = s.Program.TargetLevel.ToString(),
                    Duration = s.Program.Duration,
                    Deadline = s.Program.Deadline,
                    DomainName = s.Program.Domain?.Name,
                    SubDomainName = s.Program.SubDomain?.Name,
                    MentorName = s.Program.MentorProfile.User.FirstName + " " + s.Program.MentorProfile.User.LastName,
                    ProfilePictureUrl = s.Program.MentorProfile.ProfilePictureUrl,
                    ProgramImageUrl = s.Program.ProgramImageUrl
                });

            return ApiResponse<IEnumerable<ProgramViewDto>>.SuccessResponse(
                response, "Saved programs retrieved successfully");
        }
        public async Task<ApiResponse<MentorCardDto>> GetMentorCardByProgramIdAsync(int programId)
        {
            
            var program = await _unitOfWork.Programs.GetAsync(
                filter: p => p.ProgramId == programId,
                includes: new Expression<Func<Program, object>>[] {
                p => p.MentorProfile.User,
                P=>P.MentorProfile.Domain
                }
            );

            if (program == null || program.MentorProfile == null)
                return ApiResponse<MentorCardDto>.ErrorResponse("Mentor details not found");

            var mentor = program.MentorProfile;

            var result = new MentorCardDto
            {
                MentorProfileId = mentor.UserId,
                FullName = mentor.User.FirstName + " " + mentor.User.LastName,
                DomainName=mentor.Domain.Name,
                ProfilePicture = mentor.ProfilePictureUrl,
                Bio = mentor.Bio 
            };

            return ApiResponse<MentorCardDto>.SuccessResponse(result, "Success");
        }
        public async Task<ApiResponse<IEnumerable<ProgramQuestionDto>>> GetProgramQuestionsAsync(int programId)
        {
            var programs = await _unitOfWork.Programs.GetProgramAsync(
                p => p.ProgramId == programId && p.ProgramPostStatus == ProgramPostStatus.Published,
                $"{nameof(Program.Questions)}.{nameof(ProgramQuestion.Options)}"
               );

            var program = programs.FirstOrDefault();

            if (program == null)
                return ApiResponse<IEnumerable<ProgramQuestionDto>>.ErrorResponse("Program not found");

            var response = program.Questions.Select(q => new ProgramQuestionDto
            {
                ProgramQuestionId = q.ProgramQuestionId,
                QuestionText = q.QuestionText,
                AnswerType = q.AnswerType.ToString(),
                MaxSelections = q.AnswerType == AnswerType.MultipleChoice ? q.MaxSelections : null,

              
                Options = q.Options != null
                     ? q.Options.Select(o => o.OptionText).ToList()
                     : new List<string>()
            });
            return ApiResponse<IEnumerable<ProgramQuestionDto>>.SuccessResponse(response, "Questions retrieved successfully");
        }
        public async Task<ApiResponse<bool>> ToggleLikeProgramAsync(int programId, Guid userId)
        {
            var like = await _unitOfWork.PostLikes.GetAsync(
                l => l.ProgramId == programId && l.UserId == userId);

            if (like == null)
            {
                await _unitOfWork.PostLikes.CreateAsync(new PostLike
                {
                    ProgramId = programId,
                    UserId = userId
                });
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Program liked successfully");
            }

            await _unitOfWork.PostLikes.DeleteAsync(like);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(false, "Program unliked successfully");
        }
        public async Task<ApiResponse<bool>> ToggleSaveProgramAsync(int programId, Guid userId)
        {
            var save = await _unitOfWork.SavedPosts.GetAsync(
                s => s.ProgramId == programId && s.UserId == userId);

            if (save == null)
            {
                await _unitOfWork.SavedPosts.CreateAsync(new SavedPost
                {
                    ProgramId = programId,
                    UserId = userId
                });
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Program saved successfully");
            }

            await _unitOfWork.SavedPosts.DeleteAsync(save);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(false, "Program unsaved successfully");
        }

        public async Task<ApiResponse<string>> GenerateShareLinkAsync(int programId, Guid senderId)
        {
            var program = await _unitOfWork.Programs.GetByIdAsync(programId);

            if (program == null || program.ProgramPostStatus != ProgramPostStatus.Published)
                return ApiResponse<string>.ErrorResponse("Program not found");

            var share = new SharedPost
            {
                ProgramId = programId,
                UserId = senderId,
                SharedAt = DateTime.UtcNow
            };

            await _unitOfWork.SharedPosts.CreateAsync(share);
            await _unitOfWork.SaveChangesAsync();

            var encryptedId = ShareLinkHelper.EncryptProgramId(programId);
            var link = $"https://mentora.com/programs/{encryptedId}";

            return ApiResponse<string>.SuccessResponse(link, "Share link generated successfully");
        }
        public async Task<ApiResponse<ProgramViewDto>> GetProgramByShareLinkAsync(string encryptedLink)
        {
            try
            {
                var programId = ShareLinkHelper.DecryptProgramId(encryptedLink);
                return await GetProgramViewAsync(programId);
            }
            catch
            {
                return ApiResponse<ProgramViewDto>.ErrorResponse("Invalid share link");
            }
        }
        public async Task<ApiResponse<bool>> ApplyToProgramAsync(
      int programId, Guid menteeProfileId, CreateApplicationDto dto)
        {
            var programData = (await _unitOfWork.Programs.GetProgramAsync(
                p => p.ProgramId == programId && p.ProgramPostStatus == ProgramPostStatus.Published,
                $"{nameof(Program.Questions)}.{nameof(ProgramQuestion.Options)}"
            )).FirstOrDefault();

            if (programData == null)
                return ApiResponse<bool>.ErrorResponse("Program not found");

            var existingApplication = await _unitOfWork.MentorshipApplications.GetAllAsync(
                a => a.ProgramId == programId && a.MenteeProfileId == menteeProfileId);

            if (existingApplication.Any())
                return ApiResponse<bool>.ErrorResponse("Already applied to this program");

            var menteeProfile = await _unitOfWork.MenteeProfiles.GetByUserIdAsync(menteeProfileId);
            if (menteeProfile == null)
                return ApiResponse<bool>.ErrorResponse("Mentee profile not found");

            var programReqs = await _unitOfWork.MentorshipRequirements.GetRequirementsByProgramIdAsync(programId);
            var menteeInterests = await _unitOfWork.MenteeInterests.GetByMenteeProfileIdAsync(menteeProfileId);

            bool isEducationMatch = (int)menteeProfile.EducationStatus >= (int)programData.EducationLevel;

            bool meetTechRequirements = true;
            foreach (var req in programReqs)
            {
                var isSatisfied = menteeInterests.Any(interest =>
                    interest.TechnologyId == req.TechnologyId &&
                    (int)interest.ExperienceLevel >= (int)req.RequiredExperienceLevel);

                if (!isSatisfied) { meetTechRequirements = false; break; }
            }

            if (!isEducationMatch || !meetTechRequirements)
                return ApiResponse<bool>.ErrorResponse("This Program may not fit your Level");

          
            if (programData.Questions.Any())
            {
             
                if (dto.Answers == null || !dto.Answers.Any())
                    return ApiResponse<bool>.ErrorResponse("You must answer all program questions before submitting");

           
                var emptyAnswers = dto.Answers
                    .Where(a => string.IsNullOrWhiteSpace(a.QuestionAnswer))
                    .ToList();

                if (emptyAnswers.Any())
                    return ApiResponse<bool>.ErrorResponse("All answers must have content, no empty answers allowed");

              
                var answeredQuestionIds = dto.Answers.Select(a => a.QuestionId).ToHashSet();
                var unansweredQuestions = programData.Questions
                    .Where(q => !answeredQuestionIds.Contains(q.ProgramQuestionId))
                    .ToList();

                if (unansweredQuestions.Any())
                {
                    return ApiResponse<bool>.ErrorResponse("Missing required program questions.");
                }
            }
            

            var application = new MentorshipApplication
            {
                ProgramId = programId,
                MenteeProfileId = menteeProfileId,
                Status = ApplicationStatus.Pending,
                MeetRequirements = true,
                AppliedAt = DateTime.UtcNow,
                Answers = new List<ApplicationAnswer>()
            };

          
            if (dto.Answers != null)
            {
                var validQuestionIds = programData.Questions.Select(q => q.ProgramQuestionId).ToHashSet();

                foreach (var answer in dto.Answers)
                {
                    
                    if (!validQuestionIds.Contains(answer.QuestionId))
                        return ApiResponse<bool>.ErrorResponse($"Invalid question ID: {answer.QuestionId}");

                    var question = programData.Questions.First(q => q.ProgramQuestionId == answer.QuestionId);

                    // Multiple Choice  Options
                    if (question.AnswerType == AnswerType.MultipleChoice)
                    {
                        var selectedOptions = answer.QuestionAnswer
                            .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim())
                            .ToList();

                        if (question.MaxSelections.HasValue && selectedOptions.Count > question.MaxSelections.Value)
                            return ApiResponse<bool>.ErrorResponse($"You can select up to {question.MaxSelections} options only.");

                        var validOptions = question.Options.Select(o => o.OptionText).ToHashSet();
                        foreach (var selected in selectedOptions)
                        {
                            if (!validOptions.Contains(selected))
                                return ApiResponse<bool>.ErrorResponse($"'{selected}' is not a valid choice.");
                        }
                    }

                    application.Answers.Add(new ApplicationAnswer
                    {
                        ProgramQuestionId = answer.QuestionId,
                        AnswerText = answer.QuestionAnswer
                    });
                }
            }

            await _unitOfWork.MentorshipApplications.CreateAsync(application);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Application submitted successfully");
        }
    }
}
