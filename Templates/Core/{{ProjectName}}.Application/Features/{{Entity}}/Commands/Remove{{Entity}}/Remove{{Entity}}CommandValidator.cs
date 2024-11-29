using FluentValidation;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Remove__Entity__
{
    public class Remove__Entity__CommandValidator : AbstractValidator<Remove__Entity__Command>
    {
        public Remove__Entity__CommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id must not be empty.");
        }
    }
}
