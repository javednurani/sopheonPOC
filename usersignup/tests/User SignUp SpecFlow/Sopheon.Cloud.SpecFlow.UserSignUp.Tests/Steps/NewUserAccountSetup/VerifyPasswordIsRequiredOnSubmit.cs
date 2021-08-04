using NUnit.Framework;
using Sopheon.Cloud.SpecFlow.Base.Hooks;
using Sopheon.Cloud.SpecFlow.Base.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Sopheon.Cloud.SpecFlow.UserSignUp.Tests.Steps.NewUserAccountSetup
{
	[Binding]
	public sealed class VerifyPasswordIsRequiredOnSubmit
	{
		AccountSetUpPage accountSetUpPage = new AccountSetUpPage();

		public VerifyPasswordIsRequiredOnSubmit()
		{
		}

		[When(@"the user clicks submit with a blank Password Field")]
		public void WhenTheUserClicksSubmitWithABlankPasswordField()
		{
			accountSetUpPage.clickCreateButton();
			Assert.IsTrue(BaseHook.driver.Url.Contains("https://jwt.ms"));
		}
	}
}
