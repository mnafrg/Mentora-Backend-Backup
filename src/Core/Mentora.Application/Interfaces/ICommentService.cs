using Mentora.Application.DTOs;
using Mentora.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Application.Interfaces
{
    public interface ICommentService
    {
        Task<ApiResponse<IEnumerable<CommentDto>>> GetCommentsAsync(int programId);
        Task<ApiResponse<bool>> AddCommentAsync(int programId, Guid menteeProfileId, string commentText);
    }
}
