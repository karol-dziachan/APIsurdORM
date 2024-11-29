using FluentValidation;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entities__ByParameters
{
    public class Remove__Entities__ByParametersCommandValidator : AbstractValidator<Remove__Entities__ByParametersCommand>
    {
        public Remove__Entities__ByParametersCommandValidator()
        {
            RuleFor(x => x.Parameters).NotEmpty().WithMessage("Parameters must not be empty.");
            // Add more validation rules specific to your entity here
        }
    }
}
