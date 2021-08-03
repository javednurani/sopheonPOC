import { AccountInfo } from '@azure/msal-browser';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import { DefaultButton, IContextualMenuProps } from '@fluentui/react';
import React, { FunctionComponent, useEffect, useState } from 'react';
import { useIntl } from 'react-intl';

import { azureSettings, getAuthorityUrl } from '../azureSettings';

const LoginSignupButton: FunctionComponent = () => {
  const { formatMessage } = useIntl();
  const { instance, accounts } = useMsal();

  const [account, setAccount] = useState<AccountInfo>();

  useEffect(() => {
    setMsalAccount();
  }, [accounts]);

  // Important to set MSAL account correctly - "accounts" provided by useMsal() hook can include auth responses from initiating any user flow,
  // including profile edit, password change, etc.  We need the account / auth response returned from the SignUpSignIn flow.
  // See MS example code with a version of this function
  // https://github.com/Azure-Samples/ms-identity-b2c-javascript-spa/blob/main/App/authRedirect.js
  const setMsalAccount = () => {
    /**
     * See here for more information on account retrieval:
     * https://github.com/AzureAD/microsoft-authentication-library-for-js/blob/dev/lib/msal-common/docs/Accounts.md
     */

    const currentMsalAccounts = instance.getAllAccounts();

    if (currentMsalAccounts.length < 1) {
      return;
    } else if (currentMsalAccounts.length > 1) {
      /**
       * Due to the way MSAL caches account objects, the auth response from initiating a user-flow
       * is cached as a new account, which results in more than one account in the cache. Here we make
       * sure we are selecting the account with homeAccountId that contains the sign-up/sign-in user-flow,
       * as this is the default flow the user initially signed-in with.
       */
      const msalAccounts = currentMsalAccounts.filter(
        msalAccount =>
          msalAccount.homeAccountId.toUpperCase().includes(azureSettings.AD_B2C_SignUpSignIn_Policy.toUpperCase()) &&
          msalAccount.idTokenClaims !== undefined &&
          // AccountInfo.idTokenClaims is typed as 'object' by Microsoft
          // @ts-ignore
          msalAccount.idTokenClaims.iss.toUpperCase().includes(adB2cAuthorityDomain.toUpperCase()) &&
          // AccountInfo.idTokenClaims is typed as 'object' by Microsoft
          // @ts-ignore
          msalAccount.idTokenClaims.aud === adB2cClientId
      );

      if (msalAccounts.length > 1) {
        // localAccountId identifies the entity for which the token asserts information.
        if (msalAccounts.every(msalAccount => msalAccount.localAccountId === msalAccounts[0].localAccountId)) {
          // All accounts belong to the same user
          setAccount(msalAccounts[0]);
        } else {
          // Multiple users detected. Logout all to be safe.
          instance.logout();
        }
      } else if (msalAccounts.length === 1) {
        setAccount(msalAccounts[0]);
      }
    } else if (currentMsalAccounts.length === 1) {
      setAccount(currentMsalAccounts[0]);
    }
  };

  // TODO, refactor with Cloud-1214 work
  const isDev = process.env.NODE_ENV === 'development';
  const adB2cClientId = isDev ? azureSettings.AD_B2C_ClientId_Dev : azureSettings.AD_B2C_ClientId;
  const adB2cTenantName: string = isDev ? azureSettings.AD_B2C_TenantName_Dev : azureSettings.AD_B2C_TenantName;
  const adB2cAuthorityDomain = `${adB2cTenantName}.b2clogin.com`;

  const changePasswordClick = () => {
    // TODO, this will be refactored once main branch (with Cloud-1214 work) is merged into Cloud-1275 story branch
    // ternary statements will be replaced by logic in azureSettings, and getAuthorityUrl helper function will be used
    const redirectUri: string = isDev ? azureSettings.SPA_Root_URL_Dev : azureSettings.SPA_Root_URL;

    const passwordChangeAuthorityUrl = `https://${adB2cTenantName}.b2clogin.com/${adB2cTenantName}.onmicrosoft.com/${azureSettings.AD_B2C_PasswordChange_Policy}`;

    instance.loginRedirect({
      authority: passwordChangeAuthorityUrl,
      redirectUri: redirectUri,
      scopes: [],
    });
  };
  const editProfileClick = () => {
    // Cloud-1339: The below ts-ignore is due to not including a 'scopes' property in the RedirectRequest object
    // The linked example code from Microsoft, demo'ing a loginRedirect Profile Edit, does not include 'scopes' on the RedirectRequest
    // https://github.com/Azure-Samples/ms-identity-b2c-javascript-spa/blob/main/App/authRedirect.js
    // @ts-ignore
    instance.loginRedirect({
      authority: getAuthorityUrl(azureSettings.AD_B2C_ProfileEdit_Policy),
      redirectUri: azureSettings.SPA_Root_URL,
    });
  };

  const menuProps: IContextualMenuProps = {
    items: [
      {
        key: 'profile',
        text: formatMessage({ id: 'auth.myprofile' }),
        iconProps: { iconName: 'EditContact' },
        onClick: editProfileClick,
      },
      {
        key: 'changepassword',
        text: formatMessage({ id: 'auth.changepassword' }),
        iconProps: { iconName: 'Permissions' },
        onClick: changePasswordClick,
      },
      {
        key: 'signout',
        text: formatMessage({ id: 'auth.signout' }),
        iconProps: { iconName: 'SignOut' },
      },
    ],
  };

  return (
    <React.Fragment>
      <AuthenticatedTemplate>
        <DefaultButton text={account ? account.name : formatMessage({ id: 'auth.myprofile' })} split menuProps={menuProps} />
      </AuthenticatedTemplate>
      <UnauthenticatedTemplate>
        <DefaultButton text={formatMessage({ id: 'auth.loginbutton' })} onClick={() => instance.loginRedirect()} />
      </UnauthenticatedTemplate>
    </React.Fragment>
  );
};

export default LoginSignupButton;
