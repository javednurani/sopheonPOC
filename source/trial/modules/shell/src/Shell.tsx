import { Configuration, PublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import React, { FunctionComponent } from 'react';
import { ConnectedProps } from 'react-redux';

import App from './App';
import ConnectedIntlProvider from './languages/ConnectedIntlProvider';
import { azureSettings, getAuthorityUrl } from './settings/azureSettings';
import { shellApi } from './ShellApi';
import ConnectedThemeProvider from './themes/components/connectedThemeProvider/ConnectedThemeProvider';

const msalConfig: Configuration = {
  auth: {
    authority: getAuthorityUrl(azureSettings.AD_B2C_SignUpSignIn_Policy),
    clientId: azureSettings.AD_B2C_ClientId,
    knownAuthorities: [`${azureSettings.AD_B2C_TenantName}.b2clogin.com`],
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
