namespace Mentora.Application.DTOs.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;

public class AdminActionRequest
{
    // What to do with the content: None | Approved | ContentDeleted
    public ContentAction ContentAction { get; set; }
 
    // What to do with the user: None | Warning | TemporaryBan | PermanentBan
    public UserAction UserAction { get; set; }
    /// Required when UserAction = TemporaryBan.
    /// Duration in hours (e.g. 24 or 168 for 7 days).
    public int? BanDurationHours { get; set; }

    // Optional message shown to the warned/banned user and stored in the
    public string? UserActionMessage { get; set; }
 
    //mInternal admin note stored on the ReportedItem.
    public string? AdminNotes { get; set; }
}
 