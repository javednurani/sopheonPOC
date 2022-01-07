import axe from '@axe-core/react';
import { initializeIcons } from '@fluentui/react';
import { ThemeProvider } from '@fluentui/react-theme-provider';
import React from 'react';
import ReactDOM from 'react-dom';
import { IntlProvider } from 'react-intl';
import { Provider } from 'react-redux';

import AppContainer from './AppContainer';
import { store } from './store';
import { lightTheme, messages } from './TempSopheonSharedUI';
import { IsolatedShellApi } from './TempSopheonShellAPI';

const shellApi = new IsolatedShellApi(store);

initializeIcons();

const locale = 'en';

if (process.env.NODE_ENV !== 'production') {
  axe(React, ReactDOM, 1000);
}

ReactDOM.render(
  <Provider store={store}>
    <IntlProvider locale={locale} messages={messages[locale]}>
      <ThemeProvider applyTo="body" theme={lightTheme}>
        <AppContainer shellApi={shellApi} />
      </ThemeProvider>
    </IntlProvider>
  </Provider>,
  document.getElementById('root')
);
