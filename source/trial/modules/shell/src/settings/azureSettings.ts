import { isDev } from './environmentSettings';

const azureRawSettings: Record<string, string> = {
  AD_B2C_ClientId: '^ShellAppClientId^',
  AD_B2C_ClientId_Dev: '8bdfb9a7-913a-48a8-9fe0-5b2877fb844d',
  AD_B2C_TenantName: '^B2CTenantName^',
  AD_B2C_TenantName_Dev: 'StratusB2CDev',
  AD_B2C_SignUpSignIn_Policy: 'B2C_1A_SIGNUP_SIGNIN',
  AD_B2C_SignUp_Policy: 'B2C_1A_SIGNUP',
  AD_B2C_PasswordChange_Policy: 'B2C_1A_PROFILEEDIT_PASSWORDCHANGE',
  AD_B2C_ProfileEdit_Policy: 'B2C_1A_PROFILEEDIT',
  SPA_Root_URL: 'https://^BrowserWebAppUrl^.azureedge.net/',
  SPA_Root_URL_Dev: 'https://localhost:3000/',
};

// these collapsed settings incorporate the current environment
export const azureSettings: Record<string, string> = {
  AD_B2C_ClientId: isDev ? azureRawSettings.AD_B2C_ClientId_Dev : azureRawSettings.AD_B2C_ClientId,
  AD_B2C_TenantName: isDev ? azureRawSettings.AD_B2C_TenantName_Dev : azureRawSettings.AD_B2C_TenantName,
  AD_B2C_SignUpSignIn_Policy: azureRawSettings.AD_B2C_SignUpSignIn_Policy,
  AD_B2C_SignUp_Policy: azureRawSettings.AD_B2C_SignUp_Policy,
  AD_B2C_PasswordChange_Policy: azureRawSettings.AD_B2C_PasswordChange_Policy,
  AD_B2C_ProfileEdit_Policy: azureRawSettings.AD_B2C_ProfileEdit_Policy,
  SPA_Root_URL: isDev ? azureRawSettings.SPA_Root_URL_Dev : azureRawSettings.SPA_Root_URL,
};

export function getAuthorityUrl(adB2cPolicyName: string): string {
  return `https://${azureSettings.AD_B2C_TenantName}.b2clogin.com/${azureSettings.AD_B2C_TenantName}.onmicrosoft.com/${adB2cPolicyName}`;
}

export function getAuthorityDomain(): string {
  return `${azureSettings.AD_B2C_TenantName}.b2clogin.com`;
}
