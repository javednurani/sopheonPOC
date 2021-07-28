import { initializeIcons, registerIcons, ScrollablePane, ScrollbarVisibility, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { CSSProperties, FunctionComponent } from 'react';
import { useIntl } from 'react-intl';
import { BrowserRouter, Route, Switch } from 'react-router-dom';

import { appModules } from './appModuleSettings';
import NewUserLanding from './authentication/NewUserLanding';
import { DynamicModule } from './DynamicModule';
import Footer from './footer/Footer';
import Header from './header/Header';
import { ReactComponent as LLogo } from './images/Lucy24_logo.svg';
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

  return (
    <div className="App" style={appStyle}>
      <BrowserRouter>
        <Switch>
          <Route exact path="/newuserlanding">
            <NewUserLanding />
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
              <Stack.Item shrink>
                <Header changeTheme={changeTheme} />
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
              <Stack.Item>
                <Footer />
              </Stack.Item>
            </Stack>
          </Route>
        </Switch>
      </BrowserRouter>
    </div>
  );
};

export default App;
