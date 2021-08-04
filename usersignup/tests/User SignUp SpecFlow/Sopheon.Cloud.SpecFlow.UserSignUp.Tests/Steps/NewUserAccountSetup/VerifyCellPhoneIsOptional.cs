using Sopheon.Cloud.SpecFlow.Base.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Sopheon.Cloud.SpecFlow.UserSignUp.Tests.Steps.NewUserAccountSetup
{
	[Binding]
	public sealed class VerifyCellPhoneIsOptional
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
		SignUpPage signUpPage = new SignUpPage();

		public VerifyCellPhoneIsOptional()
		{
			accountSetUpPage = new AccountSetUpPage();
			signUpPage = new SignUpPage();
		}

		[Given(@"the user is on the PL Account setup page")]
		public void GivenTheUserIsOnThePLAccountSetupPage()
		{
			signUpPage.NavigateSignUpPagePage();
			signUpPage.signUpNowButtonClick();
		}

		[When(@"the user creates an account without a cell phone")]
		public void WhenTheUserCreatesAnAccountWithoutACellPhone()
		{
			accountSetUpPage.nameBoxType();
			accountSetUpPage.emailBoxType();
			accountSetUpPage.newPassWordBoxType();
			accountSetUpPage.reenterPassWordBoxType();
		}

		[Then(@"the account is created")]
		public void ThenTheAccountIsCreated()
		{
			accountSetUpPage.clickCreateButton();
		}
	}
}
