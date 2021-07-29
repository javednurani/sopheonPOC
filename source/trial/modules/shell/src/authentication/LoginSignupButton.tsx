import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import { DefaultButton, IContextualMenuProps } from '@fluentui/react';
import React, { FunctionComponent } from 'react';
import { useIntl } from 'react-intl';

import azureSettings from '../azureSettings';

const LoginSignupButton: FunctionComponent = () => {
  const { formatMessage } = useIntl();
  const { instance, accounts } = useMsal();

  const editProfileClick = () => {
    const isDev = process.env.NODE_ENV === 'development';
    const redirectUri: string = isDev ? azureSettings.SPA_Root_URL_Dev : azureSettings.SPA_Root_URL;
    const adB2cTenantName: string = isDev ? azureSettings.AD_B2C_TenantName_Dev : azureSettings.AD_B2C_TenantName;
    const profileEditAuthorityUrl = `https://${adB2cTenantName}.b2clogin.com/${adB2cTenantName}.onmicrosoft.com/${azureSettings.AD_B2C_ProfileEdit_Policy}`;
    instance.loginRedirect({
      authority: profileEditAuthorityUrl,
      redirectUri: redirectUri,
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
