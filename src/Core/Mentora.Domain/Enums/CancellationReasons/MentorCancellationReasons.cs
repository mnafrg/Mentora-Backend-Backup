using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Enums.CancellationReasons
{
   
   public enum MentorCancellationReasons
    {
        MenteeUnresponsive = 1,
        MenteeNotCommitted = 2,
        GoalsNotClear = 3,
        BehaviorIssue = 4,
        PaceTooSlow = 5,
        ScopeChanged = 6,
        MentorAvailabilityChanged = 7
    }
}
