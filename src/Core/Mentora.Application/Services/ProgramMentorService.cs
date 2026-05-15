using DocumentFormat.OpenXml.Presentation;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Programs;
using Mentora.Application.Interfaces;
using Mentora.Application.Interfaces.Services.Classroom;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Services
{
    public class ProgramMentorService : IProgramMentorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassroomService _classroomService;

        public ProgramMentorService(IUnitOfWork unitOfWork, IClassroomService classroomService)
        {
            _unitOfWork = unitOfWork;
            _classroomService = classroomService;
        }

        public async Task<ApiResponse<CreateProgramResponseDto>> CreateProgramAsync(CreateProgramٌRequestDto dto, Guid mentorProfileId)
        {
            var title = dto.Title.Trim().ToLower();

            var existingProgram = await _unitOfWork.Programs.GetAllAsync(p =>
                                                     p.Title.Trim().ToLower() == title && p.MentorProfileId == mentorProfileId);


            if (existingProgram.Any())
                return ApiResponse<CreateProgramResponseDto>.ErrorResponse("Program with this title already exists");

            if (dto.Status == ProgramPostStatus.Published)
            {
                if (string.IsNullOrWhiteSpace(dto.Description) ||
                    string.IsNullOrWhiteSpace(dto.Availability) ||
                    string.IsNullOrWhiteSpace(dto.Duration) ||
                    dto.DomainId == 0 || dto.SubDomainId == 0
                    || dto.Technologies == null || !dto.Technologies.Any()
                    || dto.Capacity <= 0)
                {
                    return ApiResponse<CreateProgramResponseDto>.ErrorResponse("Cannot publish program. All details (Description, Availability, Duration, Domain) are required for publishing.");
                }
            }


            var program = new Program
            {
                Title = dto.Title,
                Description = dto.Description,
                Deadline = dto.Deadline,
                ProgramImageUrl = dto.ProgramImageUrl,
                Availability = dto.Availability,
                Duration = dto.Duration,
                EducationLevel = dto.EducationLevel,
                TargetLevel = dto.TargetLevel,
                Capacity = dto.Capacity,
                ProgramPostStatus = dto.Status,
                DomainId = dto.DomainId,
                SubDomainId = dto.SubDomainId,
                RoadmapId = dto.RoadmapId,
                MentorProfileId = mentorProfileId
            };

            if (dto.Questions != null)
            {
                foreach (var q in dto.Questions)
                {
                    if (string.IsNullOrWhiteSpace(q.QuestionText) && string.IsNullOrWhiteSpace(q.AnswerType))
                        continue;

                    if (!Enum.TryParse<AnswerType>(q.AnswerType, out var answerTypeEnum))
                    {
                        return ApiResponse<CreateProgramResponseDto>.ErrorResponse($"Invalid Answer Type: {q.AnswerType}");
                    }


                    if (answerTypeEnum == AnswerType.MultipleChoice && (q.Options == null || !q.Options.Any()))
                    {
                        return ApiResponse<CreateProgramResponseDto>.ErrorResponse("MCQ questions must have options");
                    }


                    var question = new ProgramQuestion
                    {
                        QuestionText = q.QuestionText,
                        AnswerType = answerTypeEnum,
                        MaxSelections = q.MaxSelections
                    };


                    if (answerTypeEnum == AnswerType.MultipleChoice && q.Options != null)
                    {
                        foreach (var optionText in q.Options)
                        {
                            question.Options.Add(new QuestionOption { OptionText = optionText });
                        }
                    }

                    program.Questions.Add(question);
                }
            }
            if (dto.Technologies != null && dto.Technologies.Any())
            {
                foreach (var tech in dto.Technologies)
                {
                    program.MentorshipRequirements.Add(new MentorshipRequirement
                    {
                        TechnologyId = tech.TechnologyId,
                        RequiredExperienceLevel = tech.RequiredExperienceLevel
                    });
                }
            }

            await _unitOfWork.Programs.CreateAsync(program);
            await _unitOfWork.SaveChangesAsync();

            // Create classroom for the program automatically
            await _classroomService.CreateClassroomForProgramAsync(
                program.ProgramId,
                program.Title,
                program.Description
            );

            return ApiResponse<CreateProgramResponseDto>.SuccessResponse(
       new CreateProgramResponseDto { ProgramId = program.ProgramId },
       "Program created successfully");
        }
        public async Task<ApiResponse<IEnumerable<ProgramResponseDto>>> GetAllDraftsAsync(Guid mentorProfileId)
        {

            var programs = await _unitOfWork.Programs.GetProgramAsync(
                p => p.MentorProfileId == mentorProfileId && p.ProgramPostStatus == ProgramPostStatus.Draft,
                nameof(Program.Domain),
                nameof(Program.SubDomain),
                $"{nameof(Program.Questions)}.{nameof(ProgramQuestion.Options)}",
                $"{nameof(Program.MentorshipRequirements)}.{nameof(MentorshipRequirement.Technology)}"

            );

            var response = programs.Select(p => new ProgramResponseDto
            {
                ProgramId = p.ProgramId,
                Title = p.Title,
                Deadline = p.Deadline,
                ProgramImageUrl = p.ProgramImageUrl,
                Description = p.Description,
                Availability = p.Availability,
                Duration = p.Duration,
                EducationLevel = p.EducationLevel.ToString(),
                TargetLevel = p.TargetLevel.ToString(),
                Capacity = p.Capacity,

                DomainName = p.Domain?.Name,
                SubDomainName = p.SubDomain?.Name,
                CreatedAt = DateTime.UtcNow,
                Technologies = p.MentorshipRequirements
                    .Where(mr => mr.Technology != null)
                    .Select(mr => new TechnologyRequirementDto
                    {
                        TechnologyId = mr.TechnologyId,
                        RequiredExperienceLevel = mr.RequiredExperienceLevel
                    }).ToList(),
                Questions = p.Questions.Select(q => new ProgramQuestionDto
                {
                    QuestionText = q.QuestionText,
                    AnswerType = q.AnswerType.ToString(),
                    MaxSelections = q.AnswerType == AnswerType.MultipleChoice ? q.MaxSelections : null,
                    Options = q.AnswerType == AnswerType.MultipleChoice
                       ? q.Options.Select(o => o.OptionText).ToList()
                       : null
                }).ToList(),

                RoadmapId = p.RoadmapId
            });

            return ApiResponse<IEnumerable<ProgramResponseDto>>.SuccessResponse(response, "Drafts retrieved successfully");
        }
        public async Task<ApiResponse<ProgramResponseDto>> GetProgramByIdAsync(int programId, Guid mentorProfileId)
        {
            var program = await _unitOfWork.Programs.GetDraftProgramWithDetailsAsync(
         programId,
         nameof(Program.Domain),
                nameof(Program.SubDomain),
                $"{nameof(Program.Questions)}.{nameof(ProgramQuestion.Options)}",
                $"{nameof(Program.MentorshipRequirements)}.{nameof(MentorshipRequirement.Technology)}"

     );

            if (program == null || program.MentorProfileId != mentorProfileId)
                return ApiResponse<ProgramResponseDto>.ErrorResponse("Program not found");

            if (program.ProgramPostStatus != ProgramPostStatus.Draft)
                return ApiResponse<ProgramResponseDto>.ErrorResponse("Only Drafts can be accessed.");

            var response = new ProgramResponseDto
            {
                ProgramId = program.ProgramId,
                Title = program.Title,
                Deadline = program.Deadline,
                ProgramImageUrl = program.ProgramImageUrl,
                Description = program.Description,
                Availability = program.Availability,
                Duration = program.Duration,
                EducationLevel = program.EducationLevel.ToString(),
                TargetLevel = program.TargetLevel.ToString(),
                Capacity = program.Capacity,

                DomainName = program.Domain.Name,
                SubDomainName = program.SubDomain.Name,

                RoadmapId = program.RoadmapId,
                Technologies = program.MentorshipRequirements
                    .Where(mr => mr.Technology != null)
                    .Select(mr => new TechnologyRequirementDto
                    {
                        TechnologyId = mr.TechnologyId,
                        RequiredExperienceLevel = mr.RequiredExperienceLevel
                    }).ToList(),
                    Questions = program.Questions.Select(q => new ProgramQuestionDto
                {
                    QuestionText = q.QuestionText,
                    AnswerType = q.AnswerType.ToString(),
                    MaxSelections = q.AnswerType == AnswerType.MultipleChoice ? q.MaxSelections : null,
                    Options = q.AnswerType == AnswerType.MultipleChoice
                        ? q.Options.Select(o => o.OptionText).ToList()
                        : null
                }).ToList()
            };

            return ApiResponse<ProgramResponseDto>.SuccessResponse(response, "Program retrieved successfully");
        }
        public async Task<ApiResponse<IEnumerable<ProgramResponseDto>>> GetAllPublishedAsync(Guid mentorProfileId)
        {
            var programs = await _unitOfWork.Programs.GetProgramAsync(
                p => p.MentorProfileId == mentorProfileId &&
                     p.ProgramPostStatus == ProgramPostStatus.Published,
                nameof(Program.Domain),
                nameof(Program.SubDomain),
                $"{nameof(Program.Questions)}.{nameof(ProgramQuestion.Options)}",
                $"{nameof(Program.MentorshipRequirements)}.{nameof(MentorshipRequirement.Technology)}"
            );

            var response = programs.Select(p => new ProgramResponseDto
            {
                ProgramId = p.ProgramId,
                Title = p.Title,
                Description = p.Description,
                ProgramImageUrl = p.ProgramImageUrl,
                Availability = p.Availability,
                Duration = p.Duration,
                Deadline = p.Deadline,
                EducationLevel = p.EducationLevel.ToString(),
                TargetLevel = p.TargetLevel.ToString(),
                Capacity = p.Capacity,
                DomainName = p.Domain?.Name,
                SubDomainName = p.SubDomain?.Name,
                RoadmapId = p.RoadmapId,
                Technologies = p.MentorshipRequirements
                    .Where(mr => mr.Technology != null)
                    .Select(mr => new TechnologyRequirementDto
                    {
                        TechnologyId = mr.TechnologyId,
                        RequiredExperienceLevel = mr.RequiredExperienceLevel
                    }).ToList(),
                Questions = p.Questions.Select(q => new ProgramQuestionDto
                {
                    QuestionText = q.QuestionText,
                    AnswerType = q.AnswerType.ToString(),
                    MaxSelections = q.AnswerType == AnswerType.MultipleChoice ? q.MaxSelections : null,
                    Options = q.AnswerType == AnswerType.MultipleChoice
                        ? q.Options.Select(o => o.OptionText).ToList()
                        : null
                }).ToList()
            });

            return ApiResponse<IEnumerable<ProgramResponseDto>>.SuccessResponse(
                response, "Published programs retrieved successfully");
        }
        public async Task<ApiResponse<ProgramResponseDto>> UpdateProgramAsync(int programId, UpdateProgramDto dto, Guid mentorProfileId)
        {
            var programs = await _unitOfWork.Programs.GetProgramAsync(
     p => p.ProgramId == programId,
     nameof(Program.Domain),
     nameof(Program.SubDomain),
     $"{nameof(Program.Questions)}.{nameof(ProgramQuestion.Options)}",
     $"{nameof(Program.MentorshipRequirements)}.{nameof(MentorshipRequirement.Technology)}"
 );

            var program = programs.FirstOrDefault();
            if (program == null)
                return ApiResponse<ProgramResponseDto>.ErrorResponse("Program not found");
            var oldStatus = program.ProgramPostStatus;

            if (dto.Title != null) program.Title = dto.Title.Trim();
            if (dto.ProgramImageUrl != null) program.ProgramImageUrl = dto.ProgramImageUrl;
            if (dto.Deadline.HasValue) program.Deadline = dto.Deadline.Value;
            if (dto.Description != null) program.Description = dto.Description;
            if (dto.Availability != null) program.Availability = dto.Availability;
            if (dto.Duration != null) program.Duration = dto.Duration;
            if (dto.EducationLevel != null) program.EducationLevel = dto.EducationLevel.Value;
            if (dto.TargetLevel != null) program.TargetLevel = dto.TargetLevel.Value;
            if (dto.Capacity != null) program.Capacity = dto.Capacity.Value;
            if (dto.DomainId != null) program.DomainId = dto.DomainId.Value;
            if (dto.SubDomainId != null) program.SubDomainId = dto.SubDomainId.Value;
            if (dto.RoadmapId != null) program.RoadmapId = dto.RoadmapId;

            if (dto.Status.HasValue)
            {
                if (dto.Status.Value == ProgramPostStatus.Published)
                {
                    if (string.IsNullOrWhiteSpace(program.Description) ||
                        string.IsNullOrWhiteSpace(program.Duration) ||
                        program.DomainId == 0 ||
                        program.SubDomainId == 0 ||
                        !program.MentorshipRequirements.Any())
                    {
                        return ApiResponse<ProgramResponseDto>.ErrorResponse(
                            "Cannot publish! Please complete all required fields.");
                    }
                }
                program.ProgramPostStatus = dto.Status.Value;
            }

         

            await _unitOfWork.Programs.UpdateAsync(program);
            await _unitOfWork.SaveChangesAsync();
          
            var response = new ProgramResponseDto
            {
                ProgramId = program.ProgramId,
                Title = program.Title,
                Deadline = program.Deadline,
                ProgramImageUrl = program.ProgramImageUrl,
                Description = program.Description,
                Availability = program.Availability,
                Duration = program.Duration,
                EducationLevel = program.EducationLevel.ToString(),
                TargetLevel = program.TargetLevel.ToString(),
                Capacity = program.Capacity,
                DomainName = program.Domain?.Name,
                SubDomainName = program.SubDomain?.Name,
                RoadmapId = program.RoadmapId,
                Technologies = program.MentorshipRequirements
                        .Where(mr => mr.Technology != null)
                        .Select(mr => new TechnologyRequirementDto
                        {
                            TechnologyId = mr.TechnologyId,
                            RequiredExperienceLevel = mr.RequiredExperienceLevel
                        }).ToList(),
                Questions = program.Questions.Select(q => new ProgramQuestionDto
                {
                    QuestionText = q.QuestionText,
                    AnswerType = q.AnswerType.ToString(),
                    MaxSelections = q.AnswerType == AnswerType.MultipleChoice ? q.MaxSelections : null,
                    Options = q.AnswerType == AnswerType.MultipleChoice
                        ? q.Options.Select(o => o.OptionText).ToList()
                        : null
                }).ToList()
            };

            var message = dto.Status.HasValue && dto.Status.Value == ProgramPostStatus.Published
        && oldStatus != ProgramPostStatus.Published
        ? "Program published successfully"
        : dto.Status.HasValue && dto.Status.Value == ProgramPostStatus.Draft
        && oldStatus == ProgramPostStatus.Published
        ? "Program unpublished successfully" 
        : "Program updated successfully";
            return ApiResponse<ProgramResponseDto>.SuccessResponse(response, message);






        }
    }
}
