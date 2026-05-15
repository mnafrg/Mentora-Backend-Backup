using Microsoft.EntityFrameworkCore;

using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Roadmap;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Services
{
    public class RoadmapService : IRoadmapService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoadmapService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<int>> CreateBasicInfoAsync(CreateRoadmapBasicInfoDto dto, Guid mentorId)
        {

            // FIX: title null/empty guard
            if (string.IsNullOrWhiteSpace(dto.Title))
                return ApiResponse<int>.ErrorResponse("Roadmap title is required.");

            // FIX: Duration zero/negative guard
            if (dto.Duration <= 0)
                return ApiResponse<int>.ErrorResponse("Duration must be greater than zero.");

            // FIX: TargetLevelFrom/To validation before use
            if (!Enum.IsDefined(typeof(ExperienceLevel), dto.TargetLevelFrom))
                return ApiResponse<int>.ErrorResponse("Invalid TargetLevelFrom value.");
            if (!Enum.IsDefined(typeof(ExperienceLevel), dto.TargetLevelTo))
                return ApiResponse<int>.ErrorResponse("Invalid TargetLevelTo value.");
            if (dto.TargetLevelFrom > dto.TargetLevelTo)
                return ApiResponse<int>.ErrorResponse("TargetLevelFrom cannot be greater than TargetLevelTo.");

            var mentorRoadmaps = await _unitOfWork.Roadmaps.GetByMentorIdAsync(mentorId);

            var existingDraft = mentorRoadmaps.FirstOrDefault(r =>
                r.Title.Trim().ToLower() == dto.Title.Trim().ToLower() &&
                r.Status == RoadmapStatus.Draft);

            if (existingDraft != null)
            {
                return ApiResponse<int>.ErrorResponse("This title already exists in your drafts. Please use a different title or continue editing the existing one.");
            }
            var roadmap = new Roadmap
            {
                Title = dto.Title,
                Description = dto.Description,
                SkillDomainId = dto.SkillDomainId,
                SubDomainId = dto.SubDomainId,
                Duration = dto.Duration,
                TargetLevelFrom = dto.TargetLevelFrom,
                TargetLevelTo = dto.TargetLevelTo,
                MentorProfileId = mentorId,
                Status = RoadmapStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            // FIX: null guard on TechnologyIds
            if (dto.TechnologyIds != null)
            {
                foreach (var techId in dto.TechnologyIds)
                {
                    roadmap.RoadmapTechnologies.Add(new RoadmapTechnology { TechnologyId = techId });
                }
            }
            await _unitOfWork.Roadmaps.AddAsync(roadmap);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(roadmap.RoadmapId, "Roadmap basic info saved successfully.");
        }
        public async Task<ApiResponse<List<RoadmapBasicInfoDto>>> GetAllRoadmapsBasicInfoAsync()
        {
            var roadmaps = await _unitOfWork.Roadmaps.GetAllBasicInfoAsync();

            if (roadmaps == null || !roadmaps.Any())
                return ApiResponse<List<RoadmapBasicInfoDto>>.SuccessResponse(new List<RoadmapBasicInfoDto>(), "No roadmaps available at the moment.");

            
            var result = roadmaps.Select(roadmap => new RoadmapBasicInfoDto
            {
                
                Title = roadmap.Title,
                Description = roadmap.Description,
                SkillDomainId = roadmap.SkillDomainId,
                SubDomainId = roadmap.SubDomainId,
                Duration = roadmap.Duration,
                TargetLevelFrom = roadmap.TargetLevelFrom,
                TargetLevelTo = roadmap.TargetLevelTo,

                
                TechnologyIds = roadmap.RoadmapTechnologies?.Select(rt => rt.TechnologyId).ToList() ?? new List<int>(),

                PhasesCount = roadmap.Phases?.Count() ?? 0
            }).ToList();

            return ApiResponse<List<RoadmapBasicInfoDto>>.SuccessResponse(result);
        }
        public async Task<ApiResponse<bool>> ToggleSaveRoadmapAsync(int roadmapId, Guid userId)
        {
            var roadmap = await _unitOfWork.Roadmaps.GetByIdAsync(roadmapId);
            if (roadmap == null || roadmap.Status != RoadmapStatus.Published)
                return ApiResponse<bool>.ErrorResponse("Roadmap not found");

            var save = await _unitOfWork.SavedRoadmaps.GetAsync(
                s => s.RoadmapId == roadmapId && s.UserId == userId);

            if (save == null)
            {
                await _unitOfWork.SavedRoadmaps.CreateAsync(new SavedRoadmap
                {
                    RoadmapId = roadmapId,
                    UserId = userId,
                    SavedAt = DateTime.UtcNow
                });
                await _unitOfWork.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Roadmap saved successfully");
            }

            await _unitOfWork.SavedRoadmaps.DeleteAsync(save);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(false, "Roadmap unsaved successfully");
        }


        public async Task<ApiResponse<IEnumerable<RoadmapDetailsDto>>> GetSavedRoadmapsAsync(Guid userId)
        {
            var savedRoadmaps = await _unitOfWork.SavedRoadmaps.GetAllAsync(
                s => s.UserId == userId,
                s => s.Roadmap
            );

            var result = savedRoadmaps
                .Where(s => s.Roadmap != null && s.Roadmap.Status == RoadmapStatus.Published)
                .Select(s => new RoadmapDetailsDto
                {
                    RoadmapId = s.Roadmap.RoadmapId,
                    Title = s.Roadmap.Title,
                    Description = s.Roadmap.Description,
                    Duration = s.Roadmap.Duration,
                    TargetLevelFrom = s.Roadmap.TargetLevelFrom,
                    TargetLevelTo = s.Roadmap.TargetLevelTo,
                    SkillDomainId = s.Roadmap.SkillDomainId,
                    SubDomainId = s.Roadmap.SubDomainId,

                });

            return ApiResponse<IEnumerable<RoadmapDetailsDto>>.SuccessResponse(
                result, "Saved roadmaps retrieved successfully");
        }
        public async Task<ApiResponse<int>> CreatePhaseAsync(CreateRoadmapPhaseDto dto, Guid mentorId)
        {
            var roadmap = await _unitOfWork.Roadmaps.GetByIdAsync(dto.RoadmapId);
            if (roadmap is null)
                return ApiResponse<int>.ErrorResponse("Roadmap not found");

            if (roadmap.MentorProfileId != mentorId)
                return ApiResponse<int>.ErrorResponse("Unauthorized");

            // FIX: title null/empty guard
            if (string.IsNullOrWhiteSpace(dto.Title))
                return ApiResponse<int>.ErrorResponse("Phase title is required.");

            var existingPhases = await _unitOfWork.RoadmapPhases.GetByRoadmapIdAsync(dto.RoadmapId);

            if (existingPhases.Any(p => p.Title.Trim().ToLower() == dto.Title.Trim().ToLower()))
            {
                return ApiResponse<int>.ErrorResponse($"The title '{dto.Title}' is already Existed.");
            }
        
          
            var phase = new RoadmapPhase
            {
                RoadmapId = dto.RoadmapId,
                Title = dto.Title,
                Summary = dto.Summary,
               
            };

            _unitOfWork.RoadmapPhases.Add(phase);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(phase.RoadmapPhaseId, $"Phase '{phase.Title}' created successfully! Now you can start adding topics to this phase.");
        }

        public async Task<ApiResponse<bool>> UpdatePhaseAsync(PhaseDto dto)
        {
            var phase = await _unitOfWork.RoadmapPhases.GetByIdWithDetailsAsync(dto.PhaseId);
            if (phase is null)
                return ApiResponse<bool>.ErrorResponse("Phase not found.");


            // FIX: title guard
            if (string.IsNullOrWhiteSpace(dto.Title))
                return ApiResponse<bool>.ErrorResponse("Phase title is required.");

            if (dto.Title is not null) phase.Title = dto.Title;
            if (dto.Summary is not null) phase.Summary = dto.Summary;

            var incomingTopicIds = dto.Topics.Select(t => t.Id).ToList();
            var topicsToRemove = phase.Topics
                .Where(t => !incomingTopicIds.Contains(t.TopicId))
                .ToList();

            foreach (var topicToDelete in topicsToRemove)
            {
                phase.Topics.Remove(topicToDelete);
            }

            foreach (var topicDto in dto.Topics)
            {
                var existingTopic = phase.Topics.FirstOrDefault(t => t.TopicId == topicDto.Id);

                if (existingTopic is not null)
                {
                   
                    if (topicDto.Title is not null) existingTopic.Title = topicDto.Title;
                    if (topicDto.Summary is not null) existingTopic.Summary = topicDto.Summary;

                  
                    var incomingMaterialIds = topicDto.Materials.Select(m => m.Id).ToList();
                    var materialsToRemove = existingTopic.Materials
                        .Where(m => !incomingMaterialIds.Contains(m.MaterialId))
                        .ToList();

                    foreach (var materialToDelete in materialsToRemove)
                    {
                        existingTopic.Materials.Remove(materialToDelete);
                    }

                    foreach (var materialDto in topicDto.Materials)
                    {
                        var existingMaterial = existingTopic.Materials.FirstOrDefault(m => m.MaterialId == materialDto.Id);
                        if (existingMaterial is not null)
                        {
                            if (materialDto.Title is not null) existingMaterial.Title = materialDto.Title;
                            if (materialDto.Url is not null) existingMaterial.Url = materialDto.Url;
                        }
                        else
                        {
                            existingTopic.Materials.Add(new TopicMaterial
                            {
                                Title = materialDto.Title,
                                Url = materialDto.Url,
                                MaterialType = materialDto.MaterialType
                            });
                        }
                    }

                   
                    var incomingTaskIds = topicDto.Tasks.Select(t => t.Id).ToList();
                    var tasksToRemove = existingTopic.Tasks
                        .Where(t => !incomingTaskIds.Contains(t.TaskId))
                        .ToList();

                    foreach (var taskToDelete in tasksToRemove)
                    {
                        existingTopic.Tasks.Remove(taskToDelete);
                    }

                    foreach (var taskDto in topicDto.Tasks)
                    {
                        var existingTask = existingTopic.Tasks.FirstOrDefault(t => t.TaskId == taskDto.Id);
                        if (existingTask is not null)
                        {
                            if (taskDto.Title is not null) existingTask.Title = taskDto.Title;
                            if (taskDto.Description is not null) existingTask.Description = taskDto.Description;
                            if (taskDto.DeadLine != default) existingTask.DeadLine = taskDto.DeadLine;
                            if (taskDto.AttachmentUrl is not null) existingTask.TaskAttachmentUrl = taskDto.AttachmentUrl;
                        }
                        else
                        {
                            existingTopic.Tasks.Add(new TopicTask
                            {
                                Title = taskDto.Title,
                                Description = taskDto.Description,
                                DeadLine = taskDto.DeadLine,
                                TaskAttachmentUrl = taskDto.AttachmentUrl
                            });
                        }
                    }
                }
                else
                {
              
                    phase.Topics.Add(new Topic
                    {
                        Title = topicDto.Title,
                        Summary = topicDto.Summary,
                        RoadmapPhaseId = phase.RoadmapPhaseId,
                        Materials = topicDto.Materials.Select(m => new TopicMaterial
                        {
                            Title = m.Title,
                            Url = m.Url,
                            MaterialType = m.MaterialType
                        }).ToList(),
                        Tasks = topicDto.Tasks.Select(t => new TopicTask
                        {
                            Title = t.Title,
                            Description = t.Description,
                            DeadLine = t.DeadLine,
                            TaskAttachmentUrl = t.AttachmentUrl
                        }).ToList()
                    });
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Phase updated successfully with all topics and multiple tasks.");
        }
        public async Task<ApiResponse<bool>> DeletePhaseAsync(int phaseId, Guid mentorId)
        {
           
            var phase = await _unitOfWork.RoadmapPhases.GetByIdAsync(phaseId);
            if (phase is null)
                return ApiResponse<bool>.ErrorResponse("Phase not found.");

           
            var roadmap = await _unitOfWork.Roadmaps.GetByIdAsync(phase.RoadmapId);
            if (roadmap.MentorProfileId != mentorId)
                return ApiResponse<bool>.ErrorResponse("Unauthorized.");

         
            _unitOfWork.RoadmapPhases.Delete(phase);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Phase deleted successfully.");
        }

        public async Task<ApiResponse<int>> CreateTopicAsync(CreateTopicDto dto, int phaseId, Guid mentorId)
        {
            var phase = await _unitOfWork.RoadmapPhases.GetByIdWithDetailsAsync(phaseId);

            if (phase is null)
                return ApiResponse<int>.ErrorResponse("Phase not found.");

            var roadmap = await _unitOfWork.Roadmaps.GetByIdWithFullHierarchyAsync(phase.RoadmapId);
            if (roadmap.MentorProfileId != mentorId)
                return ApiResponse<int>.ErrorResponse("Unauthorized.");

            // FIX: title null/empty guard
            if (string.IsNullOrWhiteSpace(dto.Title))
                return ApiResponse<int>.ErrorResponse("Topic title is required.");


            if (roadmap.Phases != null && roadmap.Phases.Any())
            {
                var allTopicsInRoadmap = roadmap.Phases
                    .SelectMany(p => p.Topics ?? new List<Topic>())
                    .ToList();

                if (allTopicsInRoadmap.Any(t => t.Title.Trim().ToLower() == dto.Title.Trim().ToLower()))
                {
                    return ApiResponse<int>.ErrorResponse($"The topic '{dto.Title}' already exists in this roadmap.");
                }

            }
        
          
            var topic = new Topic 
            {
                RoadmapPhaseId = phaseId,
                Title = dto.Title,
                Summary = dto.Summary,
          
            };

            _unitOfWork.Topics.Add(topic);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(topic.TopicId, $"Topic '{topic.Title}' added successfully to phase '{phase.Title}'.");
        }
        public async Task<ApiResponse<bool>> UpdateTopicAsync(TopicDto dto)
        {
           
            var topic = await _unitOfWork.Topics.GetByIdAsync(dto.Id.Value);

            if (topic == null)
                return ApiResponse<bool>.ErrorResponse("Topic not found.");

            // FIX: title guard
            if (string.IsNullOrWhiteSpace(dto.Title))
                return ApiResponse<bool>.ErrorResponse("Topic title is required.");

            topic.Title = dto.Title;
            topic.Summary = dto.Summary;

          
            var incomingMaterialIds = dto.Materials.Select(m => m.Id).ToList();

           
            var materialsToRemove = topic.Materials
                .Where(m => !incomingMaterialIds.Contains(m.MaterialId))
                .ToList();

            foreach (var material in materialsToRemove)
            {
                topic.Materials.Remove(material);
            }

         
            foreach (var materialDto in dto.Materials)
            {
                var existingMaterial = topic.Materials.FirstOrDefault(m => m.MaterialId == materialDto.Id);

                if (existingMaterial != null)
                {
                    existingMaterial.Title = materialDto.Title;
                    existingMaterial.Url = materialDto.Url;
                    existingMaterial.MaterialType = materialDto.MaterialType;
                }
                else
                {
                    topic.Materials.Add(new TopicMaterial
                    {
                        Title = materialDto.Title,
                        Url = materialDto.Url,
                        MaterialType = materialDto.MaterialType
                    });
                }
            }

          
            var incomingTaskIds = dto.Tasks.Select(t => t.Id).ToList();

          
            var tasksToRemove = topic.Tasks
                .Where(t => !incomingTaskIds.Contains(t.TaskId))
                .ToList();

            foreach (var task in tasksToRemove)
            {
                topic.Tasks.Remove(task);
            }

         
            foreach (var taskDto in dto.Tasks)
            {
                var existingTask = topic.Tasks.FirstOrDefault(t => t.TaskId == taskDto.Id);

                if (existingTask != null)
                {
                    existingTask.Title = taskDto.Title;
                    existingTask.Description = taskDto.Description;
                    existingTask.DeadLine = taskDto.DeadLine;
                    existingTask.TaskAttachmentUrl = taskDto.AttachmentUrl; 
                }
                else
                {
                    topic.Tasks.Add(new TopicTask
                    {
                        Title = taskDto.Title,
                        Description = taskDto.Description,
                        DeadLine = taskDto.DeadLine,
                        TaskAttachmentUrl = taskDto.AttachmentUrl
                    });
                }
            }

          
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Topic updated successfully with all materials and tasks.");
        }
        public async Task<ApiResponse<bool>> DeleteTopicAsync(int topicId, Guid mentorId)
        {
            
            var topic = await _unitOfWork.Topics.GetByIdAsync(topicId);
            if (topic is null)
                return ApiResponse<bool>.ErrorResponse("Topic not found.");

         
            var phase = await _unitOfWork.RoadmapPhases.GetByIdAsync(topic.RoadmapPhaseId);
            var roadmap = await _unitOfWork.Roadmaps.GetByIdAsync(phase.RoadmapId);
            if (roadmap.MentorProfileId != mentorId)
                return ApiResponse<bool>.ErrorResponse("Unauthorized.");

            
            _unitOfWork.Topics.Delete(topic);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Topic deleted successfully.");
        }


        public async Task<ApiResponse<List<int>>> CreateMaterialsAsync(ListOfMaterialsDto dto)
        {
          
            var topicExists = await _unitOfWork.Topics.GetByIdAsync(dto.TopicId);

            if (topicExists == null)
                return ApiResponse<List<int>>.ErrorResponse("Topic not found.");

            var existingMaterials = await _unitOfWork.Materials.GetByTopicIdAsync(dto.TopicId);

            var addedEntities = new List<TopicMaterial>();

            foreach (var materialDto in dto.Materials)
            {
            
                bool isDuplicate = existingMaterials.Any(m =>
                    m.Title.Trim().ToLower() == materialDto.Title.Trim().ToLower() );

                if (isDuplicate)
                {
                 
                    return ApiResponse<List<int>>.ErrorResponse($"The material '{materialDto.Title}' already exists in this topic.");
                }

                var material = new TopicMaterial
                {
                    Title = materialDto.Title,
                    Url = materialDto.Url,
                    MaterialType = materialDto.MaterialType,
                    TopicId = dto.TopicId
                };

                _unitOfWork.Materials.Add(material);
                addedEntities.Add(material); 
            }

        
            await _unitOfWork.SaveChangesAsync();

            var newIds = addedEntities.Select(m => m.MaterialId).ToList();

            return ApiResponse<List<int>>.SuccessResponse(newIds, $"{newIds.Count} materials created successfully.");
        }
      
        public async Task<ApiResponse<bool>> UpdateMaterialAsync(MaterialDto dto)
        {
            var material = await _unitOfWork.Materials.GetByIdAsync(dto.Id.Value);

            if (material == null)
                return ApiResponse<bool>.ErrorResponse("Material not found.");

            // FIX: title/URL null guards
            if (string.IsNullOrWhiteSpace(dto.Title))
                return ApiResponse<bool>.ErrorResponse("Material title is required.");
            if (string.IsNullOrWhiteSpace(dto.Url))
                return ApiResponse<bool>.ErrorResponse("Material URL is required.");

            material.Title        = dto.Title.Trim();
            material.Url          = dto.Url.Trim();
            material.MaterialType = dto.MaterialType;


            _unitOfWork.Materials.Update(material);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Material updated successfully.");
        }
        public async Task<ApiResponse<bool>> DeleteMaterialAsync(int id)
        {
           
            var material = await _unitOfWork.Materials.GetByIdAsync(id);

            if (material == null)
                return ApiResponse<bool>.ErrorResponse("Material not found.");

            _unitOfWork.Materials.Delete(material);

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Material deleted successfully.");
        }

        public async Task<ApiResponse<int>> CreateTaskAsync(TaskDto dto)
        {
            var topic = await _unitOfWork.Topics.GetByIdAsync(dto.Id.Value);

            if (topic == null)
                return ApiResponse<int>.ErrorResponse("Topic not found.");

            // FIX: title guard
            if (string.IsNullOrWhiteSpace(dto.Title))
                return ApiResponse<int>.ErrorResponse("Task title is required.");

            // FIX: DeadLine past-date guard and UTC conversion
            if (dto.DeadLine.HasValue &&
                dto.DeadLine.Value.ToUniversalTime() <= DateTime.UtcNow)
                return ApiResponse<int>.ErrorResponse("Task deadline must be in the future.");


            var task = new TopicTask
            {
                Title = dto.Title,
                Description = dto.Description,
                DeadLine = dto.DeadLine,
                TaskAttachmentUrl = dto.AttachmentUrl,
                TopicId = dto.Id.Value,


            };

            _unitOfWork.Tasks.Add(task);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<int>.SuccessResponse(task.TaskId, "Task created successfully.");
        }
        public async Task<ApiResponse<bool>> UpdateTaskAsync(TaskDto dto)
        {
           
            var task = await _unitOfWork.Tasks.GetByIdAsync(dto.Id.Value);

            if (task == null)
                return ApiResponse<bool>.ErrorResponse("Task not found.");
            
            // FIX: title guard
            if (string.IsNullOrWhiteSpace(dto.Title))
                return ApiResponse<bool>.ErrorResponse("Task title is required.");

            // FIX: DeadLine past-date guard and UTC conversion
            if (dto.DeadLine.HasValue &&
                dto.DeadLine.Value.ToUniversalTime() <= DateTime.UtcNow)
                return ApiResponse<bool>.ErrorResponse("Task deadline must be in the future.");

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DeadLine = dto.DeadLine;
            task.TaskAttachmentUrl = dto.AttachmentUrl;



            _unitOfWork.Tasks.Update(task);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Task updated successfully.");
        }
        public async Task<ApiResponse<bool>> DeleteTaskAsync(int id)
        {
            var task = await _unitOfWork.Tasks.GetByIdAsync(id);

            if (task == null)
                return ApiResponse<bool>.ErrorResponse("Task not found.");

      
            _unitOfWork.Tasks.Delete(task);

            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Task deleted successfully.");
        }
        public async Task<ApiResponse<RoadmapContentDto>> GetContentAsync(int roadmapId, Guid mentorId)
        {
            var roadmap = await _unitOfWork.Roadmaps.GetByIdWithFullHierarchyAsync(roadmapId);

            if (roadmap is null)
                return ApiResponse<RoadmapContentDto>.ErrorResponse("Roadmap not found.");

            if (roadmap.MentorProfileId != mentorId)
                return ApiResponse<RoadmapContentDto>.ErrorResponse("Unauthorized.");

            if (roadmap.Status != RoadmapStatus.Draft)
                return ApiResponse<RoadmapContentDto>.ErrorResponse("Roadmap is not in draft mode.");

            return ApiResponse<RoadmapContentDto>.SuccessResponse(new RoadmapContentDto
            {
                RoadmapId = roadmap.RoadmapId,
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
                        }).ToList()

                    }).ToList()
                }).ToList()
            });
        }
        public async Task<ApiResponse<RoadmapDetailsDto>> GetFullRoadmapAsync(int roadmapId, Guid userId, bool isMentor)
        {
            var roadmap = await _unitOfWork.Roadmaps.GetByIdWithFullHierarchyAsync(roadmapId);

            if (roadmap is null)
                return ApiResponse<RoadmapDetailsDto>.ErrorResponse("Roadmap not found.");

          
            if (isMentor && roadmap.MentorProfileId != userId)
                return ApiResponse<RoadmapDetailsDto>.ErrorResponse("Unauthorized.");

         
            if (!isMentor && roadmap.Status != RoadmapStatus.Published)
                return ApiResponse<RoadmapDetailsDto>.ErrorResponse("Roadmap not found.");

            var result = new RoadmapDetailsDto
            {
                RoadmapId = roadmap.RoadmapId,
                Title = roadmap.Title,
                Description = roadmap.Description,
                SkillDomainId = roadmap.SkillDomainId,
                SubDomainId = roadmap.SubDomainId,
                Duration = roadmap.Duration,
                TargetLevelFrom = roadmap.TargetLevelFrom,
                TargetLevelTo = roadmap.TargetLevelTo,
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
                        }).ToList()

                    }).ToList()
                }).ToList()
            };

            return ApiResponse<RoadmapDetailsDto>.SuccessResponse(result);
        }

        public async Task<ApiResponse<List<RoadmapDetailsDto>>> GetAllPublishedRoadmapsAsync(Guid mentorId)
        {
            var roadmaps = await _unitOfWork.Roadmaps.GetAllRoadmapsFullHierarchyAsync(mentorId);

            if (roadmaps == null || !roadmaps.Any())
            {
                return ApiResponse<List<RoadmapDetailsDto>>.SuccessResponse(new List<RoadmapDetailsDto>());
            }

            var results = roadmaps.Select(roadmap => new RoadmapDetailsDto
            {
                RoadmapId = roadmap.RoadmapId,
                Title = roadmap.Title,
                Description = roadmap.Description,
                SkillDomainId = roadmap.SkillDomainId,
                SubDomainId = roadmap.SubDomainId,
                Duration = roadmap.Duration,
                TargetLevelFrom = roadmap.TargetLevelFrom,
                TargetLevelTo = roadmap.TargetLevelTo,
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
                        }).ToList()

                    }).ToList()
                }).ToList()
            }).ToList();

            return ApiResponse<List<RoadmapDetailsDto>>.SuccessResponse(results);
        }
        
        public async Task<ApiResponse<bool>> UpdateRoadmapAsync(int roadmapId, UpdateRoadmapDto dto, Guid mentorId)
        {
            var roadmap = await _unitOfWork.Roadmaps.GetByIdWithFullHierarchyAsync(roadmapId);
            if (roadmap is null)
                return ApiResponse<bool>.ErrorResponse("Roadmap not found.");

            if (roadmap.MentorProfileId != mentorId)
                return ApiResponse<bool>.ErrorResponse("Unauthorized.");

            // FIX: null guard on title — nulls would wipe the existing title
            if (dto.Title != null)
            {
                if (string.IsNullOrWhiteSpace(dto.Title))
                    return ApiResponse<bool>.ErrorResponse("Title cannot be empty.");
                roadmap.Title = dto.Title.Trim();
            }

            if (dto.Description != null)
                roadmap.Description = dto.Description.Trim();

            // FIX: zero/negative guard on Duration
            if (dto.Duration <= 0)
                return ApiResponse<bool>.ErrorResponse("Duration must be greater than zero.");
            roadmap.Duration = dto.Duration;

            // FIX: null guard on TargetLevelFrom/To before Enum.IsDefined — nullable enums
            if (dto.TargetLevelFrom.HasValue &&
                !Enum.IsDefined(typeof(ExperienceLevel), dto.TargetLevelFrom.Value))
                return ApiResponse<bool>.ErrorResponse("Invalid TargetLevelFrom value.");

            if (dto.TargetLevelTo.HasValue &&
                !Enum.IsDefined(typeof(ExperienceLevel), dto.TargetLevelTo.Value))
                return ApiResponse<bool>.ErrorResponse("Invalid TargetLevelTo value.");

            if (dto.TargetLevelFrom.HasValue && dto.TargetLevelTo.HasValue &&
                dto.TargetLevelFrom.Value > dto.TargetLevelTo.Value)
                return ApiResponse<bool>.ErrorResponse("TargetLevelFrom cannot be greater than TargetLevelTo.");

            roadmap.TargetLevelFrom = dto.TargetLevelFrom ?? roadmap.TargetLevelFrom;
            roadmap.TargetLevelTo   = dto.TargetLevelTo   ?? roadmap.TargetLevelTo;

            // FIX: null guard on TechnologyIds — Clear() on null list would crash
            if (dto.TechnologyIds != null)
            {
                roadmap.RoadmapTechnologies.Clear();
                foreach (var techId in dto.TechnologyIds)
                    roadmap.RoadmapTechnologies.Add(new RoadmapTechnology { TechnologyId = techId });
            }

            // FIX: removed published-roadmap deletion block — mentors can now freely
            // delete phases, topics, tasks and materials regardless of status.
            if (dto.Phases != null)
            {
                var incomingPhaseIds = dto.Phases
                    .Where(p => p.PhaseId.HasValue)
                    .Select(p => p.PhaseId.Value)
                    .ToList();

                var phasesToRemove = roadmap.Phases
                    .Where(p => !incomingPhaseIds.Contains(p.RoadmapPhaseId))
                    .ToList();
                foreach (var phase in phasesToRemove)
                    roadmap.Phases.Remove(phase);

                foreach (var phaseDto in dto.Phases)
                {
                    var existingPhase = roadmap.Phases
                        .FirstOrDefault(p => p.RoadmapPhaseId == phaseDto.PhaseId);

                    if (existingPhase is not null)
                    {
                        if (!string.IsNullOrWhiteSpace(phaseDto.Title))
                            existingPhase.Title = phaseDto.Title.Trim();
                        existingPhase.Summary = phaseDto.Summary?.Trim();

                        if (phaseDto.Topics != null)
                        {

                            var incomingTopicIds = phaseDto.Topics
                                .Where(t => t.Id.HasValue)
                                .Select(t => t.Id.Value)
                                .ToList();

                            var topicsToRemove = existingPhase.Topics
                                .Where(t => !incomingTopicIds.Contains(t.TopicId))
                                .ToList();
                            foreach (var t in topicsToRemove)
                                existingPhase.Topics.Remove(t);

                            foreach (var topicDto in phaseDto.Topics)
                            {
                                var existingTopic = existingPhase.Topics
                                    .FirstOrDefault(t => t.TopicId == topicDto.Id);

                                if (existingTopic is not null)
                                {
                                    if (!string.IsNullOrWhiteSpace(topicDto.Title))
                                        existingTopic.Title = topicDto.Title.Trim();
                                    existingTopic.Summary = topicDto.Summary?.Trim();

                                    // Materials
                                    if (topicDto.Materials != null)
                                    {
                                        var incomingMatIds = topicDto.Materials
                                            .Where(m => m.Id.HasValue)
                                            .Select(m => m.Id.Value)
                                            .ToList();
                                        var matsToRemove = existingTopic.Materials
                                            .Where(m => !incomingMatIds.Contains(m.MaterialId))
                                            .ToList();
                                        foreach (var m in matsToRemove)
                                            existingTopic.Materials.Remove(m);

                                        foreach (var matDto in topicDto.Materials)
                                        {
                                            if (string.IsNullOrWhiteSpace(matDto.Title) ||
                                                string.IsNullOrWhiteSpace(matDto.Url))
                                                return ApiResponse<bool>.ErrorResponse(
                                                    "Material title and URL are required.");

                                            var existingMat = existingTopic.Materials
                                                .FirstOrDefault(m => m.MaterialId == matDto.Id);
                                            if (existingMat != null)
                                            {
                                                existingMat.Title        = matDto.Title.Trim();
                                                existingMat.Url          = matDto.Url.Trim();
                                                existingMat.MaterialType = matDto.MaterialType;
                                            }
                                            else
                                            {
                                                existingTopic.Materials.Add(new TopicMaterial
                                                {
                                                    Title        = matDto.Title.Trim(),
                                                    Url          = matDto.Url.Trim(),
                                                    MaterialType = matDto.MaterialType
                                                });
                                            }
                                        }
                                    }

                                    // Tasks — FIX: was single Task, now collection Tasks
                                    if (topicDto.Tasks != null)
                                    {
                                        var incomingTaskIds = topicDto.Tasks
                                            .Where(t => t.Id.HasValue)
                                            .Select(t => t.Id.Value)
                                            .ToList();
                                        var tasksToRemove = existingTopic.Tasks
                                            .Where(t => !incomingTaskIds.Contains(t.TaskId))
                                            .ToList();
                                        foreach (var task in tasksToRemove)
                                            existingTopic.Tasks.Remove(task);

                                        foreach (var taskDto in topicDto.Tasks)
                                        {
                                            if (string.IsNullOrWhiteSpace(taskDto.Title))
                                                return ApiResponse<bool>.ErrorResponse(
                                                    "Task title is required.");

                                            // FIX: DeadLine past-date guard
                                            if (taskDto.DeadLine.HasValue &&
                                                taskDto.DeadLine.Value.ToUniversalTime() <= DateTime.UtcNow)
                                                return ApiResponse<bool>.ErrorResponse(
                                                    $"Task '{taskDto.Title}' deadline must be in the future.");

                                            var existingTask = existingTopic.Tasks
                                                .FirstOrDefault(t => t.TaskId == taskDto.Id);
                                            if (existingTask != null)
                                            {
                                                existingTask.Title             = taskDto.Title.Trim();
                                                existingTask.Description       = taskDto.Description?.Trim();
                                                // FIX: was TaskAttachmentUrl = taskDto.AttachmentUrl
                                                // which was never mapped in old code
                                                existingTask.DeadLine          = taskDto.DeadLine.HasValue
                                                    ? taskDto.DeadLine.Value.ToUniversalTime()
                                                    : null;
                                            }
                                            else
                                            {
                                                existingTopic.Tasks.Add(new TopicTask
                                                {
                                                    Title       = taskDto.Title.Trim(),
                                                    Description = taskDto.Description?.Trim(),
                                                    DeadLine    = taskDto.DeadLine.HasValue
                                                        ? taskDto.DeadLine.Value.ToUniversalTime()
                                                        : null,
                                                    TopicId     = existingTopic.TopicId
                                                });
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // FIX: new topic — was silently ignoring all nested data
                                    var newTopic = new Topic
                                    {
                                        Title          = topicDto.Title?.Trim() ?? string.Empty,
                                        Summary        = topicDto.Summary?.Trim(),
                                        RoadmapPhaseId = existingPhase.RoadmapPhaseId
                                    };

                                    // FIX: populate materials on new topic
                                    if (topicDto.Materials != null)
                                    {
                                        foreach (var matDto in topicDto.Materials)
                                        {
                                            if (string.IsNullOrWhiteSpace(matDto.Title) ||
                                                string.IsNullOrWhiteSpace(matDto.Url))
                                                return ApiResponse<bool>.ErrorResponse(
                                                    "Material title and URL are required.");
                                            newTopic.Materials.Add(new TopicMaterial
                                            {
                                                Title        = matDto.Title.Trim(),
                                                Url          = matDto.Url.Trim(),
                                                MaterialType = matDto.MaterialType
                                            });
                                        }
                                    }

                                    // FIX: populate tasks on new topic
                                    if (topicDto.Tasks != null)
                                    {
                                        foreach (var taskDto in topicDto.Tasks)
                                        {
                                            if (string.IsNullOrWhiteSpace(taskDto.Title))
                                                return ApiResponse<bool>.ErrorResponse(
                                                    "Task title is required.");
                                            if (taskDto.DeadLine.HasValue &&
                                                taskDto.DeadLine.Value.ToUniversalTime() <= DateTime.UtcNow)
                                                return ApiResponse<bool>.ErrorResponse(
                                                    $"Task '{taskDto.Title}' deadline must be in the future.");
                                            newTopic.Tasks.Add(new TopicTask
                                            {
                                                Title       = taskDto.Title.Trim(),
                                                Description = taskDto.Description?.Trim(),
                                                DeadLine    = taskDto.DeadLine.HasValue
                                                    ? taskDto.DeadLine.Value.ToUniversalTime()
                                                    : null
                                            });
                                        }
                                    }

                                    existingPhase.Topics.Add(newTopic);
                                }
                            }
                        }
                    }
                    else
                    {
                        // FIX: new phase — was silently ignoring all nested topics
                        var newPhase = new RoadmapPhase
                        {
                            Title    = phaseDto.Title?.Trim() ?? string.Empty,
                            Summary  = phaseDto.Summary?.Trim(),
                            RoadmapId = roadmapId
                        };

                        if (phaseDto.Topics != null)
                        {
                            foreach (var topicDto in phaseDto.Topics)
                            {
                                var newTopic = new Topic
                                {
                                    Title   = topicDto.Title?.Trim() ?? string.Empty,
                                    Summary = topicDto.Summary?.Trim(),
                                };

                                if (topicDto.Materials != null)
                                {
                                    foreach (var matDto in topicDto.Materials)
                                    {
                                        if (string.IsNullOrWhiteSpace(matDto.Title) ||
                                            string.IsNullOrWhiteSpace(matDto.Url))
                                            return ApiResponse<bool>.ErrorResponse(
                                                "Material title and URL are required.");
                                        newTopic.Materials.Add(new TopicMaterial
                                        {
                                            Title        = matDto.Title.Trim(),
                                            Url          = matDto.Url.Trim(),
                                            MaterialType = matDto.MaterialType
                                        });
                                    }
                                }

                                if (topicDto.Tasks != null)
                                {
                                    foreach (var taskDto in topicDto.Tasks)
                                    {
                                        if (string.IsNullOrWhiteSpace(taskDto.Title))
                                            return ApiResponse<bool>.ErrorResponse(
                                                "Task title is required.");
                                        if (taskDto.DeadLine.HasValue &&
                                            taskDto.DeadLine.Value.ToUniversalTime() <= DateTime.UtcNow)
                                            return ApiResponse<bool>.ErrorResponse(
                                                $"Task '{taskDto.Title}' deadline must be in the future.");
                                        newTopic.Tasks.Add(new TopicTask
                                        {
                                            Title       = taskDto.Title.Trim(),
                                            Description = taskDto.Description?.Trim(),
                                            DeadLine    = taskDto.DeadLine.HasValue
                                                ? taskDto.DeadLine.Value.ToUniversalTime()
                                                : null
                                        });
                                    }
                                }

                                newPhase.Topics.Add(newTopic);
                            }
                        }

                        roadmap.Phases.Add(newPhase);
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Roadmap updated successfully.");
        }
        public async Task<ApiResponse<bool>> DeleteRoadmapAsync(int roadmapId, Guid mentorId)
        {
          
            var roadmap = await _unitOfWork.Roadmaps.GetByIdAsync(roadmapId);
            if (roadmap is null)
                return ApiResponse<bool>.ErrorResponse("Roadmap not found.");

       
            if (roadmap.MentorProfileId != mentorId)
                return ApiResponse<bool>.ErrorResponse("Unauthorized.");

        
            _unitOfWork.Roadmaps.Delete(roadmap);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Roadmap deleted successfully.");
        }
        public async Task<ApiResponse<bool>> PublishRoadmapAsync(int roadmapId, Guid mentorId)
        {
            var roadmap = await _unitOfWork.Roadmaps.GetByIdWithFullHierarchyAsync(roadmapId);
            if (roadmap is null)
                return ApiResponse<bool>.ErrorResponse("Roadmap not found.");

            if (roadmap.MentorProfileId != mentorId)
                return ApiResponse<bool>.ErrorResponse("Unauthorized.");
      
            roadmap.Status = RoadmapStatus.Published;
            _unitOfWork.Roadmaps.Update(roadmap);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Roadmap published successfully.");
        }
        public async Task<ApiResponse<bool>> UnpublishRoadmapAsync(int roadmapId, Guid mentorId)
        {
            var roadmap = await _unitOfWork.Roadmaps.GetByIdAsync(roadmapId);

            if (roadmap is null)
                return ApiResponse<bool>.ErrorResponse("Roadmap not found.");

            if (roadmap.MentorProfileId != mentorId)
                return ApiResponse<bool>.ErrorResponse("Unauthorized.");

          
            if (roadmap.Status != RoadmapStatus.Published)
                return ApiResponse<bool>.ErrorResponse("Roadmap is not currently published.");
         

            roadmap.Status = RoadmapStatus.Draft;

            _unitOfWork.Roadmaps.Update(roadmap);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Roadmap unpublished successfully.");
        }

    }
}
    

