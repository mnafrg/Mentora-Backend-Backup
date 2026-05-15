namespace Mentora.Domain.Enums.Interactions.Report;

// How Admin resolved the reported CONTENT 
public enum ContentAction
{
    None           = 0,
    Approved       = 1,   // No violation – content stays
    ContentDeleted = 2    // Content removed
}