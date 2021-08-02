import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import { DefaultButton, IContextualMenuProps } from '@fluentui/react';
import React, { FunctionComponent } from 'react';
import { useIntl } from 'react-intl';

import azureSettings from '../azureSettings';

const LoginSignupButton: FunctionComponent = () => {
  const { formatMessage } = useIntl();
  const { instance, accounts } = useMsal();

  const changePasswordClick = () => {
    // TODO, this will be refactored once main branch (with Cloud-1214 work) is merged into Cloud-1275 story branch
    // ternary statements will be replaced by logic in azureSettings, and getAuthorityUrl helper function will be used
    const isDev = process.env.NODE_ENV === 'development';
    const redirectUri: string = isDev ? azureSettings.SPA_Root_URL_Dev : azureSettings.SPA_Root_URL;

    const adB2cTenantName: string = isDev ? azureSettings.AD_B2C_TenantName_Dev : azureSettings.AD_B2C_TenantName;
    const passwordChangeAuthorityUrl = `https://${adB2cTenantName}.b2clogin.com/${adB2cTenantName}.onmicrosoft.com/${azureSettings.AD_B2C_PasswordChange_Policy}`;

    // Cloud-1339: The below ts-ignore is due to not including a 'scopes' property in the RedirectRequest object
    // The linked example code from Microsoft, demo'ing a loginRedirect Profile Edit, does not include 'scopes' on the RedirectRequest
    // This call to Password Change is similar, in that a loginRedirect call is used to access a non-signupsignin flow
    // https://github.com/Azure-Samples/ms-identity-b2c-javascript-spa/blob/main/App/authRedirect.js
    // @ts-ignore
    instance.loginRedirect({
      authority: passwordChangeAuthorityUrl,
      redirectUri: redirectUri,
    });
  };

  const menuProps: IContextualMenuProps = {
    items: [
      {
        key: 'profile',
        text: formatMessage({ id: 'auth.myprofile' }),
        iconProps: { iconName: 'EditContact' },
      },
      {
        key: 'changepassword',
        // TODO-1214 LOCALIZE
        text: formatMessage({ id: 'auth.changepassword' }),
        iconProps: { iconName: 'EditContact' },
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
        <DefaultButton text={accounts[0] ? accounts[0].name : formatMessage({ id: 'auth.myprofile' })} split menuProps={menuProps} />
      </AuthenticatedTemplate>
      <UnauthenticatedTemplate>
        <DefaultButton text={formatMessage({ id: 'auth.loginbutton' })} onClick={() => instance.loginRedirect()} />
      </UnauthenticatedTemplate>
    </React.Fragment>
  );
};

export default LoginSignupButton;
