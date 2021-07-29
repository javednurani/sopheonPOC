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
	public sealed class VerifyEmailAddressIsRequiredOnSubmit
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();

		public VerifyEmailAddressIsRequiredOnSubmit()
		{
		}

		[When(@"the user clicks submit with a blank E-mail Address Field")]
		public void WhenTheUserClicksSubmitWithABlankE_MailAddressField()
		{
			accountSetUpPage.clickCreateButton();
		}
	}
}
