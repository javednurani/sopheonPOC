import { initializeIcons, registerIcons, ScrollablePane, ScrollbarVisibility, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { CSSProperties, FunctionComponent } from 'react';
import { useIntl } from 'react-intl';
import { BrowserRouter, Route, Switch } from 'react-router-dom';

import IdleMonitor from './authentication/IdleMonitor';
import Login from './authentication/Login';
import Signup from './authentication/Signup';
import { DynamicModule } from './DynamicModule';
import Footer from './footer/Footer';
import Header from './header/Header';
import { ReactComponent as LLogo } from './images/Lucy24_logo.svg';
import { appModules } from './settings/appModuleSettings';
import { shellApi } from './ShellApi';
import { ChangeThemeAction } from './themes/themeReducer/themeReducer';

export interface AppProps {
  changeTheme: (useDarkTheme: boolean) => ChangeThemeAction;
}

const App: FunctionComponent<AppProps> = ({ changeTheme }: AppProps) => {
  const { formatMessage } = useIntl();
  const loadingMessage: string = formatMessage({ id: 'fallback.loading' });
  useTheme();

  initializeIcons();

  const lucyIconStyle: CSSProperties = {
    width: '20px',
    height: '20px',
    overflow: 'visible',
  };

  registerIcons({
    icons: {
      Lucy: <LLogo style={lucyIconStyle} />,
    },
  });

  const appStyle: CSSProperties = {
    textAlign: 'center',
    height: '100vh',
  };

  const pageContainerStyle: CSSProperties = {
    position: 'relative',
    height: '100%',
  };

  // TODO: check for user product existance CLOUD-2148
  const userHasProduct = false;

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
              {(!location.pathname.includes('product') || userHasProduct) &&
                <Stack.Item>
                  <Header changeTheme={changeTheme} />
                </Stack.Item>
              }
              <Stack.Item shrink>
                <IdleMonitor />
              </Stack.Item>
              <Stack.Item
                verticalFill
                styles={{
                  root: {
                    height: '100%',
                    overflow: 'auto',
                  },
                }}
              >
                <main style={pageContainerStyle} role="main">
                  <ScrollablePane scrollbarVisibility={ScrollbarVisibility.auto}>
                    <Switch>
                      {appModules.map(appModule => (
                        <Route exact key={appModule.scope} path={appModule.routeName}>
                          <DynamicModule module={appModule} loadingMessage={loadingMessage} shellApi={shellApi} />
                        </Route>
                      ))}
                    </Switch>
                  </ScrollablePane>
                </main>
              </Stack.Item>
              {(!location.pathname.includes('product') || userHasProduct) &&
                <Stack.Item>
                  <Footer />
                </Stack.Item>
              }
            </Stack>
          </Route>
        </Switch>
      </BrowserRouter>
    </div>
  );
};

export default App;
