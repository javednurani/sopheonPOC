import { messages } from '@sopheon/shared-ui';
import { FetchStatus } from '@sopheon/shell-api';
import { mount } from 'enzyme';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';
import { IntlProvider } from 'react-intl';

import App, { Props } from './App';
import OnboardingInfo from './onboarding/onboardingInfo';

expect.extend(toHaveNoViolations);

describe('Testing the App component', () => {
  it('Render test for the App component', () => {
    const appProps: Props = {
      currentStep: 1,
      nextStep: jest.fn(),
      environmentKey: 'asdf',
      getAccessToken: jest.fn(),
      accessToken: '',
      showHeader: jest.fn(),
      hideHeader: jest.fn(),
      products: [],
      getProductsFetchStatus: FetchStatus.DoneSuccess,
      getProducts: jest.fn(),
      createProduct: jest.fn(),
      updateProduct: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <App {...appProps} />
      </IntlProvider>
    );

    expect(wrapper.find(OnboardingInfo)).toHaveLength(1);
  });
  it('Accessibility test for the App component', async () => {
    const appProps: Props = {
      currentStep: 1,
      nextStep: jest.fn(),
      environmentKey: 'asdf',
      getAccessToken: jest.fn(),
      accessToken: '',
      showHeader: jest.fn(),
      hideHeader: jest.fn(),
      products: [],
      getProductsFetchStatus: FetchStatus.NotActive,
      getProducts: jest.fn(),
      createProduct: jest.fn(),
      updateProduct: jest.fn(),
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
});
