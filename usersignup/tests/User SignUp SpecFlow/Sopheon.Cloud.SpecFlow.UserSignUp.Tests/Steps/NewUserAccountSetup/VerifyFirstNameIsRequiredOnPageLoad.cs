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
	public sealed class VerifyFirstNameIsRequiredOnPageLoad
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
		SignUpPage signUpPage = new SignUpPage();

		public VerifyFirstNameIsRequiredOnPageLoad()
		{
		}

		[Given(@"the user is on the PL Account Setup Page")]
		public void GivenTheUserIsOnThePLAccountSetupPage()
		{
			signUpPage.NavigateSignUpPagePage();
		}

		[When(@"loads the page")]
		public void WhenLoadsThePage()
		{
			signUpPage.signUpNowButtonClick();
		}

		[Then(@"the First Name field is marked as required")]
		public void ThenTheFirstNameFieldIsMarkedAsRequired()
		{
			Assert.That(accountSetUpPage.IsNameExist, Is.True, "Name");
		}
	}
}
