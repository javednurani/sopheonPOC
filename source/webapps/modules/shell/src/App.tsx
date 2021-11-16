import { useIsAuthenticated } from '@azure/msal-react';
import { initializeIcons, registerIcons, ScrollablePane, ScrollbarVisibility, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import { EnvironmentScopedApiRequestDto, FetchStatus, GetProductsAction, Product } from '@sopheon/shell-api';
import React, { CSSProperties, FunctionComponent, useEffect } from 'react';
import { useIntl } from 'react-intl';
import { BrowserRouter, Route, Switch } from 'react-router-dom';

import { getAccessToken } from './authentication/authHelpers';
import { SetEnvironmentKeyAction } from './authentication/authReducer';
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
  setEnvironmentKey: (environmentKey: string) => SetEnvironmentKeyAction;
  environmentKey: string | null;
  products: Product[];
  getProductsFetchStatus: FetchStatus;
  getProducts: (requestDto: EnvironmentScopedApiRequestDto) => GetProductsAction;
}

const App: FunctionComponent<AppProps> = ({
  changeTheme,
  setEnvironmentKey,
  environmentKey,
  products,
  getProductsFetchStatus,
  getProducts,
}: AppProps) => {
  const { formatMessage } = useIntl();
  const isAuthenticated = useIsAuthenticated();

  useEffect(() => {
    // get any Products for logged in User
    if (environmentKey && getProductsFetchStatus === FetchStatus.NotActive) {
      // TODO, use isAuthenticated ?
      getAccessToken().then(token => {
        const requestDto: EnvironmentScopedApiRequestDto = {
          EnvironmentKey: environmentKey || '',
          AccessToken: token,
        };

        getProducts(requestDto);
      });
    }
  }, [environmentKey, getProductsFetchStatus]);

  const userHasProduct: boolean = products && products.length > 0;

  const userIsOnboardingProductApp = location.pathname.includes('product') && isAuthenticated && !userHasProduct;

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
              {!userIsOnboardingProductApp && (
                <Stack.Item>
                  <Header changeTheme={changeTheme} setEnvironmentKey={setEnvironmentKey} />
                </Stack.Item>
              )}
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
              {/* Only hide Header/Footer for Product Onboarding (when on /product page, is authenticated, but does NOT have a product) */}
              {!userIsOnboardingProductApp && (
                <Stack.Item>
                  <Footer />
                </Stack.Item>
              )}
            </Stack>
          </Route>
        </Switch>
      </BrowserRouter>
    </div>
  );
};

export default App;
