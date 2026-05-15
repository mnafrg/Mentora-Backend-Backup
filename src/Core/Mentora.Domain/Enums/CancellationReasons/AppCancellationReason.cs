using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mentora.Domain.Enums.CancellationReasons;

public enum AppCancellationReason
{
    ScheduleConflict = 1,
    AcceptedOtherOffer = 2,
    LostInterest = 3,
    AvailabilityChanged = 4,
    TimeZoneIssue = 5,
    NotReadyYet = 6,
    PersonalIssue = 7,
    ApplicationSentByMistake = 8
}
