import { PrimaryButton } from '@fluentui/react';
import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import React from 'react';
import { FormattedMessage, IntlProvider } from 'react-intl';

import { Props } from './App';
import OnboardingInfo from './onboardingInfo';

describe('Testing the onboardingInfo component', () => {
  it('Render test for the onboardingInfo component', () => {
    const appProps: Props = {
      currentStep: 1,
      nextStep: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <OnboardingInfo
          currentStep={appProps.currentStep}
          nextStep={appProps.nextStep}
        />
      </IntlProvider>
    );

    expect(wrapper.find(FormattedMessage).text()).toBe(`step${appProps.currentStep}`);
    expect(wrapper.find(PrimaryButton)).toHaveLength(1);
  });
  it('userEvent test for the onboardingInfo component', () => {
    const appProps: Props = {
      currentStep: 1,
      nextStep: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <OnboardingInfo
          currentStep={appProps.currentStep}
          nextStep={appProps.nextStep}
        />
      </IntlProvider>
    );

    wrapper.find(PrimaryButton).simulate('click');
    expect(appProps.nextStep).toHaveBeenCalled();
  });
});
