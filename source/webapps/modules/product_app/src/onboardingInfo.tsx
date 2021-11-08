import { Label, PrimaryButton, Stack, TextField } from '@fluentui/react';
import React from 'react';
import { FormattedMessage, useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';

export type OnboardingInfoProps = AppStateProps & AppDispatchProps;

const OnboardingInfo: React.FunctionComponent<OnboardingInfoProps> = ({ currentStep, nextStep }: OnboardingInfoProps) => {
  const { formatMessage } = useIntl();
  const headerStyle: React.CSSProperties = {
    // TODO: Use header styles from cloud-1583 at merge
  };

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
          <Label style={headerStyle}>{formatMessage({ id: 'onboarding.setupproduct' })}</Label>
          <Stack.Item>
            <TextField
              label={formatMessage({ id: 'onboarding.yourproductname' })}
              aria-label={formatMessage({ id: 'onboarding.yourproductname' })}
              required
              maxLength={300}
            />
          </Stack.Item>
          <PrimaryButton text={formatMessage({ id: 'next' })} aria-label={formatMessage({ id: 'next' })} onClick={() => nextStep()} />
        </Stack>
      );
    case 3:
      return (
        <Stack className="step3" horizontalAlign="center">
          <FormattedMessage id={'step3'} />
          <PrimaryButton text={formatMessage({ id: 'next' })} aria-label={formatMessage({ id: 'next' })} onClick={() => nextStep()} />
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
