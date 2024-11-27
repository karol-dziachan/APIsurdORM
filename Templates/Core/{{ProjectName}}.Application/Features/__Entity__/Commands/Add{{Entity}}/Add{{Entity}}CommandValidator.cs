using FluentValidation;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Add__Entity__
{
    internal class Add__Entity__CommandValidator : AbstractValidator<Add__Entity__Command>
    {
        public Add__Entity__CommandValidator()
        {
            RuleFor(x => x.Entity).NotNull().WithMessage("Entity must not be null.");
            // Add more validation rules as needed
        }
    }
}
