using Mentora.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 namespace Mentora.Domain.Entities;

    public class MentorshipCancellation
    {
      
        public int Id { get; set; }

      
        public int MentorshipId { get; set; }

       
        public int ProgramId { get; set; }

       
        public Guid MenteeProfileId { get; set; }
    public  MenteeProfile MenteeProfile { get; set; } = null!;
    public Guid MentorProfileId { get; set; }
    public MentorProfile MentorProfile { get; set; } = null!;

    public DateTime CancellationDate { get; set; } = DateTime.UtcNow;

       
        public CancellationActor CancellationActor { get; set; }


    public string CancellationReasonValue { get; set; } = null!;
    }

