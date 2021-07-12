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
    clientId: '&ShellAppClientId&', // TODO application (clientId) of app registration
    authority: 'https://&B2CTenantName&.b2clogin.com/&B2CTenantName&.onmicrosoft.com/B2C_1A_SIGNUP_SIGNIN', // TODO authority with B2C tenant Id
    knownAuthorities: ['&B2CTenantName&.b2clogin.com'], // TODO
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
