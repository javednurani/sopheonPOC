using FluentValidation.TestHelper;
using Sopheon.CloudNative.Environments.Domain;
using Sopheon.CloudNative.Environments.Functions.Validators;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests.Validators
{
   public class RequiredNameValidator_UnitTests
   {
      private RequiredNameValidator _sut = new RequiredNameValidator();

      [Theory]
      [InlineData("")]
      [InlineData(" ")]
      [InlineData("  ")]
      public void FieldsEmpty_ReturnsCorrectValidationErrors(string requiredString)
      {
         TestValidationResult<string> result = _sut.TestValidate(requiredString);
         result.ShouldHaveValidationErrorFor(x => x);
      }

      [Fact]
      public void ValidName_Success()
      {
         string requiredString = Some.Random.String();

         TestValidationResult<string> result = _sut.TestValidate(requiredString);

         result.ShouldNotHaveAnyValidationErrors();
      }

      [Fact]
      public void NameTooLong_ReturnsCorrectValidationError()
      {
         string longName = Some.Random.String(ModelConstraints.NAME_LENGTH + 1);

         TestValidationResult<string> result = _sut.TestValidate(longName);

         result.ShouldHaveValidationErrorFor(x => x);
      }
   }
}
