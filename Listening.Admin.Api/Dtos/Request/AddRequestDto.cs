using FluentValidation;

namespace Listening.Admin.Api.Dtos.Request
{
    public class AddRequestDto
    {
        public string Title { get;  set; }
        public long ForeginId { get;  set; }
        public Uri? CoverImgUrl { get;  set; }
    }


    public class AddRequestDtoValidator : AbstractValidator<AddRequestDto>
    {
        public AddRequestDtoValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 500);

            RuleFor(x => x.ForeginId)
                .GreaterThan(0).WithMessage("ForeginId must be at greater than 0.");
        }
    }
}
