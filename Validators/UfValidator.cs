using FluentValidation;

namespace EMixApi.Validators
{
    public class UfValidator : AbstractValidator<string>
    {
        public UfValidator() 
        {
            RuleFor(x => x)
                .Length(2).WithMessage("UF deve conter apenas dois caracteres.")
                .Matches(@"^[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ ]+$").WithMessage("UF deve conter apenas letras.");
        }
    }
}
