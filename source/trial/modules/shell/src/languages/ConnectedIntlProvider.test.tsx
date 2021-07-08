import { constants, messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import React from 'react';
import { FormattedMessage } from 'react-intl';

import { LanguageShape } from './../types';
import ConnectedIntlProvider from './ConnectedIntlProvider';

describe('Test ConnectedIntlProvider component', () => {
  it('Test ConnectedIntlProvider component render', () => {
    const language: LanguageShape = {
      locale: 'en',
      direction: constants.LOCALE_DIR_LTR,
      messages: messages.en,
    };
    const wrapper = mount(
      <ConnectedIntlProvider language={language}>
        <FormattedMessage id="defaultTitle" />
      </ConnectedIntlProvider>
    );
    expect(wrapper.find(FormattedMessage).text()).toBe(messages.en.defaultTitle);
  });
});
