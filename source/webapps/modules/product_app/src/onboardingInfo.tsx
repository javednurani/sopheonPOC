import { ProgressIndicator, Stack } from '@fluentui/react';
import React, { CSSProperties } from 'react';
import { FormattedMessage } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';

export type OnboardingInfoProps = AppStateProps & AppDispatchProps;

const OnboardingInfo: React.FunctionComponent<OnboardingInfoProps> = ({ currentStep, nextStep }: OnboardingInfoProps) => {
  const progressBarStyles: CSSProperties = {
    padding: '0vh 25vw 0vh 25vw',
  };

  switch (currentStep) {
    case 2:
      return (
        <Stack className="step2">
          <Stack.Item align={'center'}>
            <FormattedMessage id={'step2'} />
          </Stack.Item>
          <Stack.Item align={'auto'} style={progressBarStyles}>
            <ProgressIndicator
              label={'Step 2 of 3: Setup your Product'}
              description={'Next: Setup your Goals'}
              ariaValueText={'Step 2 of 3: Setup your Product'}
              percentComplete={0.68}
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
