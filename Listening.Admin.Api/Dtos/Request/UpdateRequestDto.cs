using FluentValidation;

namespace Listening.Admin.Api.Dtos.Request
{
    public class UpdateRequestDto
    {
        public string Title { get;  set; }

        public Uri? CoverImgUrl { get; set; }
        public int SequenceNumber { get; set; }
    }


    public class UpdateRequestDtoValidator : AbstractValidator<UpdateRequestDto>
    {
        public UpdateRequestDtoValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 500);

            RuleFor(x => x.SequenceNumber).GreaterThanOrEqualTo(0).WithMessage("SequenceNumber must be 0 or greater.");


        }
    }
}
