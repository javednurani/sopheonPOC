import { Label, PrimaryButton, Stack, TextField } from '@fluentui/react';
import React from 'react';
import { FormattedMessage, useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';

export type OnboardingInfoProps = AppStateProps & AppDispatchProps;

const headerStyle: React.CSSProperties = {
  marginTop: '20px',
  marginBottom: '20px',
  fontSize: '40px',
};

const OnboardingInfo: React.FunctionComponent<OnboardingInfoProps> = ({ currentStep, nextStep }: OnboardingInfoProps) => {
  const { formatMessage } = useIntl();

  switch (currentStep) {
    case 1:
      return (
        <Stack className="step1" horizontalAlign="center">
          <FormattedMessage id={'step1'} />
          <PrimaryButton text={formatMessage({ id: 'next' })} aria-label={formatMessage({ id: 'next' })} onClick={() => nextStep()} />
        </Stack>
      );
    case 2:
      return (
        <Stack className="step2" horizontalAlign="center">
          <FormattedMessage id={'step2'} />
          <PrimaryButton text={formatMessage({ id: 'next' })} aria-label={formatMessage({ id: 'next' })} onClick={() => nextStep()} />
        </Stack>
      );
    case 3:
      return (
        <Stack className="step3" horizontalAlign="center">
          <Stack.Item>
            <Label style={headerStyle}>{formatMessage({ id: 'onboarding.setupYourGoals' })}</Label>
          </Stack.Item>
          <Stack.Item>
            <TextField label={formatMessage({ id: 'onboarding.productgoal' })} maxLength={300} multiline rows={4} />
          </Stack.Item>
          <Stack.Item>
            <TextField label={formatMessage({ id: 'onboarding.productKpi' })} maxLength={60} />
          </Stack.Item>
          <Stack.Item>
            <PrimaryButton
              text={formatMessage({ id: 'onboarding.getstarted' })}
              aria-label={formatMessage({ id: 'onboarding.getstarted' })}
              onClick={() => nextStep()}
            />
          </Stack.Item>
        </Stack>
      );
    case 4:
      return (
        <Stack horizontalAlign="center">
          <h1>To Do: send to home</h1>
        </Stack>
      );
    default:
      return (
        <Stack className="badStep" horizontalAlign="center">
          <FormattedMessage id="error.erroroccurred" />
        </Stack>
      );
  }
};

export default OnboardingInfo;
