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
	public sealed class VerifyFirstNameIsRequiredOnSubmit
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();
		SignUpPage signUpPage = new SignUpPage();

		public VerifyFirstNameIsRequiredOnSubmit()
		{
		}

		[Given(@"the user is on the PL Account Sign Up page")]
		public void GivenTheUserIsOnThePLAccountSignUpPage()
		{
			signUpPage.NavigateSignUpPagePage();
			signUpPage.signUpNowButtonClick();
		}

		[When(@"the user clicks submit with a blank First Name Field")]
		public void WhenTheUserClicksSubmitWithABlankFirstNameField()
		{
			accountSetUpPage.clickCreateButton();
		}

		[Then(@"the user is not taken to new account page")]
		public void ThenTheUserIsNotTakenToNewAccountPage()
		{
			Assert.AreEqual("A required field is missing. Please fill out all required fields and try again.", accountSetUpPage.errorMessageText);
		}
	}
}
