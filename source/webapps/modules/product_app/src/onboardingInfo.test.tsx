import { messages } from '@sopheon/shared-ui';
import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import React from 'react';
import { IntlProvider } from 'react-intl';

import { Props } from './App';
import OnboardingInfo from './onboardingInfo';

describe('Testing the onboardingInfo component', () => {
  it('step 2 renders correctly', async () => {
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

    userEvent.type(nameTextField, 'test');
    userEvent.click(industryDropdown);
    const allOpts: HTMLElement[] = screen.getAllByRole('option');
    userEvent.click(allOpts[0]); // Select the first option in the dropdown
    await waitFor(() => expect(continueButton).not.toBeDisabled());
    fireEvent.change(nameTextField, { target: { value: '' } });
    expect(nameTextField).toHaveValue('');
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
