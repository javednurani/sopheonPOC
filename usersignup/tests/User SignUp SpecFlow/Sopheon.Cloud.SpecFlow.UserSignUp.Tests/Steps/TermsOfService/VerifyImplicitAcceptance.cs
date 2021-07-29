using NUnit.Framework;
using Sopheon.Cloud.SpecFlow.Base.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Sopheon.Cloud.SpecFlow.UserSignUp.Tests.Steps.TermsOfService
{
	[Binding]
	public sealed class VerifyImplicitAcceptance
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
		SignUpPage signUpPage = new SignUpPage();

		public VerifyImplicitAcceptance()
		{
			accountSetUpPage = new AccountSetUpPage();
			signUpPage = new SignUpPage();
		}

		[Given(@"the user is on the TOS page")]
		public void GivenTheUserIsOnTheTOSPage()
		{
			signUpPage.NavigateSignUpPagePage();
			signUpPage.signUpNowButtonClick();
			signUpPage.NavigateSignUpPagePage();
			signUpPage.signUpNowButtonClick();
			accountSetUpPage.nameBoxType();
			accountSetUpPage.emailBoxType();
			accountSetUpPage.newPassWordBoxType();
			accountSetUpPage.reenterPassWordBoxType();
			accountSetUpPage.mobileBoxType();
			accountSetUpPage.clickCreateButton();
		}

		[When(@"the user clicks on Continue")]
		public void WhenTheUserClicksOnContinue()
		{


		}

		[Then(@"the user account is created")]
		public void ThenTheUserAccountIsCreated()
		{

		}

	}
}
