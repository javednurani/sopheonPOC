import { Configuration, PublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import React, { FunctionComponent } from 'react';
import { ConnectedProps } from 'react-redux';

import App from './App';
import ConnectedIntlProvider from './languages/ConnectedIntlProvider';
import { shellApi } from './ShellApi';
import ConnectedThemeProvider from './themes/components/connectedThemeProvider/ConnectedThemeProvider';

const msalConfig: Configuration = {
  auth: {
    clientId: '8bdfb9a7-913a-48a8-9fe0-5b2877fb844d', // TODO application (clientId) of app registration
    authority: 'https://StratusB2CDev.b2clogin.com/StratusB2CDev.onmicrosoft.com/B2C_1A_SIGNUP_SIGNIN', // TODO authority with B2C tenant Id
    knownAuthorities: ['StratusB2CDev.b2clogin.com'], // TODO
    redirectUri: 'https://localhost:3000/', // TODO location.origin?
  },
};

const pca = new PublicClientApplication(msalConfig);

const connector = shellApi.getConnector();

export type ShellProps = ConnectedProps<typeof connector>;

const Shell: FunctionComponent<ShellProps> = ({ changeTheme, language, theme }: ShellProps) => (
  <MsalProvider instance={pca}>
    <ConnectedIntlProvider language={language}>
      <ConnectedThemeProvider theme={theme}>
        <App changeTheme={changeTheme} />
      </ConnectedThemeProvider>
    </ConnectedIntlProvider>
  </MsalProvider>
);

export default connector(Shell);
