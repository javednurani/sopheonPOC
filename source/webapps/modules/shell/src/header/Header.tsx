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
    margin: '8px 10px 3px',
    height: '42px',
    boxShadow: '0px 1px 10px 1px #888888',
  };

  const logoStyle: React.CSSProperties = {
    height: '80%',
    width: '80%',
    maxWidth: '200px',
    maxHeight: '200px',
    minWidth: '25px',
    minHeight: '25px',
    overflow: 'visible',
    marginLeft: '20px',
    marginTop: '5px',
  };

  const navContainerStyle: React.CSSProperties = {
    marginLeft: '40px',
  };

  const stackTokensWithGap: IStackTokens = {
    childrenGap: 1,
  };

  return (
    <Sticky stickyPosition={StickyPositionType.Header} isScrollSynced>
      <header style={headerStyle} role="banner">
        <Stack horizontal verticalAlign="center">
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
