using FluentValidation;

namespace Listening.Admin.Api.Dtos.Request
{
    public class UpdateEpisodeRequestDto
    {
        public long AlbumId { get; set; }
        public string Title { get; set; }
        public string SubtitleType { get; set; }
        public string SubtitleContent { get; set; }
        public Uri AudioUrl { get; set; }
        public long DurationInSeconds { get; set; }
        public Uri? CoverImgUrl { get; set; }
        public int SequenceNumber { get; set; }
    }

    public class UpdateEpisodeRequestDtoValidator : AbstractValidator<UpdateEpisodeRequestDto>
    {
        public UpdateEpisodeRequestDtoValidator()
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
            RuleFor(x => x.SequenceNumber)
             .NotEmpty().WithMessage("SequenceNumber is required.");

        }
    }
}
