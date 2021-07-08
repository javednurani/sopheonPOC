import React, { FunctionComponent } from 'react';
import { ConnectedProps } from 'react-redux';

import App from './App';
import ConnectedIntlProvider from './languages/ConnectedIntlProvider';
import { shellApi } from './ShellApi';
import ConnectedThemeProvider from './themes/components/connectedThemeProvider/ConnectedThemeProvider';

const connector = shellApi.getConnector();

export type ShellProps = ConnectedProps<typeof connector>;

const Shell: FunctionComponent<ShellProps> = ({ changeTheme, language, theme }: ShellProps) => (
  <ConnectedIntlProvider language={language}>
    <ConnectedThemeProvider theme={theme}>
      <App changeTheme={changeTheme} />
    </ConnectedThemeProvider>
  </ConnectedIntlProvider>
);

export default connector(Shell);
