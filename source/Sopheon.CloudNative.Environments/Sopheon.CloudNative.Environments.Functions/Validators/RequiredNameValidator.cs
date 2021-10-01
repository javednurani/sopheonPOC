using FluentValidation;
using Sopheon.CloudNative.Environments.Domain;

namespace Sopheon.CloudNative.Environments.Functions.Validators
{
   public class RequiredNameValidator : AbstractValidator<string>, IRequiredNameValidator
   {
      public RequiredNameValidator()
      {
         RuleFor(str => str).NotEmpty();
         RuleFor(str => str).MaximumLength(ModelConstraints.NAME_LENGTH);
      }
   }

   public interface IRequiredNameValidator : IValidator<string>
   {

   }
}
