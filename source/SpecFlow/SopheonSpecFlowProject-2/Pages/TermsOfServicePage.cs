using OpenQA.Selenium;
using SpecFlowProject2.Hooks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SopheonSpecFlowProject2.Pages
{
    public class TermsOfServicePage : PageBase
    {
        public static string url = "https://stratusqa.b2clogin.com/StratusQA.onmicrosoft.com/B2C_1A_signup_signin/api/SelfAsserted/confirmed?csrf_token=THpHdXM3Y0lzOWJyaVUxTlNhMy9XM2RnUzlhdmhOYlRCSGtjb0JzSk1OTTlNeHNuYmpGMUV5U3hyQm5qV3lBQlMvSTdZV0xXNjJZc1dmTHBOYzJ4aWc9PTsyMDIxLTA3LTEzVDAwOjA0OjUwLjczNjg0Mlo7K0gwQldwcGdXZkJlZExHTjFUeEpJQT09O3siVGFyZ2V0RW50aXR5IjoiU2lnblVwV2l0aExvZ29uRW1haWxFeGNoYW5nZSIsIk9yY2hlc3RyYXRpb25TdGVwIjoyfQ==&tx=StateProperties=eyJUSUQiOiJlMDEyZDcxMS04MjFjLTQ1OTgtYjZlOC03Njc0ZmUzNTkzYzkifQ&p=B2C_1A_signup_signin&diags=%7B%22pageViewId%22%3A%2280ce6341-c42d-484d-85c2-184afb413915%22%2C%22pageId%22%3A%22SelfAsserted%22%2C%22trace%22%3A%5B%7B%22ac%22%3A%22T005%22%2C%22acST%22%3A1626134692%2C%22acD%22%3A2%7D%2C%7B%22ac%22%3A%22T021%20-%20URL%3Ahttps%3A%2F%2Fstratusqa.b2clogin.com%2Fstatic%2Ftenant%2Ftemplates%2FAzureBlue%2FselfAsserted.cshtml%3Fslice%3D001-000%26dc%3DCHI%22%2C%22acST%22%3A1626134692%2C%22acD%22%3A99%7D%2C%7B%22ac%22%3A%22T029%22%2C%22acST%22%3A1626134692%2C%22acD%22%3A6%7D%2C%7B%22ac%22%3A%22T004%22%2C%22acST%22%3A1626134692%2C%22acD%22%3A1%7D%2C%7B%22ac%22%3A%22T019%22%2C%22acST%22%3A1626134692%2C%22acD%22%3A11%7D%2C%7B%22ac%22%3A%22T003%22%2C%22acST%22%3A1626134692%2C%22acD%22%3A1%7D%2C%7B%22ac%22%3A%22T017T010%22%2C%22acST%22%3A1626138057%2C%22acD%22%3A2826%7D%2C%7B%22ac%22%3A%22T002%22%2C%22acST%22%3A0%2C%22acD%22%3A0%7D%5D%7D";
        IWebDriver driver;

        public TermsOfServicePage()
        {
            driver = Hook.driver;
        }

        public void NavigateAccountSetUpPage() => driver.Navigate().GoToUrl(url);
    }
}
