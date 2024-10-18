using FluentValidation;

namespace EMixApi.Validators
{
    public class CepValidator : AbstractValidator<string>
    {
        public CepValidator() 
        {
            RuleFor(x => x)
                .NotEmpty().WithMessage("Digite o CEP.")
                .Matches(@"^(\d{8}|(\d{5}-\d{3}))*$").WithMessage("CEP é inválido.");
        }
    }
}
