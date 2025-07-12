using FluentValidation;

namespace Listening.Admin.Api.Dtos.Request
{
    public record AlbumRequestDto
    {
        public string Title { get;  set; }
        public long CategoryId { get;  set; }
        public Uri? CoverImgUrl { get;  set; }
    }


    public class AlbumRequestDtoValidator : AbstractValidator<AlbumRequestDto>
    {
        public AlbumRequestDtoValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 500);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be at greater than 0.");
        }
    }
}
