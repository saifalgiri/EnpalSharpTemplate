using EnpalSharpTemplate.Model;
using FluentValidation;

namespace EnpalSharpTemplate.Validators
{
    public class IdentityServerConfigValidator : AbstractValidator<IdentityServerConfig>
    {
        public IdentityServerConfigValidator()
        {
            RuleFor(model => model.Audience)
                .NotEmpty()
                .WithMessage("Audience must be given!");
            RuleFor(model => model.BaseUrl)
                .NotEmpty()
                .WithMessage("Base url of identity Server must be given");
        }
    }
}
