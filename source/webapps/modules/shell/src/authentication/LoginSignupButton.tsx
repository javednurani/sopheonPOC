import { AccountInfo } from '@azure/msal-browser';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import { DefaultButton, IContextualMenuProps } from '@fluentui/react';
import React, { FunctionComponent, useEffect, useState } from 'react';
import { useIntl } from 'react-intl';

import { changePasswordRequest, editProfileRequest, getMsalAccount, loginButtonRequest } from './authHelpers';

const LoginSignupButton: FunctionComponent = () => {
  const { formatMessage } = useIntl();
  const { instance, accounts } = useMsal();

  const [account, setAccount] = useState<AccountInfo>();

  useEffect(() => {
    const msalAccount: AccountInfo | undefined = getMsalAccount(instance);
    if (msalAccount !== undefined) {
      setAccount(msalAccount);
    }
  }, [instance, accounts]);

  const changePasswordClick = () => {
    instance.loginRedirect(changePasswordRequest);
  };

  const editProfileClick = () => {
    instance.loginRedirect(editProfileRequest);
  };

  const logoutClick = () => {
    instance.logoutRedirect();
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
        onClick: logoutClick,
      },
    ],
  };

  return (
    <React.Fragment>
      <AuthenticatedTemplate>
        <DefaultButton text={account ? account.name : formatMessage({ id: 'auth.myprofile' })} split menuProps={menuProps} />
      </AuthenticatedTemplate>
      <UnauthenticatedTemplate>
        <DefaultButton text={formatMessage({ id: 'auth.loginbutton' })} onClick={() => instance.loginRedirect(loginButtonRequest)} />
      </UnauthenticatedTemplate>
    </React.Fragment>
  );
};

export default LoginSignupButton;
