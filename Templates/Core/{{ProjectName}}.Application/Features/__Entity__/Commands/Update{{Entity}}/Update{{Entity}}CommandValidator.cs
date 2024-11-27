using FluentValidation;

namespace __ProjectName__.Application.Features.__Entity__.Commands.Update__Entity__
{
    public class Update__Entity__CommandValidator : AbstractValidator<Update__Entity__Command>
    {
        public Update__Entity__CommandValidator()
        {
            RuleFor(x => x.Entity).NotNull().WithMessage("Entity must not be null.");
            // Add more validation rules specific to your entity here
        }
    }
}
