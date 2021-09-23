using FluentValidation;
using Sopheon.CloudNative.Environments.Domain;

namespace Sopheon.CloudNative.Environments.Functions.Validators
{
   public class RequiredStringValidator : AbstractValidator<string>, IRequiredStringValidator
   {
      public RequiredStringValidator()
      {
         RuleFor(str => str).NotEmpty();
      }
   }

   public interface IRequiredStringValidator : IValidator<string>
   {

   }
}
