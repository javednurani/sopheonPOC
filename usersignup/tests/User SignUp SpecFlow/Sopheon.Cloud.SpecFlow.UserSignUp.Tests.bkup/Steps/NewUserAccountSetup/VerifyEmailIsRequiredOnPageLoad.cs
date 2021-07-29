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
	public sealed class VerifyEmailIsRequiredOnPageLoad
	{
		SignUpPage signUpPage = new SignUpPage();

		public VerifyEmailIsRequiredOnPageLoad()
		{
		}

		[Then(@"the e-mail field is marked as required")]
		public void ThenTheE_MailFieldIsMarkedAsRequired()
		{
			signUpPage.NavigateSignUpPagePage();
			signUpPage.signInButtonClick();
			Assert.That(signUpPage.isEmailAdressRequireMessageExist, Is.True, "Please enter your email");
		}
	}
}
