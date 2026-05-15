using FluentValidation;
using Mentora.Application.DTOs.Auth;
using Mentora.Domain.Enums;
namespace Mentora.Application.Validators;

public class CompleteMenteeProfileRequestValidator : AbstractValidator<CompleteMenteeProfileRequest>
{
    public CompleteMenteeProfileRequestValidator()
    {
        RuleFor(x => x.DomainId)
            .GreaterThan(0)
            .WithMessage("Career field must be selected");

        RuleFor(x => x.EducationStatus)
            .NotEmpty()
            .WithMessage("Educational level is required")
            .Must(BeValidEducationStatus)
            .WithMessage("Invalid educational level");

        RuleFor(x => x.CurrentLevel)
            .NotEmpty()
            .WithMessage("Current level is required")
            .Must(level => Enum.TryParse<CurrentLevel>(level, true, out _))
            .WithMessage("Invalid current level. Valid values are: Beginner, Junior, Mid, Senior");
            
        RuleFor(x => x.CareerGoalId)
            .GreaterThan(0)
            .When(x => x.CareerGoalId.HasValue)
            .WithMessage("Invalid career goal");

        RuleFor(x => x.LearningStyleId)
            .GreaterThan(0)
            .When(x => x.LearningStyleId.HasValue)
            .WithMessage("Invalid learning style");


        // Relevant expertise (SubDomains) - Multi-select
        RuleFor(x => x.SubDomainIds)
            .NotEmpty()
            .WithMessage("At least one area of expertise must be selected");


        // Tools (Technologies) - 1 to 5 selections
         RuleFor(x => x.TechnologyInterests)
            .NotEmpty()
            .WithMessage("At least one tool must be selected")
            .Must(x => x.Count >= 1 && x.Count <= 5)
            .WithMessage("Please select between 1 and 5 tools");

        // Validate each technology interest entry
        RuleForEach(x => x.TechnologyInterests)
            .ChildRules(interest =>
            {
                interest.RuleFor(x => x.TechnologyId)
                    .GreaterThan(0)
                    .WithMessage("Invalid technology ID");

                interest.RuleFor(x => x.ExperienceLevel)
                    .NotEmpty()
                    .WithMessage("Experience level is required for each technology")
                    .Must(BeValidExperienceLevel)
                    .WithMessage("Invalid experience level for technology. Valid values are: None, Beginner, Intermediate, Advanced");
            });
                // Ensure no duplicate technologies
        RuleFor(x => x.TechnologyInterests)
            .Must(HaveUniqueTechnologies)
            .When(x => x.TechnologyInterests != null && x.TechnologyInterests.Any())
            .WithMessage("Cannot select the same technology multiple times");

        RuleFor(x => x.Bio)
            .MaximumLength(1000)
            .WithMessage("Bio cannot exceed 1000 characters");

        RuleFor(x => x.CountryCode)
            .Length(2)
            .When(x => !string.IsNullOrEmpty(x.CountryCode))
            .WithMessage("Invalid country code");
    }

    private bool BeValidEducationStatus(string status)
    {
        return Enum.TryParse<Domain.Enums.EducationStatus>(status, true, out _);
    }

    private bool BeValidExperienceLevel(string level)
    {
        return Enum.TryParse<Domain.Enums.ExperienceLevel>(level, true, out _);
    }
    private bool HaveUniqueTechnologies(List<TechnologyInterestDto> interests)
    {
        if (interests == null || !interests.Any())
            return true;

        var technologyIds = interests.Select(x => x.TechnologyId).ToList();
        return technologyIds.Count == technologyIds.Distinct().Count();
    }
}