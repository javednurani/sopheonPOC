using System;
using FluentValidation.TestHelper;
using Sopheon.CloudNative.Environments.Functions.Models;
using Sopheon.CloudNative.Environments.Functions.Validators;
using Sopheon.CloudNative.Environments.Testing.Common;
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
            Owner = Guid.Empty
         };

         TestValidationResult<EnvironmentDto> result = _sut.TestValidate(env);

         result.ShouldHaveValidationErrorFor(x => x.Name);
         result.ShouldHaveValidationErrorFor(x => x.Owner);

         result.ShouldNotHaveValidationErrorFor(x => x.Description);
      }

      [Fact]
      public void FieldsTooLong_ReturnsCorrectValidationErrors()
      {
         EnvironmentDto env = new EnvironmentDto
         {
            Name = Some.Random.String(65),
            Description = Some.Random.String(1025),  
            Owner = Some.Random.Guid()
         };

         TestValidationResult<EnvironmentDto> result = _sut.TestValidate(env);

         result.ShouldHaveValidationErrorFor(x => x.Name);
         result.ShouldHaveValidationErrorFor(x => x.Description);
      }

      [Fact]
      public void ValidEnvironment_Success()
      {
         EnvironmentDto env = new EnvironmentDto
         {
            Name = Some.Random.String(),
            Description = Some.Random.String(),
            Owner = Some.Random.Guid()
         };

         TestValidationResult<EnvironmentDto> result = _sut.TestValidate(env);

         result.ShouldNotHaveAnyValidationErrors();
      }
   }
}
