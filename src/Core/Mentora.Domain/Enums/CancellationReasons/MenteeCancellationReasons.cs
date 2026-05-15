using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Enums.CancellationReasons
{
  
   public enum MenteeCancellationReasons
    {
        NoTimeCommitment = 1,
        MentorNotMatching = 2,
        LearningPaceMismatch = 3,
        PersonalIssue = 4,
        WorkloadIncreased = 5,
        LostMotivation = 6,
        CommunicationIssues = 7,
        SwitchedLearningGoal = 8
    }
}
