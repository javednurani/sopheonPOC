using FluentValidation;
using Sopheon.CloudNative.Environments.Functions.Models;

namespace Sopheon.CloudNative.Environments.Functions.Validators
{
   public class EnvironmentDtoValidator : AbstractValidator<EnvironmentDto>
   {
      public EnvironmentDtoValidator()
      {
         RuleFor(e => e.Name).NotEmpty();
         RuleFor(e => e.Owner).NotEmpty();
      }
   }
}
