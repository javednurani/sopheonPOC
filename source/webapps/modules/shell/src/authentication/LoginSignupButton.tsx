import { AccountInfo } from '@azure/msal-browser';
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import { DefaultButton, IButtonStyles, IContextualMenuProps, IIconProps, IIconStyles } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import { isDarkTheme } from '@sopheon/shared-ui';
import { GetAccessTokenAction } from '@sopheon/shell-api';
import React, { FunctionComponent, useEffect, useState } from 'react';
import { useIntl } from 'react-intl';

import { pendo } from '../pendo';
import { ChangeThemeAction } from '../themes/themeReducer/themeReducer';
import { changePasswordRequest, editProfileRequest, getMsalAccount, loginButtonRequest, logoutRequest } from './authHelpers';
import { SetEnvironmentKeyAction } from './authReducer';

export interface ILoginSignupButtonProps {
  changeTheme: (useDarkTheme: boolean) => ChangeThemeAction;
  setEnvironmentKey: (environmentKey: string) => SetEnvironmentKeyAction;
  getAccessToken: () => GetAccessTokenAction;
}

const LoginSignupButton: FunctionComponent<ILoginSignupButtonProps> = ({
  setEnvironmentKey,
  getAccessToken,
  changeTheme,
}: ILoginSignupButtonProps) => {
  const { formatMessage } = useIntl();
  const theme = useTheme();
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

      if (pendo !== undefined) {
        pendo.initialize({
          visitor: {
            id: msalAccount.localAccountId, // Required if user is logged in
            // email:        // Recommended if using Pendo Feedback, or NPS Email
            // full_name:    // Recommended if using Pendo Feedback
            // role:         // Optional

            // You can add any additional visitor level key-values here,
            // as long as it's not one of the above reserved names.
          },

          account: {
            id: 'SUBSCRIPTION-UNIQUE-ID', // TODO: Use SubscriptionId when available // Required if using Pendo Feedback
            // name:         // Optional
            // is_paying:    // Recommended if using Pendo Feedback
            // monthly_value:// Recommended if using Pendo Feedback
            // planLevel:    // Optional
            // planPrice:    // Optional
            // creationDate: // Optional

            // You can add any additional account level key-values here,
            // as long as it's not one of the above reserved names.
          },
        });
      }
    }
  }, [instance, accounts]);

  const changePasswordClick = () => {
    instance.loginRedirect(changePasswordRequest);
  };

  const editProfileClick = () => {
    instance.loginRedirect(editProfileRequest);
  };

  const logoutClick = () => {
    instance.logoutRedirect(logoutRequest);
  };

  const switchTheme = () => {
    changeTheme(!isDarkTheme(theme));
  };

  const menuProps: IContextualMenuProps = {
    items: [
      {
        key: 'themeToggle',
        id: 'themeToggle',
        text: formatMessage({ id: 'header.useDarkTheme' }),
        iconProps: { iconName: isDarkTheme(theme) ? 'ToggleRight' : 'ToggleLeft' },
        onClick: switchTheme,
      },
      {
        key: 'profile',
        id: 'profile',
        text: formatMessage({ id: 'auth.myprofile' }),
        iconProps: { iconName: 'EditContact' },
        onClick: editProfileClick,
      },
      {
        key: 'changepassword',
        id: 'changepassword',
        text: formatMessage({ id: 'auth.changepassword' }),
        iconProps: { iconName: 'Permissions' },
        onClick: changePasswordClick,
      },
      {
        key: 'signout',
        id: 'signout',
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
      border: 'none',
      color: theme.palette.neutralSecondary,
    },
  };

  const contactIcon: IIconProps = { iconName: 'Contact', styles: contactIconStyles };

  return (
    <React.Fragment>
      <AuthenticatedTemplate>
        <DefaultButton
          text={account ? account.name : formatMessage({ id: 'auth.myprofile' })}
          menuProps={menuProps}
          iconProps={contactIcon}
          styles={loginButtonStyles}
        />
      </AuthenticatedTemplate>
      <UnauthenticatedTemplate>
        <DefaultButton text={formatMessage({ id: 'auth.loginbutton' })} onClick={() => instance.loginRedirect(loginButtonRequest)} />
      </UnauthenticatedTemplate>
    </React.Fragment>
  );
};

export default LoginSignupButton;
