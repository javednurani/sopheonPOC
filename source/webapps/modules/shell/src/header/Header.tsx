import { AuthenticatedTemplate } from '@azure/msal-react';
import { IStackTokens, Stack, Sticky, StickyPositionType } from '@fluentui/react';
import { GetAccessTokenAction } from '@sopheon/shell-api';
import React, { FunctionComponent } from 'react';
import { useIntl } from 'react-intl';

import { SetEnvironmentKeyAction } from '../authentication/authReducer';
import LoginSignupButton from '../authentication/LoginSignupButton';
import NotificationsButton from '../authentication/NotificationsButton';
import Navbar from '../navbar/Navbar';
import SopheonLogo from '../SopheonLogo';
import { ChangeThemeAction } from '../themes/themeReducer/themeReducer';

interface HeaderProps {
  changeTheme: (useDarkTheme: boolean) => ChangeThemeAction;
  setEnvironmentKey: (environmentKey: string) => SetEnvironmentKeyAction;
  getAccessToken: () => GetAccessTokenAction;
}

const Header: FunctionComponent<HeaderProps> = ({ changeTheme, setEnvironmentKey, getAccessToken }: HeaderProps) => {
  const { formatMessage } = useIntl();

  const headerStyle: React.CSSProperties = {
    margin: '0 0 5px 0',
    padding: '0 5px',
    height: '42px',
    boxShadow: '0 0 5px 0 #888888',
  };

  const logoStyle: React.CSSProperties = {
    width: '150px',
    overflow: 'visible',
    marginLeft: '20px',
  };

  const navContainerStyle: React.CSSProperties = {
    marginLeft: '40px',
  };

  const stackTokensWithGap: IStackTokens = {
    childrenGap: 1,
  };

  return (
    <Sticky stickyPosition={StickyPositionType.Header} isScrollSynced>
      <header role="banner">
        <Stack horizontal verticalAlign="center" style={headerStyle}>
          <Stack.Item shrink>
            <div title={formatMessage({ id: 'sopheon' })}>
              <SopheonLogo style={logoStyle} />
            </div>
          </Stack.Item>
          <Stack.Item style={navContainerStyle}>
            <AuthenticatedTemplate>
              <Navbar />
            </AuthenticatedTemplate>
          </Stack.Item>
          <Stack.Item grow>
            <Stack tokens={stackTokensWithGap} horizontal verticalAlign="center" horizontalAlign="end">
              <Stack.Item>
                <NotificationsButton />
              </Stack.Item>
              <Stack.Item>
                <LoginSignupButton setEnvironmentKey={setEnvironmentKey} getAccessToken={getAccessToken} changeTheme={changeTheme} />
              </Stack.Item>
            </Stack>
          </Stack.Item>
        </Stack>
      </header>
    </Sticky>
  );
};

export default Header;
