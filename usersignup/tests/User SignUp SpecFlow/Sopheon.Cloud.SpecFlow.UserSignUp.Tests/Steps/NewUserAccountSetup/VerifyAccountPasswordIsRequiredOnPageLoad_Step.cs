using NUnit.Framework;
using Sopheon.Cloud.SpecFlow.Base.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Sopheon.Cloud.SpecFlow.UserSignUp.Tests.Steps.NewUserAccountSetup
{
	[Binding]
	public sealed class VerifyAccountPasswordIsRequiredOnPageLoad_Step
	{
		SignUpPage signUpPage = new SignUpPage();

		public VerifyAccountPasswordIsRequiredOnPageLoad_Step()
		{
			signUpPage = new SignUpPage();
		}

		[Then(@"the Account Password field is marked as required")]
		public void ThenTheAccountPasswordFieldIsMarkedAsRequired()
		{
			signUpPage.NavigateSignUpPagePage();
			signUpPage.nameBoxType();
			signUpPage.signInButtonClick();
			Assert.That(signUpPage.isPassWordRequireMessageExist, Is.True, "Please enter your password");
		}
	}
}
