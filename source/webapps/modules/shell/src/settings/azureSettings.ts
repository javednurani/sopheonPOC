import { isDev } from './environmentSettings';

// TODO, some rawSettings are not assigned per environment (eg, AD_B2C_SignUp_Policy), can we consolidate/ eliminate pass through?
// these raw settings allow us to defined multiple, environment-specific versions of a setting
const azureRawSettings: Record<string, string> = {
  AD_B2C_ClientId: '^B2CShellAppClientId^',
  AD_B2C_ClientId_Dev: '8bdfb9a7-913a-48a8-9fe0-5b2877fb844d',
  AD_B2C_TenantName: '^B2CTenantLoginName^',
  AD_B2C_TenantName_Dev: 'StratusB2CDev',
  AD_B2C_SignUpSignIn_Policy: 'B2C_1A_SIGNUP_SIGNIN',
  AD_B2C_SignUp_Policy: 'B2C_1A_SIGNUP',
  AD_B2C_PasswordChange_Policy: 'B2C_1A_PROFILEEDIT_PASSWORDCHANGE',
  AD_B2C_ProfileEdit_Policy: 'B2C_1A_PROFILEEDIT',
  SPA_Root_URL: 'https://^BrowserWebAppUrl^.azureedge.net/product',
  SPA_Root_URL_Dev: 'https://localhost:3000/product',
  // TODO Cloud-2259, query Graph API for App Reg "PMAPI"
  // https://docs.microsoft.com/en-us/graph/api/application-list?view=graph-rest-1.0&tabs=http#http-request
  Product_Management_API_Application_Client_Id: '^B2CProductManagementApiClientId^',
  Product_Management_API_Application_Client_Id_Dev: 'd7c97f69-2f27-43a0-b998-c659ab05d8ba',
  Product_Management_API_Scope_PMCore_ReadWrite: 'PMCore.ReadWrite'
};

// these collapsed settings incorporate the current environment to assign the relevant raw setting
export const azureSettings: Record<string, string> = {
  AD_B2C_ClientId: isDev ? azureRawSettings.AD_B2C_ClientId_Dev : azureRawSettings.AD_B2C_ClientId,
  AD_B2C_TenantName: isDev ? azureRawSettings.AD_B2C_TenantName_Dev : azureRawSettings.AD_B2C_TenantName,
  AD_B2C_SignUpSignIn_Policy: azureRawSettings.AD_B2C_SignUpSignIn_Policy,
  AD_B2C_SignUp_Policy: azureRawSettings.AD_B2C_SignUp_Policy,
  AD_B2C_PasswordChange_Policy: azureRawSettings.AD_B2C_PasswordChange_Policy,
  AD_B2C_ProfileEdit_Policy: azureRawSettings.AD_B2C_ProfileEdit_Policy,
  SPA_Root_URL: isDev ? azureRawSettings.SPA_Root_URL_Dev : azureRawSettings.SPA_Root_URL,
  Product_Management_API_Application_Client_Id: isDev
    ? azureRawSettings.Product_Management_API_Application_Client_Id_Dev
    : azureRawSettings.Product_Management_API_Application_Client_Id,
  Product_Management_API_Scope_PMCore_ReadWrite: azureRawSettings.Product_Management_API_Scope_PMCore_ReadWrite,
};

export function getAuthorityUrl(adB2cPolicyName: string): string {
  return `https://${azureSettings.AD_B2C_TenantName}.b2clogin.com/${azureSettings.AD_B2C_TenantName}.onmicrosoft.com/${adB2cPolicyName}`;
}

export function getAuthorityDomain(): string {
  return `${azureSettings.AD_B2C_TenantName}.b2clogin.com`;
}

// returns https://StratusB2CDev.onmicrosoft.com/d7c97f69-2f27-43a0-b998-c659ab05d8ba
// see ProductManagementApi "Application ID URI" on Azure, this helper may not be necessary
export function getProductManagementApiApplicationIdUri() {
  return `https://${azureSettings.AD_B2C_TenantName}.onmicrosoft.com/${azureSettings.Product_Management_API_Application_Client_Id}`;
}

// returns https://StratusB2CDev.onmicrosoft.com/d7c97f69-2f27-43a0-b998-c659ab05d8ba/PMCore.ReadWrite
// needed in AADB2C Requests to request Private WebAPI Scope(s)
export function getProductManagementApiScopeCoreReadWrite() {
  return `${getProductManagementApiApplicationIdUri()}/${azureSettings.Product_Management_API_Scope_PMCore_ReadWrite}`;
}
