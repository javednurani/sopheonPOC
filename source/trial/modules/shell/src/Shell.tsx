import { Configuration, PublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import React, { FunctionComponent } from 'react';
import { ConnectedProps } from 'react-redux';

import App from './App';
import azureSettings from './azureSettings';
import ConnectedIntlProvider from './languages/ConnectedIntlProvider';
import { shellApi } from './ShellApi';
import ConnectedThemeProvider from './themes/components/connectedThemeProvider/ConnectedThemeProvider';

const isDev = process.env.NODE_ENV === 'development';

const adB2cTenantName: string = isDev ? azureSettings.AD_B2C_TenantName_Dev : azureSettings.AD_B2C_TenantName;

const msalConfig: Configuration = {
  auth: {
    authority: `https://${adB2cTenantName}.b2clogin.com/${adB2cTenantName}.onmicrosoft.com/${azureSettings.AD_B2C_SignUpSignIn_Policy}`,
    clientId: isDev ? azureSettings.AD_B2C_ClientId_Dev : azureSettings.AD_B2C_ClientId,
    knownAuthorities: [`${adB2cTenantName}.b2clogin.com`],
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
