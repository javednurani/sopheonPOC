import { AccountInfo, Configuration, PublicClientApplication } from '@azure/msal-browser';
import { setIconOptions } from '@fluentui/react/lib-commonjs/Styling';
import { constants, messages } from '@sopheon/shared-ui';
import { render as rtlRender, RenderResult } from '@testing-library/react';
import React, { ReactElement, ReactNode } from 'react';
import { Provider } from 'react-redux';
import renderer from 'react-test-renderer';
import { combineReducers, createStore } from 'redux';

import ConnectedIntlProvider from './languages/ConnectedIntlProvider';
import { shell } from './rootReducer';
import { RootState } from './store';
import { initialState as initialThemeState } from './themes/themeReducer/themeReducer';
import { LanguageShape } from './types';

export const randomString = (): string => Math.random().toString().substring(2, 15);

export const randomMsalAccount = (): AccountInfo => ({
  homeAccountId: randomString(),
  localAccountId: randomString(),
  environment: randomString(),
  tenantId: randomString(),
  username: `${randomString()}@test.com`,
  name: randomString(), // This value will appear on button
});

export const testMsalInstance = (): PublicClientApplication => {
  const msalConfig: Configuration = {
    auth: {
      clientId: randomString(),
    },
  };
  const pca = new PublicClientApplication(msalConfig);
  return pca;
};

const rootReducer = combineReducers({ shell });
const initialLanguageState: LanguageShape = {
  direction: constants.LOCALE_DIR_LTR,
  locale: 'en',
  messages: messages.en,
};

// react-test-renderer - for snapshot testing
export const renderConnectedNodeForSnapshot: (
  initialState: RootState,
  children: ReactNode,
  props?: { locale: string } | undefined
) => renderer.ReactTestRenderer = (initialState, children, props = { locale: 'en' }) => {
  const store = createStore(rootReducer);

  return renderer.create(
    <Provider store={store}>
      <ConnectedIntlProvider language={initialLanguageState}>{children}</ConnectedIntlProvider>
    </Provider>
  );
};

// Not moved to shared-ui package becuase it depends on local root reducer and store
export const languageRender: (ui: ReactElement, initialState: RootState) => RenderResult = (ui, initialState) => {
  const store = createStore(rootReducer);

  const Wrapper: ({ children }: any) => ReactElement = ({ children }: any) => (
    <Provider store={store}>
      <ConnectedIntlProvider language={initialLanguageState}>{children}</ConnectedIntlProvider>
    </Provider>
  );

  return rtlRender(ui, { wrapper: Wrapper });
};

// Check if any slice is not present and set a default value
// TODO: Make this automatic else will need to be updated for each new slice
export const getInitState: (initialState: Partial<RootState>) => RootState = initialState => {
  const rootState: RootState = {
    shell: {
      theme: initialState?.shell?.theme ? initialState.shell.theme : initialThemeState,
    },
  };

  return rootState;
};

export const disableFluentUiIconWarnings: () => void = () => {
  setIconOptions({
    disableWarnings: true,
  });
};

export * from '@testing-library/react';
