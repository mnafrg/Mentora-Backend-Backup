using global::Mentora.Application.Interfaces;
using global::Mentora.Application.Interfaces.Services;
using Mentora.Application.DTOs.Common;
using Mentora.Application.DTOs.Explore;
using Mentora.Application.DTOs.Explore.Mentora.Application.DTOs.Explore;
using Mentora.Application.DTOs.Roadmap;
using Mentora.Domain.Entities;
using Mentora.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Services
{
    public class ExploreService : IExploreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExploreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<List<MentorExploreDto>>> ExploreMentorsAsync(ExploreSearchRequest request)
        {
            var allProfiles = await _unitOfWork.MentorProfiles.GetAllWithUserAsync();

            var query = allProfiles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchTerm = request.SearchQuery.ToLower();

                query = query.Where(p =>
                  (p.User != null && (p.User.FirstName.ToLower().Contains(searchTerm) ||
                   p.User.LastName.ToLower().Contains(searchTerm) ||
                   (p.User.FirstName + " " + p.User.LastName).ToLower().Contains(searchTerm))) ||
                   (p.Bio != null && p.Bio.ToLower().Contains(searchTerm)) ||
                   (p.Domain != null && p.Domain.Name.ToLower().Contains(searchTerm))
                );
            }

            var results = query.Select(p => new MentorExploreDto
            {
                MentorId = p.UserId,
                FullName = $"{p.User.FirstName} {p.User.LastName}",
                Bio = p.Bio,
                // JobTitle = p.JobTitle ?? "Expert",
                ProfileImageUrl = p.ProfilePictureUrl,
                Rating = p.AverageRating,
                DomainName = p.Domain.Name,

            }).ToList();

            return ApiResponse<List<MentorExploreDto>>.SuccessResponse(results);
        }
        public async Task<ApiResponse<List<ProgramExploreDto>>> ExploreProgramsAsync(ExploreSearchRequest request)
        {

            var allPrograms = await _unitOfWork.Programs.GetAllAsync(
          filter: p => p.ProgramPostStatus == ProgramPostStatus.Published,
          includes: new Expression<Func<Program, object>>[] {
            p => p.MentorProfile,
            p => p.MentorProfile.User,
            p => p.Domain,
            p => p.SubDomain
          }
      );
            var query = allPrograms.AsQueryable();


            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchTerm = request.SearchQuery.ToLower();
                query = query.Where(p =>
                    p.Title.ToLower().Contains(searchTerm) ||
                    (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                     (p.MentorProfile.User.FirstName + " " + p.MentorProfile.User.LastName).ToLower().Contains(searchTerm)
                );
            }


            var results = query.Select(p => new ProgramExploreDto
            {
                Id = p.ProgramId,
                Title = p.Title,
                Description = p.Description,
                MentorName = $"{p.MentorProfile.User.FirstName} {p.MentorProfile.User.LastName}",
                DomainName = p.Domain.Name,
                SubDomainName = p.SubDomain.Name,
                MentorProfileImageUrl = p.MentorProfile.ProfilePictureUrl,
            }).ToList();

            return ApiResponse<List<ProgramExploreDto>>.SuccessResponse(results);
        }

        public async Task<ApiResponse<List<RoadmapExploreDto>>> ExploreRoadmapsAsync(ExploreSearchRequest request)
        {
            
            var allRoadmaps = await _unitOfWork.Roadmaps.GetAllBasicInfoAsync();

            var filteredRoadmaps = allRoadmaps.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchTerm = request.SearchQuery.Trim().ToLower();

                filteredRoadmaps = filteredRoadmaps.Where(r =>
                    r.Title.ToLower().Contains(searchTerm) ||
                    r.Description.ToLower().Contains(searchTerm) ||
                    (r.SkillDomain != null && r.SkillDomain.Name.ToLower().Contains(searchTerm)) ||
                    (r.SubDomain != null && r.SubDomain.Name.ToLower().Contains(searchTerm))
                );
            }

            var results = filteredRoadmaps.Select(r => new RoadmapExploreDto
            {
                RoadmapId = r.RoadmapId,
                Title = r.Title,
                Description = r.Description,
                SkillDomainId = r.SkillDomainId,
                SubDomainId = r.SubDomainId,
                Duration = r.Duration,
                TargetLevelFrom = r.TargetLevelFrom,
                TargetLevelTo = r.TargetLevelTo,

               
                TechnologyIds = r.RoadmapTechnologies.Select(rt => rt.TechnologyId).ToList(),
                PhasesCount = r.Phases.Count()
            }).ToList();

      
            return ApiResponse<List<RoadmapExploreDto>>.SuccessResponse(results);
        }
        public async Task<ApiResponse<List<CommunityExploreDto>>> ExploreCommunitiesAsync(ExploreSearchRequest request)
        {
            var allCommunities = await _unitOfWork.Communities.GetAllCommunitiesAsync();

            var query = allCommunities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchQuery))
            {
                var searchTerm = request.SearchQuery.Trim().ToLower();

                query = query.Where(c =>
                    c.Name.ToLower().Contains(searchTerm) ||
                    (c.Description != null && c.Description.ToLower().Contains(searchTerm))
                );
            }

         
            var results = query.Select(c => new CommunityExploreDto
            {
                CommunityId = c.CommunityId,
                Name = c.Name,
                Description = c.Description,
                CoverImageUrl = c.CoverImageUrl,
                DomainId = c.DomainId,
                CreatorName = c.CreatedByUser != null
                ? c.CreatedByUser.FirstName + " " + c.CreatedByUser.LastName
                : "Unknown Creator",

                MembersCount = c.Members != null ? c.Members.Count : 0,
                PostsCount = c.Posts != null ? c.Posts.Count : 0
            }).ToList();

            return ApiResponse<List<CommunityExploreDto>>.SuccessResponse(results);
        }
    }
}