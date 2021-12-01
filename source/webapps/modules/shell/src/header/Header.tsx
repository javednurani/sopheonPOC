import { IStackTokens, Stack, Sticky, StickyPositionType } from '@fluentui/react';
import { GetAccessTokenAction } from '@sopheon/shell-api';
import React, { FunctionComponent } from 'react';
import { useIntl } from 'react-intl';

import { SetEnvironmentKeyAction } from '../authentication/authReducer';
import LoginSignupButton from '../authentication/LoginSignupButton';
import Navbar from '../navbar/Navbar';
import SopheonLogo from '../SopheonLogo';
import ThemeSelector from '../themes/components/themeSelector/ThemeSelector';
import { ChangeThemeAction } from '../themes/themeReducer/themeReducer';

interface HeaderProps {
  changeTheme: (useDarkTheme: boolean) => ChangeThemeAction;
  setEnvironmentKey: (environmentKey: string) => SetEnvironmentKeyAction;
  getAccessToken: () => GetAccessTokenAction;
}

const Header: FunctionComponent<HeaderProps> = ({ changeTheme, setEnvironmentKey, getAccessToken }: HeaderProps) => {
  const { formatMessage } = useIntl();

  const headerStyle: React.CSSProperties = {
    marginTop: '8px',
    borderBottom: '1px solid',
  };

  const logoStyle: React.CSSProperties = {
    height: '100%',
    width: '100%',
    maxWidth: '200px',
    maxHeight: '200px',
    minWidth: '25px',
    minHeight: '25px',
    overflow: 'visible',
  };

  const logoContainerStyle: React.CSSProperties = {
    margin: '5px',
  };

  const stackTokensWithGap: IStackTokens = {
    childrenGap: 2,
  };

  /*
  const location = useLocation();

  const getTitle = (path: string): string => {
    const currentApp: AppModule | undefined = appModules.find(appModule => appModule.routeName === path);
    return currentApp ? formatMessage({ id: currentApp.displayNameResourceKey }) : formatMessage({ id: 'defaultTitle' });
  };

  const pageTitle: string = getTitle(location.pathname);
  */

  return (
    <Sticky stickyPosition={StickyPositionType.Header} isScrollSynced>
      <header style={headerStyle} role="banner">
        <Stack horizontal verticalAlign="center">
          <Stack.Item shrink>
            <div style={logoContainerStyle} title={formatMessage({ id: 'sopheon' })}>
              <SopheonLogo style={logoStyle} />
            </div>
          </Stack.Item>
          <Stack.Item>
            <Navbar />
          </Stack.Item>
          <Stack.Item grow align="center">
            <h1 id="page-title" className="page-title">
              {/* {pageTitle} */}
            </h1>
          </Stack.Item>
          <Stack.Item>
            <Stack tokens={stackTokensWithGap}>
              <Stack.Item>
                <LoginSignupButton setEnvironmentKey={setEnvironmentKey} getAccessToken={getAccessToken} />
              </Stack.Item>
              <Stack.Item>
                <ThemeSelector changeTheme={changeTheme} />
              </Stack.Item>
            </Stack>
          </Stack.Item>
        </Stack>
      </header>
    </Sticky>
  );
};

export default Header;
