const azureSettings: Record<string, string> = {
  AD_B2C_ClientId: '&ShellAppClientId&',
  AD_B2C_ClientId_Dev: '8bdfb9a7-913a-48a8-9fe0-5b2877fb844d',
  AD_B2C_TenantName: '&B2CTenantName&',
  AD_B2C_TenantName_Dev: 'StratusB2CDev',
  SPA_Root_URL: 'https://&BrowserWebAppUrl&.azureedge.net/',
  SPA_Root_URL_Dev: 'https://localhost:3000/',
};

export default azureSettings;
