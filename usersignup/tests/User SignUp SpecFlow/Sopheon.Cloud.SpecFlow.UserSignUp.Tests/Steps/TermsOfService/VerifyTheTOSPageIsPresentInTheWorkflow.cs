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
	public sealed class VerifyTheTOSPageIsPresentInTheWorkflow
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
		SignUpPage signUpPage = new SignUpPage();
		TermsOfServicePage termsOfServicePage = new TermsOfServicePage();

		public VerifyTheTOSPageIsPresentInTheWorkflow()
		{
			accountSetUpPage = new AccountSetUpPage();
			signUpPage = new SignUpPage();
			termsOfServicePage = new TermsOfServicePage();
		}

		[Given(@"the user is in the New Account Sign Up workflow")]
		public void GivenTheUserIsInTheNewAccountSignUpWorkflow()
		{
			signUpPage.NavigateSignUpPagePage();
			signUpPage.signUpNowButtonClick();
			signUpPage.NavigateSignUpPagePage();
			signUpPage.signUpNowButtonClick();
		}

		[When(@"the user clicks on Create")]
		public void WhenTheUserClicksOnCreate()
		{
			accountSetUpPage.nameBox2Type();
			accountSetUpPage.emailBox2Type();
			accountSetUpPage.newPassWord2BoxType();
			accountSetUpPage.reenterPassWordBox2Type();
			accountSetUpPage.mobileBox2Type();
			accountSetUpPage.clickCreateButton();
		}

		[Then(@"the user is taken to the TOS page")]
		public void ThenTheUserIsTakenToTheTOSPage()
		{
			Assert.That(termsOfServicePage.isTOSMassegeExist, Is.True, "Accept the Terms of Service to keep going");
		}

	}
}
