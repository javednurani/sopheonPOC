using System;
using FluentValidation.TestHelper;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.UnitTests.TestHelpers;
using Sopheon.CloudNative.Environments.Functions.Validators;
using Xunit;

namespace Sopheon.CloudNative.Environments.Functions.UnitTests
{
   public class EnvironmentDtoValidator_UnitTests
   {
      private EnvironmentDtoValidator _sut = new EnvironmentDtoValidator();

      [Theory]
      [InlineData(null, null)]
      [InlineData("", "")]
      [InlineData(" ", " ")]
      public void FieldsEmpty_ReturnsCorrectValidationErrors(string name, string description)
      {
         EnvironmentDto env = new EnvironmentDto
         {
            Name = name,
            Description = description,
            Owner = default(Guid)
         };

         TestValidationResult<EnvironmentDto> result = _sut.TestValidate(env);

         result.ShouldHaveValidationErrorFor(x => x.Name);
         result.ShouldHaveValidationErrorFor(x => x.Owner);

         result.ShouldNotHaveValidationErrorFor(x => x.Description);
      }

      [Fact]
      public void ValidEnvironment_Success()
      {
         EnvironmentDto env = new EnvironmentDto
         {
            Name = SomeRandom.String(),
            Description = SomeRandom.String(),
            Owner = SomeRandom.Guid()
         };

         TestValidationResult<EnvironmentDto> result = _sut.TestValidate(env);

         result.ShouldNotHaveAnyValidationErrors();
      }
   }
}
