using System;
using System.Collections.Generic;
using System.Text;

namespace Sopheon.Cloud.SpecFlow.Base.Pages
{
	public class SpecConfig
	{
		public string BaseWebAppUrl { get; set; }
		public string BaseB2CLoginUrl { get; set; }
		public string B2CLoginRedirectUrl { get; set; }
		public string BaseMarketingUrl { get; set; }
		public string B2CClientId { get; set; }
	}
}
