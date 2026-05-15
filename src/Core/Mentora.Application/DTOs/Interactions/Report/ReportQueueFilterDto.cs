namespace Mentora.Application.DTOs.Interactions.Report;
using Mentora.Domain.Enums.Interactions.Report;

public class ReportQueueFilterDto
{
    public ReportedItemStatus? Status { get; set; }
    public ReportTargetType?   TargetType { get; set; }
 
    ///Sort options: HighestScore | MostRecent
    public string SortBy { get; set; } = "HighestScore";
    public int Page     { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}