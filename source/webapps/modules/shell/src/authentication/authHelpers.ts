import { AccountInfo, Configuration, IPublicClientApplication, PublicClientApplication, RedirectRequest } from '@azure/msal-browser';

import { azureSettings, getAuthorityDomain, getAuthorityUrl, getProductManagementApiScopeCoreReadWrite } from '../settings/azureSettings';

const OIDC_SCOPES = ['openid', 'offline_access'];
const PRODUCT_MANAGEMENT_SCOPES = [getProductManagementApiScopeCoreReadWrite()];

export const ALL_SCOPES = OIDC_SCOPES.concat(PRODUCT_MANAGEMENT_SCOPES);

export const msalInstance = (): PublicClientApplication => {
  const msalConfig: Configuration = {
    auth: {
      authority: getAuthorityUrl(azureSettings.AD_B2C_SignUpSignIn_Policy),
      clientId: azureSettings.AD_B2C_ClientId,
      knownAuthorities: [getAuthorityDomain()],
    },
  };

  const pca = new PublicClientApplication(msalConfig);
  return pca;
};

export const getAccessToken: () => Promise<string> = async () => {
  const FAILURE_MESSAGE = 'shell::authHelpers::getAccessToken FAILURE';
  try {
    // outside of the component tree / React Context, create a new PublicClientApplication (with same config options) to access MSAL
    const pca = msalInstance();
    const account = getMsalAccount(pca);
    if (account) {
      const acquireTokenResponse = await pca.acquireTokenSilent({
        scopes: PRODUCT_MANAGEMENT_SCOPES,
        account: account
      });

      return acquireTokenResponse.accessToken || FAILURE_MESSAGE;
    }
    return FAILURE_MESSAGE;
  } catch (error) {
    console.log(error);
    return FAILURE_MESSAGE;
  }
};

export const loginButtonRequest: RedirectRequest = {
  scopes: ALL_SCOPES,
  redirectUri: azureSettings.SPA_Root_URL,
  redirectStartPage: azureSettings.SPA_Root_URL,
};

export const changePasswordRequest: RedirectRequest = {
  authority: getAuthorityUrl(azureSettings.AD_B2C_PasswordChange_Policy),
  redirectUri: azureSettings.SPA_Root_URL,
  scopes: [],
};

export const editProfileRequest: RedirectRequest = {
  authority: getAuthorityUrl(azureSettings.AD_B2C_ProfileEdit_Policy),
  redirectUri: azureSettings.SPA_Root_URL,
  scopes: [],
};

export const getAuthLandingRedirectRequest = (adB2cPolicyName: string): RedirectRequest => ({
  authority: getAuthorityUrl(adB2cPolicyName),
  scopes: ALL_SCOPES,
  redirectUri: azureSettings.SPA_Root_URL,
  redirectStartPage: azureSettings.SPA_Root_URL,
  // extraQueryParameters object can be used to pass in values that are resolved as custom policy claims
  // https://docs.microsoft.com/en-us/azure/active-directory-b2c/claim-resolver-overview#oauth2-key-value-parameters
  extraQueryParameters: undefined
});

// Important to set MSAL account correctly - "accounts" provided by useMsal() hook can include auth responses from initiating any user flow,
// including profile edit, password change, etc.  We need the account / auth response returned from the SignUpSignIn flow.
// See MS example code with a version of this function
// https://github.com/Azure-Samples/ms-identity-b2c-javascript-spa/blob/main/App/authRedirect.js
export const getMsalAccount = (instance: IPublicClientApplication): AccountInfo | undefined => {
  /**
   * See here for more information on account retrieval:
   * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-common/docs/Accounts.md
   */

  const currentMsalAccounts = instance.getAllAccounts();

  if (currentMsalAccounts.length < 1) {
    return undefined;
  } else if (currentMsalAccounts.length > 1) {
    /**
     * Due to the way MSAL caches account objects, the auth response from initiating a user-flow
     * is cached as a new account, which results in more than one account in the cache. Here we make
     * sure we are selecting the account with homeAccountId that contains the sign-up/sign-in user-flow,
     * as this is the default flow the user initially signed-in with.
     */

    const msalAccounts = currentMsalAccounts.filter(
      msalAccount =>
        // Either of these policies can result in valid Account responses
        (msalAccount.homeAccountId.toUpperCase().includes(azureSettings.AD_B2C_SignUpSignIn_Policy.toUpperCase()) ||
          msalAccount.homeAccountId.toUpperCase().includes(azureSettings.AD_B2C_SignUp_Policy.toUpperCase())) &&

        msalAccount.idTokenClaims !== undefined &&
        // AccountInfo.idTokenClaims is typed as 'object' by Microsoft
        // @ts-ignore
        msalAccount.idTokenClaims.iss.toUpperCase().includes(getAuthorityDomain().toUpperCase()) &&
        // AccountInfo.idTokenClaims is typed as 'object' by Microsoft
        // @ts-ignore
        msalAccount.idTokenClaims.aud === azureSettings.AD_B2C_ClientId
    );

    if (msalAccounts.length > 1) {
      // localAccountId identifies the entity for which the token asserts information.
      if (msalAccounts.every(msalAccount => msalAccount.localAccountId === msalAccounts[0].localAccountId)) {
        // All accounts belong to the same user
        return msalAccounts[0];
      }
      // Multiple users detected. Logout all to be safe.
      // end of function / return undefined will be reached
      instance.logout();
    } else if (msalAccounts.length === 1) {
      return msalAccounts[0];
    }
  } else if (currentMsalAccounts.length === 1) {
    return currentMsalAccounts[0];
  }
  // case of multiple users detected will fall here after instance.logout()
  // case of invalid currentMsalAccounts returned from instance.getAllAccounts() will also fall here
  return undefined;
};
