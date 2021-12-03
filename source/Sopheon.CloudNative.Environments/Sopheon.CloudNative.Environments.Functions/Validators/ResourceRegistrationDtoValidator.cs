using FluentValidation;
using Sopheon.CloudNative.Environments.Functions.Models;

namespace Sopheon.CloudNative.Environments.Functions.Validators
{
   public class ResourceRegistrationDtoValidator : AbstractValidator<ResourceRegistrationDto>
   {
      public ResourceRegistrationDtoValidator()
      {
         RuleFor(e => e.ResourceTypeId).NotEqual(0);
         RuleFor(e => e.Uri).NotEmpty();
      }
   }
}
