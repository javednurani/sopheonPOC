import { Label, ProgressIndicator, Stack, TextField } from '@fluentui/react';
import React, { CSSProperties } from 'react';
import { FormattedMessage, useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';

export type OnboardingInfoProps = AppStateProps & AppDispatchProps;

const OnboardingInfo: React.FunctionComponent<OnboardingInfoProps> = ({ currentStep, nextStep }: OnboardingInfoProps) => {
  const { formatMessage } = useIntl();
  const headerStyle: React.CSSProperties = {
    // TODO: Use header styles from cloud-1583 at merge
  };
  const progressBarStyles: CSSProperties = {
    padding: '0vh 25vw 0vh 25vw',
  };

  switch (currentStep) {
    case 2:
      return (
        <Stack className="step2" horizontalAlign="center">
          <Stack.Item>
            <Label style={headerStyle}>{formatMessage({ id: 'onboarding.setupproduct' })}</Label>
          </Stack.Item>
          <Stack.Item>
            <TextField
              label={formatMessage({ id: 'onboarding.yourproductname' })}
              aria-label={formatMessage({ id: 'onboarding.yourproductname' })}
              required
              maxLength={300}
            />
          </Stack.Item>
          <Stack.Item align={'auto'} style={progressBarStyles}>
            <ProgressIndicator
              label={formatMessage({ id: 'onboarding.step2of3' })}
              description={formatMessage({ id: 'onboarding.nextGoals' })}
              ariaValueText={formatMessage({ id: 'onboarding.step2of3' })}
              percentComplete={0.67}
              barHeight={12}
            />
          </Stack.Item>
        </Stack>
      );
    case 3:
      return (
        <Stack className="step3" horizontalAlign="center">
          <FormattedMessage id={'step3'} />
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
