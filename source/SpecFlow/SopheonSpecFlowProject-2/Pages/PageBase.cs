using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace SopheonSpecFlowProject2.Pages
{
	public abstract class PageBase
	{
		private static SpecConfig _config = JsonConvert.DeserializeObject<SpecConfig>(File.ReadAllText(".\\SpecConfig.json"));
		public string WebAppUrl => _config.BaseWebAppUrl.StartsWith("&") ? "https://Stratus-Test.Azureedge.net" : _config.BaseWebAppUrl;
		public string B2CLoginUrl => _config.BaseB2CLoginUrl.StartsWith("&") ? "https://StratusB2CTest.b2clogin.com/StratusB2CTest.onmicrosoft.com" : _config.BaseB2CLoginUrl;
		public string MarketingUrl => _config.BaseMarketingUrl.StartsWith("&") ? "https://stratuswebsitetest.z22.web.core.windows.net/" : _config.BaseMarketingUrl;
		public string B2CClientId => _config.B2CClientId.StartsWith("&") ? "cdec0f05-29f9-40f7-b6d3-5873718fea19" : _config.B2CClientId;
	}
}
