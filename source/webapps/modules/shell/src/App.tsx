import { AuthenticatedTemplate, UnauthenticatedTemplate } from '@azure/msal-react';
import { initializeIcons, registerIcons, ScrollablePane, ScrollbarVisibility, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import { GetAccessTokenAction, HideAnnouncementAction, ShowAnnouncementModel } from '@sopheon/shell-api';
import React, { CSSProperties, FunctionComponent } from 'react';
import { useIntl } from 'react-intl';
import { BrowserRouter, Route, Switch } from 'react-router-dom';

import Announcement from './Announcement';
import { SetEnvironmentKeyAction } from './authentication/authReducer';
import IdleMonitor from './authentication/IdleMonitor';
import Login from './authentication/Login';
import Signup from './authentication/Signup';
import { DynamicModule } from './DynamicModule';
import Header from './header/Header';
import { ReactComponent as SopheonLogoDark } from './images/sopheon_logo_blk_txt.svg';
import { ReactComponent as SopheonLogoLight } from './images/sopheon_logo_wht_txt.svg';
import { appModules } from './settings/appModuleSettings';
import { shellApi } from './ShellApi';
import { ChangeThemeAction } from './themes/themeReducer/themeReducer';

export interface AppProps {
  changeTheme: (useDarkTheme: boolean) => ChangeThemeAction;
  setEnvironmentKey: (environmentKey: string) => SetEnvironmentKeyAction;
  environmentKey: string | null;
  headerShown: boolean;
  getAccessToken: () => GetAccessTokenAction;
  hideAnnouncement: () => HideAnnouncementAction;
  announcementShown: boolean;
  announcementContent: ShowAnnouncementModel | null;
}

const App: FunctionComponent<AppProps> = ({
  changeTheme,
  setEnvironmentKey,
  headerShown,
  getAccessToken,
  hideAnnouncement,
  announcementShown,
  announcementContent,
}: AppProps) => {
  const { formatMessage } = useIntl();

  const loadingMessage: string = formatMessage({ id: 'fallback.loading' });
  useTheme();

  initializeIcons();

  registerIcons({
    icons: {
      SopheonLogoDark: <SopheonLogoDark />,
      SopheonLogoLight: <SopheonLogoLight />,
    },
  });

  const appStyle: CSSProperties = {
    height: '100vh',
  };

  const pageContainerStyle: CSSProperties = {
    position: 'relative',
    height: '100%',
  };

  return (
    <div className="App" style={appStyle}>
      <BrowserRouter>
        <Switch>
          <Route exact path="/login">
            <Login />
          </Route>
          <Route exact path="/signup">
            <Signup />
          </Route>
          <Route path="/">
            <Stack
              grow
              styles={{
                root: {
                  height: '100%',
                  width: '100%',
                },
              }}
            >
              <Stack.Item>
                {headerShown && <Header changeTheme={changeTheme} setEnvironmentKey={setEnvironmentKey} getAccessToken={getAccessToken} />}
                {announcementShown && <Announcement hideAnnouncement={hideAnnouncement} announcementContent={announcementContent} />}
              </Stack.Item>
              <Stack.Item shrink>
                <IdleMonitor />
              </Stack.Item>
              <Stack.Item
                verticalFill
                styles={{
                  root: {
                    height: '100%',
                    overflow: 'auto',
                    zIndex: '9999',
                  },
                }}
              >
                <main style={pageContainerStyle} role="main">
                  <ScrollablePane scrollbarVisibility={ScrollbarVisibility.auto}>
                    <Switch>
                      {appModules.map(appModule => (
                        <Route exact key={appModule.scope} path={appModule.routeName}>
                          <AuthenticatedTemplate>
                            <DynamicModule module={appModule} loadingMessage={loadingMessage} shellApi={shellApi} />
                          </AuthenticatedTemplate>
                          <UnauthenticatedTemplate>
                            <Login />
                          </UnauthenticatedTemplate>
                        </Route>
                      ))}
                    </Switch>
                  </ScrollablePane>
                </main>
              </Stack.Item>
            </Stack>
          </Route>
        </Switch>
      </BrowserRouter>
    </div>
  );
};

export default App;
