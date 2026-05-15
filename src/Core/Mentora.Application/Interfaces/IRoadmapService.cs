using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Roadmap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces
{
    public interface IRoadmapService
    {
        Task<ApiResponse<int>> CreateBasicInfoAsync(CreateRoadmapBasicInfoDto dto, Guid mentorId);
        Task<ApiResponse<List<RoadmapBasicInfoDto>>> GetAllRoadmapsBasicInfoAsync();
        Task<ApiResponse<bool>> ToggleSaveRoadmapAsync(int roadmapId, Guid userId);
        Task<ApiResponse<IEnumerable<RoadmapDetailsDto>>> GetSavedRoadmapsAsync(Guid userId);

        Task<ApiResponse<int>> CreatePhaseAsync(CreateRoadmapPhaseDto dto, Guid mentorId);
        Task<ApiResponse<bool>> UpdatePhaseAsync(PhaseDto dto);
        Task<ApiResponse<bool>> DeletePhaseAsync(int phaseId, Guid mentorId);


        Task<ApiResponse<int>> CreateTopicAsync(CreateTopicDto dto, int phaseId, Guid mentorId);
        Task<ApiResponse<bool>> UpdateTopicAsync(TopicDto dto);
        Task<ApiResponse<bool>> DeleteTopicAsync(int topicId, Guid mentorId);

        Task<ApiResponse<List<int>>> CreateMaterialsAsync(ListOfMaterialsDto dto);
        Task<ApiResponse<bool>> UpdateMaterialAsync(MaterialDto dto);
        Task<ApiResponse<bool>> DeleteMaterialAsync(int id);



      Task<ApiResponse<int>> CreateTaskAsync(TaskDto dto);
        Task<ApiResponse<bool>> UpdateTaskAsync(TaskDto dto);
        Task<ApiResponse<bool>> DeleteTaskAsync(int id);


        Task<ApiResponse<RoadmapContentDto>> GetContentAsync(int roadmapId, Guid mentorId);
        Task<ApiResponse<RoadmapDetailsDto>> GetFullRoadmapAsync(int roadmapId, Guid userId, bool isMentor);
        Task<ApiResponse<List<RoadmapDetailsDto>>> GetAllPublishedRoadmapsAsync(Guid mentorId);
        
            Task<ApiResponse<bool>> UpdateRoadmapAsync(int roadmapId, UpdateRoadmapDto dto, Guid mentorId);
        Task<ApiResponse<bool>> DeleteRoadmapAsync(int roadmapId, Guid mentorId);
        Task<ApiResponse<bool>> PublishRoadmapAsync(int roadmapId, Guid mentorId);
        Task<ApiResponse<bool>> UnpublishRoadmapAsync(int roadmapId, Guid mentorId);




      
    }
}
