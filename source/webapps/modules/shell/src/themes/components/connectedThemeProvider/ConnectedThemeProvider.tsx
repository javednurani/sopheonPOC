import { ThemeProvider } from '@fluentui/react-theme-provider';
import React, { ReactNode } from 'react';

import { ThemeShape } from '../../../types';

export interface ConnectedThemeProviderProps {
  children: ReactNode;
  theme: ThemeShape;
}

const ConnectedThemeProvider: React.FunctionComponent<ConnectedThemeProviderProps> = ({ children, theme }: ConnectedThemeProviderProps) => (
  <ThemeProvider applyTo="body" theme={theme.theme}>
    {children}
  </ThemeProvider>
);

export default ConnectedThemeProvider;
