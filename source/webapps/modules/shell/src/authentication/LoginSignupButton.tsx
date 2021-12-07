import { AccountInfo } from '@azure/msal-browser';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import {
  DefaultButton,
  IButtonStyles,
  IconButton,
  IContextualMenuProps,
  IIconProps,
  IIconStyles,
  ITooltipHostStyles,
  TooltipHost,
} from '@fluentui/react';
import { GetAccessTokenAction } from '@sopheon/shell-api';
import React, { FunctionComponent, useEffect, useState } from 'react';
import { useIntl } from 'react-intl';

import { changePasswordRequest, editProfileRequest, getMsalAccount, loginButtonRequest } from './authHelpers';
import { SetEnvironmentKeyAction } from './authReducer';

export interface ILoginSignupButtonProps {
  setEnvironmentKey: (environmentKey: string) => SetEnvironmentKeyAction;
  getAccessToken: () => GetAccessTokenAction;
}

const LoginSignupButton: FunctionComponent<ILoginSignupButtonProps> = ({ setEnvironmentKey, getAccessToken }: ILoginSignupButtonProps) => {
  const { formatMessage } = useIntl();
  const { instance, accounts } = useMsal();

  const [account, setAccount] = useState<AccountInfo>();

  useEffect(() => {
    const msalAccount: AccountInfo | undefined = getMsalAccount(instance);
    if (msalAccount !== undefined) {
      if (msalAccount.idTokenClaims && msalAccount.idTokenClaims.extension_environmentKey) {
        setEnvironmentKey(msalAccount.idTokenClaims.extension_environmentKey);
      }
      setAccount(msalAccount);
      getAccessToken();
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

  const contactIconStyles: Partial<IIconStyles> = {
    root: {
      fontSize: '18px',
    },
  };

  const loginButtonStyles: Partial<IButtonStyles> = {
    root: {
      height: '36px',
      borderRadius: '2px',
    },
  };

  const contactIcon: IIconProps = { iconName: 'Contact', styles: contactIconStyles };

  const hostStyles: Partial<ITooltipHostStyles> = { root: { display: 'inline-block' } };

  return (
    <React.Fragment>
      <AuthenticatedTemplate>
        <TooltipHost content={account ? account.name : formatMessage({ id: 'auth.myprofile' })} id="profileImageTooltip" styles={hostStyles}>
          <IconButton menuProps={menuProps} iconProps={contactIcon} styles={loginButtonStyles} />
        </TooltipHost>
      </AuthenticatedTemplate>
      <UnauthenticatedTemplate>
        <DefaultButton text={formatMessage({ id: 'auth.loginbutton' })} onClick={() => instance.loginRedirect(loginButtonRequest)} />
      </UnauthenticatedTemplate>
    </React.Fragment>
  );
};

export default LoginSignupButton;
