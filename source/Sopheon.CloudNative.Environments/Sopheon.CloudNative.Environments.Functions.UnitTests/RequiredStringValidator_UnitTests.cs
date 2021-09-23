using FluentValidation.TestHelper;
using Sopheon.CloudNative.Environments.Functions.Validators;
using Sopheon.CloudNative.Environments.Testing.Common;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class RequiredStringValidator_UnitTests
   {
      private RequiredStringValidator _sut = new RequiredStringValidator();

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
      public void ValidEnvironment_Success()
      {
         string requiredString = Some.Random.String();

         TestValidationResult<string> result = _sut.TestValidate(requiredString);

         result.ShouldNotHaveAnyValidationErrors();
      }
   }
}
