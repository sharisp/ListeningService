using FluentValidation;

namespace Listening.Admin.Api.Dtos.Request
{
    public record CategoryRequestDto
    {
        public string Title { get; set; }
        public long KindId { get; set; }
        public Uri? CoverImgUrl { get; set; }
    }

    public class CategoryRequestDtoValidator : AbstractValidator<CategoryRequestDto>
    {
        public CategoryRequestDtoValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 500);

            RuleFor(x => x.KindId)
                .GreaterThan(0).WithMessage("KindId must be at greater than 0.");
        }
    }
}
