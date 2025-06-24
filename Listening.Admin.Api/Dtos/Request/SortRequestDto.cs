using FluentValidation;

namespace Listening.Admin.Api.Dtos.Request
{
    public class SortRequestDto
    {
        public List<long> Ids { get; set; }
    }
    public class SortRequestDtoValidator : AbstractValidator<SortRequestDto>
    {
        public SortRequestDtoValidator()
        {

            RuleFor(x => x.Ids).Must(x => x != null && x.Count > 0)
                .WithMessage("Ids is required.")
                .WithErrorCode("SortRequestDto.IdsRequired");


        }
    }
}
