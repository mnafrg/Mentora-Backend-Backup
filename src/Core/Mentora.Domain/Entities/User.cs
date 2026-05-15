using Mentora.Domain.Entities.Profiles;
using Mentora.Domain.Enums; 
using System;
using System.Collections.Generic;

namespace Mentora.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public UserRole? Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; } = false;

        public MenteeProfile? MenteeProfile { get; set; }
        public MentorProfile? MentorProfile { get; set; }
       
        public ICollection<EmailVerificationToken> EmailVerificationTokens { get; set; } = new List<EmailVerificationToken>();
        public ICollection<PostLike> LikedPosts { get; set; } = new List<PostLike>();
        public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
        public ICollection<PostComment> MyComments { get; set; } = new List<PostComment>();
        public ICollection<SharedPost> SharedPosts { get; set; } = new List<SharedPost>();
    }
}

