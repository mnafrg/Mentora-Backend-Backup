using Mentora.Application.DTOs;
using Mentora.Application.DTOs.Common;
using Mentora.Application.Interfaces;
using Mentora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Services
{
    public class CommentService :  ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<IEnumerable<CommentDto>>> GetCommentsAsync(int programId)
        {
            var comments = await _unitOfWork.Comments.GetAllAsync(
                filter: c => c.ProgramId == programId && !c.IsDeleted,
                includes: new Expression<Func<PostComment, object>>[] {
            c => c.User.MentorProfile,
            c => c.User.MenteeProfile
                }
            );

            var response = comments.Select(c => new CommentDto
            {
                CommentId = c.CommentId,
                CommentText = c.CommentText,
                UserName = c.User.FirstName + " " + c.User.LastName,
                CreatedAt = DateTime.Now,
                UserProfilePicture = c.User.MentorProfile != null
                                 ? c.User.MentorProfile?.ProfilePictureUrl
                                 : (c.User.MenteeProfile != null
                                    ? c.User.MenteeProfile.ProfilePictureUrl : null)

            });

            return ApiResponse<IEnumerable<CommentDto>>.SuccessResponse(response, "Success");
        }

        public async Task<ApiResponse<bool>> AddCommentAsync(int programId, Guid userId, string commentText)
        {
            var comment = new PostComment
            {
                ProgramId = programId,
                UserId = userId,
                CommentText = commentText
            };

            await _unitOfWork.Comments.CreateAsync(comment);
            await _unitOfWork.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Comment added successfully");
        }
    }
}
