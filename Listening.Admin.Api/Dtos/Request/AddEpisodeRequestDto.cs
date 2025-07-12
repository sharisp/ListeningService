using FluentValidation;

namespace Listening.Admin.Api.Dtos.Request
{
    public class AddEpisodeRequestDto
    {
        public long AlumId { get; set; }
        public string Title { get; set; }
        public string SubtitleType { get; set; }
        public string SubtitleContent { get; set; }
        public Uri AudioUrl { get; set; }
        public long DurationInSeconds { get; set; }
        public Uri? CoverImgUrl { get; set; }

    }
    public class AddEpisodeRequestDtoValidator : AbstractValidator<AddEpisodeRequestDto>
    {
        public AddEpisodeRequestDtoValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 500);

            RuleFor(x => x.SubtitleType)
                .NotEmpty().WithMessage("SubtitleType is required.");

            RuleFor(x => x.SubtitleContent)
                .NotEmpty().WithMessage("SubtitleContent is required.");
            RuleFor(x => x.AudioUrl)
                .NotEmpty().WithMessage("AudioUrl is required.");
            RuleFor(x => x.AudioUrl)
                .NotEmpty().WithMessage("AudioUrl is required.");

        }
    }
}
