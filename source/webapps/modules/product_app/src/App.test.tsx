import { Label } from '@fluentui/react';
import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';
import { IntlProvider } from 'react-intl';

import App, { Props } from './App';
import OnboardingInfo from './onboardingInfo';

expect.extend(toHaveNoViolations);

describe.skip('Testing the App component', () => {
  it('Render test for the App component', () => {
    const appProps: Props = {
      currentStep: 1,
      nextStep: jest.fn(),
      createProduct: jest.fn(),
      updateProduct: jest.fn(),
      products: [],
      environmentKey: '',
      getAccessToken: jest.fn(),
      accessToken: '',
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <App {...appProps} />
      </IntlProvider>
    );

    expect(wrapper.find(Label)).toHaveLength(1);
    expect(wrapper.find(OnboardingInfo)).toHaveLength(1);
    expect(wrapper.find(OnboardingInfo).props()).toStrictEqual(appProps);
  });
  it('Accessibility test for the App component', async () => {
    const appProps: Props = {
      currentStep: 1,
      nextStep: jest.fn(),
      createProduct: jest.fn(),
      updateProduct: jest.fn(),
      products: [],
      environmentKey: '',
      getAccessToken: jest.fn(),
      accessToken: '',
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <App {...appProps} />
      </IntlProvider>
    );

    const results = await axe(wrapper.getDOMNode(), {
      rules: {
        // These axe rules are fixed when app is run in the shell
        region: { enabled: false },
      },
    });
    expect(results).toHaveNoViolations();
  });
  it('App snapshot render test', () => {
    const appProps: Props = {
      currentStep: 1,
      createProduct: jest.fn(),
      updateProduct: jest.fn(),
      environmentKey: '',
    };

    const tree = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <App {...appProps} />
      </IntlProvider>
    );

    expect(
      toJson(tree, {
        noKey: false,
        mode: 'deep',
      })
    ).toMatchSnapshot();
  });
});
