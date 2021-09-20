using FluentValidation;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Functions.Models;

namespace Sopheon.CloudNative.Environments.Functions.Validators
{
   public class EnvironmentDtoValidator : AbstractValidator<EnvironmentDto>
   {
      public EnvironmentDtoValidator()
      {
         RuleFor(e => e.Name).NotEmpty().MaximumLength(ModelConstraints.NAME_LENGTH);
         RuleFor(e => e.Owner).NotEmpty();
         RuleFor(e => e.Description).MaximumLength(ModelConstraints.DESCRIPTION_LENGTH);
      }
   }
}
