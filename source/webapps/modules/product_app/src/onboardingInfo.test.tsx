import { messages } from '@sopheon/shared-ui';
import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import React from 'react';
import { IntlProvider } from 'react-intl';

import { Props } from './App';
import OnboardingInfo from './onboardingInfo';

describe('Testing the onboardingInfo component', () => {
  it.skip('step 2 renders correctly', async () => {
    const appProps: Props = {
      currentStep: 2,
      nextStep: jest.fn(),
    };

    render(
      <IntlProvider locale="en" messages={messages.en}>
        <OnboardingInfo currentStep={appProps.currentStep} nextStep={appProps.nextStep} />
      </IntlProvider>
    );

    const continueButton = screen.getByRole('button', {
      name: /continue/i,
    });
    expect(continueButton).toBeDisabled();
    const nameTextField = screen.getByLabelText(messages.en['onboarding.yourproductname']);
    const industryDropdown = screen.getByLabelText(messages.en['onboarding.industryselection']);

    expect(nameTextField).toBeInTheDocument();
    expect(industryDropdown).toBeInTheDocument();

    fireEvent.change(nameTextField, { target: { value: 'test' } });
    fireEvent.change(industryDropdown, { target: { option: { key: 2 } } });
    await waitFor(() => expect(continueButton).not.toBeDisabled());
    fireEvent.change(screen.getByLabelText(messages.en['onboarding.yourproductname']), { target: { newValue: '' } });
    expect(continueButton).toBeDisabled();
  });
  it('step 3 components render correctly', async () => {
    const appProps: Props = {
      currentStep: 3,
      nextStep: jest.fn(),
    };

    render(
      <IntlProvider locale="en" messages={messages.en}>
        <OnboardingInfo currentStep={appProps.currentStep} nextStep={appProps.nextStep} />
      </IntlProvider>
    );
    const goalTextField: HTMLElement = screen.getByLabelText(messages.en['onboarding.productgoal']);
    const kpiTextField = screen.getByLabelText(messages.en['onboarding.productKpi']);

    expect(goalTextField).toBeInTheDocument();
    expect(kpiTextField).toBeInTheDocument();
    expect(
      screen.getByRole('button', {
        name: /Get Started!/i,
      })
    ).not.toBeDisabled();
  });
  it('next step function fires on button click', () => {
    const appProps: Props = {
      currentStep: 3,
      nextStep: jest.fn(),
    };
    render(
      <IntlProvider locale="en" messages={messages.en}>
        <OnboardingInfo currentStep={appProps.currentStep} nextStep={appProps.nextStep} />
      </IntlProvider>
    );
    fireEvent.click(
      screen.getByRole('button', {
        name: /Get Started!/i,
      })
    );
    expect(appProps.nextStep).toHaveBeenCalled();
  });
});
