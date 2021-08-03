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
      const adB2cAuthorityDomain = `${azureSettings.AD_B2C_TenantName}.b2clogin.com`;

      const msalAccounts = currentMsalAccounts.filter(
        msalAccount =>
          msalAccount.homeAccountId.toUpperCase().includes(azureSettings.AD_B2C_SignUpSignIn_Policy.toUpperCase()) &&
          msalAccount.idTokenClaims !== undefined &&
          // AccountInfo.idTokenClaims is typed as 'object' by Microsoft
          // @ts-ignore
          msalAccount.idTokenClaims.iss.toUpperCase().includes(adB2cAuthorityDomain.toUpperCase()) &&
          // AccountInfo.idTokenClaims is typed as 'object' by Microsoft
          // @ts-ignore
          msalAccount.idTokenClaims.aud === azureSettings.AD_B2C_ClientId
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

  const changePasswordClick = () => {
    instance.loginRedirect({
      authority: getAuthorityUrl(azureSettings.AD_B2C_PasswordChange_Policy),
      redirectUri: azureSettings.SPA_Root_URL,
      scopes: [],
    });
  };

  const editProfileClick = () => {
    instance.loginRedirect({
      authority: getAuthorityUrl(azureSettings.AD_B2C_ProfileEdit_Policy),
      redirectUri: azureSettings.SPA_Root_URL,
      scopes: [],
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
