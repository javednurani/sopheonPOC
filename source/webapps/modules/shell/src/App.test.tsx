import { ThemeProvider } from '@fluentui/react-theme-provider';
import { darkTheme, lightTheme, messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';
import { IntlProvider } from 'react-intl';

import App, { AppProps } from './App';
import Footer from './footer/Footer';
import Header from './header/Header';

expect.extend(toHaveNoViolations);

describe('Testing the App component', () => {
  it('Render test for the App component', () => {
    const appProps: AppProps = {
      changeTheme: jest.fn(),
      setEnvironmentKey: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <App {...appProps} />
      </IntlProvider>
    );
    expect(wrapper.find(Header)).toHaveLength(1);
    expect(wrapper.find(Header).props()).toStrictEqual(appProps);
    expect(wrapper.find('main')).toHaveLength(1);
    expect(wrapper.find('main').getDOMNode()).toHaveAttribute('role', 'main');
    expect(wrapper.find(Footer)).toHaveLength(1);
  });
  it('Accessibility test for the App component', async () => {
    const appProps: AppProps = {
      changeTheme: jest.fn(),
      setEnvironmentKey: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <App {...appProps} />
      </IntlProvider>
    );
    //When the app is nested in the shell, we expect 0 axe violations
    const results = await axe(wrapper.getDOMNode());
    expect(results).toHaveNoViolations();
  });
});
