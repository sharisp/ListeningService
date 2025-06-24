using FluentValidation;

namespace Listening.Admin.Api.Dtos.Request
{
    public class AddKindRequestDto
    {
        public string Title { get; set; }
        public Uri? CoverImgUrl { get; set; }
    }

    public class AddKindRequestDtoValidator : AbstractValidator<AddKindRequestDto>
    {
        public AddKindRequestDtoValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 500);

        }
    }
}
