using ClosedXML.Excel;    
using Mentora.Application.DTOs.Applicants;
using Mentora.Application.DTOs.Application;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Profile;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IO;           
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Services
{
    public class ApplicantsService : IApplicantsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public ApplicantsService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        //Get all applications for all programs of this mentor, then filter, paginate, and map to DTOs
        public async Task<ApiResponse<ProgramApplicantsResponseDto>> GetAllApplicantsAsync(GetAllApplicantsRequestDto request, Guid mentorProfileId)
        {
            var allCount = await _unitOfWork.MentorshipApplications.CountAsync(a =>
                a.Program.MentorProfileId == mentorProfileId);

            var pendingCount = await _unitOfWork.MentorshipApplications.CountAsync(a =>
                a.Program.MentorProfileId == mentorProfileId && a.Status == ApplicationStatus.Pending);

            var acceptedCount = await _unitOfWork.MentorshipApplications.CountAsync(a =>
                a.Program.MentorProfileId == mentorProfileId && a.Status == ApplicationStatus.Accepted);

            var rejectedCount = await _unitOfWork.MentorshipApplications.CountAsync(a =>
                a.Program.MentorProfileId == mentorProfileId && a.Status == ApplicationStatus.Rejected);


            var allItems = await _unitOfWork.MentorshipApplications.GetAllAsync(
                filter: a => a.Program.MentorProfileId == mentorProfileId &&
                            (!request.Status.HasValue || a.Status == request.Status.Value) &&
                            (!request.Level.HasValue || a.MenteeProfile.CurrentLevel == request.Level.Value) &&
                            (string.IsNullOrEmpty(request.Search) ||
                             a.MenteeProfile.User.FirstName.ToLower().Contains(request.Search.ToLower().Trim()) ||
                             (a.MenteeProfile.User.FirstName + " " + a.MenteeProfile.User.LastName)
                             .ToLower().Contains(request.Search.ToLower().Trim())),
                includes: new Expression<Func<MentorshipApplication, object>>[] {
            a => a.MenteeProfile.User,
            a => a.Program
                }
            );

            var filteredCount = allItems.Count();
            var totalPages = request.PageSize.HasValue
                ? (int)Math.Ceiling((double)filteredCount / request.PageSize.Value)
                : 1;

            List<ApplicantListItemDto> pagedItems;

            if (request.PageNumber.HasValue && request.PageSize.HasValue)
            {
                pagedItems = allItems
                    .OrderByDescending(a => a.AppliedAt)
                    .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value)
                    .Select(a => new ApplicantListItemDto
                    {
                        ApplicationId = a.ApplicationId,
                        MenteeName = a.MenteeProfile.User.FirstName + " " + a.MenteeProfile.User.LastName,
                        MenteeProfilePicture = a.MenteeProfile.ProfilePictureUrl,
                        AppliedAt = a.AppliedAt,
                        Level = a.MenteeProfile.CurrentLevel.ToString(),
                        ProgramName = a.Program.Title,
                        Status = a.Status.ToString(),
                    }).ToList();
            }
            else
            {
                pagedItems = allItems
                    .OrderByDescending(a => a.AppliedAt)
                    .Select(a => new ApplicantListItemDto
                    {
                        ApplicationId = a.ApplicationId,
                        MenteeName = a.MenteeProfile.User.FirstName + " " + a.MenteeProfile.User.LastName,
                        MenteeProfilePicture = a.MenteeProfile.ProfilePictureUrl,
                        AppliedAt = a.AppliedAt,
                        Level = a.MenteeProfile.CurrentLevel.ToString(),
                        ProgramName = a.Program.Title,
                        Status = a.Status.ToString(),
                    }).ToList();
            }

            var responseData = new ProgramApplicantsResponseDto
            {
                Items = pagedItems,
                TotalCount = filteredCount,
                TotalPages = totalPages,
                CurrentPage = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? filteredCount,
                AllApplicantsCount = allCount,
                PendingCount = pendingCount,
                AcceptedCount = acceptedCount,
                RejectedCount = rejectedCount
            };
            var message = !pagedItems.Any() ? "No applicants found" : "Success";
            return ApiResponse<ProgramApplicantsResponseDto>.SuccessResponse(responseData, message);
          //  return ApiResponse<ProgramApplicantsResponseDto>.SuccessResponse(responseData, "Success");
        }


        //Get applicants for a specific program, with filtering, pagination, and mapping to DTOs
        public async Task<ApiResponse<ProgramApplicantsResponseDto>> GetApplicantsByProgramAsync(GetApplicantsRequestDto request, Guid mentorProfileId)
        { 
            var program = await _unitOfWork.Programs.GetByIdAsync(request.ProgramId);
            if (program == null || program.MentorProfileId != mentorProfileId)
            {
                return ApiResponse<ProgramApplicantsResponseDto>.ErrorResponse("Program not found ");
            }
            var allCount = await _unitOfWork.MentorshipApplications.CountAsync(a =>
                a.ProgramId == request.ProgramId && a.Program.MentorProfileId == mentorProfileId);

            var pendingCount = await _unitOfWork.MentorshipApplications.CountAsync(a =>
                a.ProgramId == request.ProgramId &&
                a.Program.MentorProfileId == mentorProfileId &&
                a.Status == ApplicationStatus.Pending);

            var acceptedCount = await _unitOfWork.MentorshipApplications.CountAsync(a =>
                a.ProgramId == request.ProgramId &&
                a.Program.MentorProfileId == mentorProfileId &&
                a.Status == ApplicationStatus.Accepted);

            var rejectedCount = await _unitOfWork.MentorshipApplications.CountAsync(a =>
                a.ProgramId == request.ProgramId &&
                a.Program.MentorProfileId == mentorProfileId &&
                a.Status == ApplicationStatus.Rejected);

            var allItems = await _unitOfWork.MentorshipApplications.GetAllAsync(
                filter: a => a.ProgramId == request.ProgramId &&
                            (!request.Status.HasValue || a.Status == request.Status.Value) &&
                            (!request.Level.HasValue || a.MenteeProfile.CurrentLevel == request.Level.Value) &&
                            (string.IsNullOrEmpty(request.Search) ||
                              (a.MenteeProfile.User.FirstName + " " + a.MenteeProfile.User.LastName)
                             .ToLower().Contains(request.Search.ToLower().Trim())), includes: new Expression<Func<MentorshipApplication, object>>[] {
            a => a.MenteeProfile.User
                }
            );

            var filteredCount = allItems.Count();
            var totalPages = request.PageSize.HasValue
                ? (int)Math.Ceiling((double)filteredCount / request.PageSize.Value)
                : 1;

            List<ApplicantListItemDto> pagedItems;

            if (request.PageNumber.HasValue && request.PageSize.HasValue)
            {
                pagedItems = allItems
                    .OrderByDescending(a => a.AppliedAt)
                    .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value)
                    .Select(a => new ApplicantListItemDto
                    {
                        ApplicationId = a.ApplicationId,
                        MenteeName = a.MenteeProfile.User.FirstName + " " + a.MenteeProfile.User.LastName,
                        MenteeProfilePicture = a.MenteeProfile.ProfilePictureUrl,
                        AppliedAt = a.AppliedAt,
                        Level = a.MenteeProfile.CurrentLevel.ToString(),
                        ProgramName = a.Program.Title,
                        Status = a.Status.ToString(),
                    }).ToList();
            }
            else
            {
                pagedItems = allItems
                    .OrderByDescending(a => a.AppliedAt)
                    .Select(a => new ApplicantListItemDto
                    {
                        ApplicationId = a.ApplicationId,
                        MenteeName = a.MenteeProfile.User.FirstName + " " + a.MenteeProfile.User.LastName,
                        MenteeProfilePicture = a.MenteeProfile.ProfilePictureUrl,
                        AppliedAt = a.AppliedAt,
                        Level = a.MenteeProfile.CurrentLevel.ToString(),
                        ProgramName = a.Program.Title,
                        Status = a.Status.ToString(),
                    }).ToList();
            }

            var responseData = new ProgramApplicantsResponseDto
            {
                Items = pagedItems,
                TotalCount = filteredCount,
                TotalPages = totalPages,
                CurrentPage = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? filteredCount,
                AllApplicantsCount = allCount,
                PendingCount = pendingCount,
                AcceptedCount = acceptedCount,
                RejectedCount = rejectedCount
            };
            return ApiResponse<ProgramApplicantsResponseDto>.SuccessResponse(responseData, "Success");
        }



        public async Task<ApiResponse<string>> UpdateApplicationStatusAsync(
      int applicationId, ApplicationStatus newStatus, Guid mentorProfileId)
        {
            var application = await _unitOfWork.MentorshipApplications.GetByIdAsync(applicationId);

            if (application == null || application.Program.MentorProfileId != mentorProfileId)
                return ApiResponse<string>.ErrorResponse("Application not found or unauthorized");

            if (application.Status == newStatus)
            {
                return ApiResponse<string>.ErrorResponse(
                    $"Application is already {newStatus}");
            }

            application.Status = newStatus;
            await _unitOfWork.MentorshipApplications.UpdateAsync(application);
            await _unitOfWork.SaveChangesAsync();

            var message = newStatus switch
            {
                ApplicationStatus.Accepted => "Mentee accepted successfully",
                ApplicationStatus.Rejected => "Application rejected successfully",
                ApplicationStatus.Pending => "Application moved back to pending successfully",
                
            };

            return ApiResponse<string>.SuccessResponse(message);
        }

        public async Task<ApiResponse<string>> NotifyAllApplicantsAsync(int programId, Guid mentorProfileId)
        {
       
            var program = await _unitOfWork.Programs.GetByIdAsync(programId);
            if (program == null || program.MentorProfileId != mentorProfileId)
                return ApiResponse<string>.ErrorResponse("Program not found or unauthorized");

            var applicants = await _unitOfWork.MentorshipApplications.GetAllAsync(
                filter: a => a.ProgramId == programId && a.Status != ApplicationStatus.Pending,
                includes: a => a.MenteeProfile.User
            );

            if (!applicants.Any())
                return ApiResponse<string>.ErrorResponse("No processed applications to notify");

            foreach (var app in applicants)
            {
                if (app?.MenteeProfile?.User == null || string.IsNullOrEmpty(app.MenteeProfile.User.Email))
                    continue;

                string statusText = app.Status == ApplicationStatus.Accepted ? "Accepted" : "Rejected";

                try
                {
                   
                    await _emailService.SendApplicationNotificationAsync(
                        app.MenteeProfile.User.Email,
                        app.MenteeProfile.User.FirstName,
                        program.Title,
                        statusText
                    );
                 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email to {app.MenteeProfile.User.Email}: {ex.Message}");
                }
            }

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse($"Notifications sent to {applicants.Count()} applicants.");
        }
        

        public async Task<byte[]> ExportApplicantsToExcelAsync(GetAllApplicantsRequestDto request, Guid mentorProfileId)
    {
      
        var allItems = await _unitOfWork.MentorshipApplications.GetAllAsync(
            filter: a => a.Program.MentorProfileId == mentorProfileId &&
                        (!request.Status.HasValue || a.Status == request.Status.Value) &&
                        (!request.Level.HasValue || a.MenteeProfile.CurrentLevel == request.Level.Value) &&
                        (string.IsNullOrEmpty(request.Search) ||
                         (a.MenteeProfile.User.FirstName + " " + a.MenteeProfile.User.LastName).ToLower().Contains(request.Search.ToLower())),
            includes: new Expression<Func<MentorshipApplication, object>>[] { a => a.MenteeProfile.User, a => a.Program }
        );

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Applicants");

            worksheet.Cell(1, 1).Value = "Applicant Name";
            worksheet.Cell(1, 2).Value = "Program";
            worksheet.Cell(1, 3).Value = "Level";
            worksheet.Cell(1, 4).Value = "Status";
            worksheet.Cell(1, 5).Value = "Applied At";

            int row = 2;
            foreach (var item in allItems)
            {
                worksheet.Cell(row, 1).Value = item.MenteeProfile.User.FirstName + " " + item.MenteeProfile.User.LastName;
                worksheet.Cell(row, 2).Value = item.Program.Title;
                worksheet.Cell(row, 3).Value = item.MenteeProfile.CurrentLevel.ToString();
                worksheet.Cell(row, 4).Value = item.Status.ToString();
                worksheet.Cell(row, 5).Value = item.AppliedAt.ToString("yyyy-MM-dd");
                row++;
            }

            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return stream.ToArray();
            }
        }
    }
        //MenteeApplications
        public async Task<ApiResponse<List<MenteeApplicationDto>>> GetMenteeApplicationsAsync(Guid menteeProfileId)
        {
            var applications = await _unitOfWork.MentorshipApplications.GetAllAsync(
         filter: a => a.MenteeProfileId == menteeProfileId, 
          includes: new Expression<Func<MentorshipApplication, object>>[]
                 {
                     a => a.Program.MentorProfile.User,
                    a => a.Program.Domain,
                 }
         
                );
            var result = applications.Select(a => new MenteeApplicationDto
            {
                ApplicationId = a.ApplicationId,
                ProgramTitle = a.Program.Title,
                ProgramDescription = a.Program.Description,  
                ProgramImageUrl = a.Program.ProgramImageUrl, 
                MentorName = $"{a.Program.MentorProfile.User.FirstName} {a.Program.MentorProfile.User.LastName}",
                MentorDomain = a.Program.Domain?.Name,
                MentorProfilePicture = a.Program.MentorProfile?.ProfilePictureUrl,
                AppliedAt = a.AppliedAt,
                Status = a.Status.ToString()
            }).ToList();
            return ApiResponse<List<MenteeApplicationDto>>.SuccessResponse(result, result.Any() ? "Success" : "No applications found.");
        }

        public async Task<ApiResponse<ApplicantProfileDetailsDto>> GetApplicantProfileAsync(int applicationId, Guid mentorId)
        {

            var application = await _unitOfWork.MentorshipApplications.GetByIdAsync(applicationId);
              

            
            if (application == null || application.Program.MentorProfileId != mentorId)
            {
                return ApiResponse<ApplicantProfileDetailsDto>.ErrorResponse("Not found or unauthorized");
            }

            var mentee = application.MenteeProfile;
            var user = mentee.User;

            var allUserLinks = await _unitOfWork.ProfileLinks.GetByUserIdAsync(user.UserId);


            var detailsDto = new ApplicantProfileDetailsDto
            {
                ApplicationId = applicationId,
                MenteeProfileId = mentee.UserId,
                MenteeName = $"{user.FirstName} {user.LastName}",
                Level = mentee.CurrentLevel.ToString(),
                MenteeProfilePicture = mentee.ProfilePictureUrl,
                Education = mentee.EducationStatus.ToString(),
                Status = application.Status.ToString(),
                Bio = mentee.Bio,
                LinkedInUrl = allUserLinks.FirstOrDefault(l => l.Label.ToLower() == "linkedin")?.URL,
                PortfolioUrl = allUserLinks.FirstOrDefault(l => l.Label.ToLower() == "portfolio")?.URL,

                QuestionsAndAnswers = application.Answers.Select(ans => new ApplicationQuestionDto
                { 
                    QuestionId=ans.ProgramQuestionId,
                    QuestionText = ans.ProgramQuestion.QuestionText,
                    QuestionAnswer = ans.AnswerText
                }).ToList()
            };

            return ApiResponse<ApplicantProfileDetailsDto>.SuccessResponse(detailsDto, "Data retrieved successfully");
        }
    }
}

