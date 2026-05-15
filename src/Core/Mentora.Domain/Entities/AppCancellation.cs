using Mentora.Domain.Enums;
using Mentora.Domain.Enums.CancellationReasons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Entities;

public class AppCancellation
{
    public int CancelledId { get; set; }

    public int ApplicationId { get; set; }
   
    public int ProgramId { get; set; }
   
    public Guid MenteeId { get; set; }
   public MenteeProfile MenteeProfile { get; set; } = null!;
   

    public DateTime CancellationDate { get; set; } 

    public AppCancellationReason CancellationReason { get; set; }
}
