using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::Mentora.Application.DTOs.Common;
using global::Mentora.Application.DTOs.Explore;
using global::Mentora.Application.DTOs.Explore.Mentora.Application.DTOs.Explore;
using Mentora.Application.DTOs.Explore;

namespace Mentora.Application.Interfaces.Services
{
    public interface IExploreService
        {
          
            Task<ApiResponse<List<RoadmapExploreDto>>> ExploreRoadmapsAsync(ExploreSearchRequest request);

            Task<ApiResponse<List<MentorExploreDto>>> ExploreMentorsAsync(ExploreSearchRequest request);

            Task<ApiResponse<List<ProgramExploreDto>>> ExploreProgramsAsync(ExploreSearchRequest request);
        Task<ApiResponse<List<CommunityExploreDto>>> ExploreCommunitiesAsync(ExploreSearchRequest request);
        }
    }

