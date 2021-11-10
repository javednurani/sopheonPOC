import { PrimaryButton } from '@fluentui/react';
import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import React from 'react';
import { FormattedMessage, IntlProvider } from 'react-intl';

import { Props } from './App';
import OnboardingInfo from './onboardingInfo';

describe.skip('Testing the onboardingInfo component', () => {
  it('Render test for the onboardingInfo component', () => {
    const appProps: Props = {
      currentStep: 2,
      nextStep: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <OnboardingInfo currentStep={appProps.currentStep} nextStep={appProps.nextStep} />
      </IntlProvider>
    );
    // TODO remove hardcoded string?
    expect(wrapper.find(FormattedMessage).text()).toBe(`Step ${appProps.currentStep}`); //TODO look for label with messages value.
    expect(wrapper.find(PrimaryButton)).toHaveLength(1);
  });
  it('userEvent test for the onboardingInfo component', () => {
    const appProps: Props = {
      currentStep: 2,
      nextStep: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <OnboardingInfo currentStep={appProps.currentStep} nextStep={appProps.nextStep} />
      </IntlProvider>
    );

    wrapper.find(PrimaryButton).simulate('click');
    expect(appProps.nextStep).toHaveBeenCalled();
  });
});
