import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import { DefaultButton, IContextualMenuProps } from '@fluentui/react';
import React, { FunctionComponent } from 'react';
import { useIntl } from 'react-intl';

import { azureSettings, getAuthorityUrl } from '../azureSettings';

const LoginSignupButton: FunctionComponent = () => {
  const { formatMessage } = useIntl();
  const { instance, accounts } = useMsal();

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
