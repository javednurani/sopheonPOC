import React, { ReactNode } from 'react';
import { IntlProvider } from 'react-intl';

import { LanguageShape } from '../types';

export interface ConnectedIntlProviderProps {
  language: LanguageShape;
  children: ReactNode;
}

const ConnectedIntlProvider: React.FunctionComponent<ConnectedIntlProviderProps> = ({ children, language }: ConnectedIntlProviderProps) => (
  <IntlProvider locale={language.locale} messages={language.messages}>
    {children}
  </IntlProvider>
);

export default ConnectedIntlProvider;
