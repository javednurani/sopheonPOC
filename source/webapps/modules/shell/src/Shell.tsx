import { MsalProvider } from '@azure/msal-react';
import React, { FunctionComponent } from 'react';
import { ConnectedProps } from 'react-redux';

import App from './App';
import { msalInstance } from './authentication/authHelpers';
import ConnectedIntlProvider from './languages/ConnectedIntlProvider';
import { shellApi } from './ShellApi';
import ConnectedThemeProvider from './themes/components/connectedThemeProvider/ConnectedThemeProvider';

const connector = shellApi.getConnector();

export type ShellProps = ConnectedProps<typeof connector>;

const Shell: FunctionComponent<ShellProps> = ({ changeTheme, theme, setEnvironmentKey, environmentKey, language, showHeaderFooter }: ShellProps) => (
  <MsalProvider instance={msalInstance()}>
    <ConnectedIntlProvider language={language}>
      <ConnectedThemeProvider theme={theme}>
        <App changeTheme={changeTheme} setEnvironmentKey={setEnvironmentKey} environmentKey={environmentKey} showHeaderFooter={showHeaderFooter} />
      </ConnectedThemeProvider>
    </ConnectedIntlProvider>
  </MsalProvider>
);

export default connector(Shell);
