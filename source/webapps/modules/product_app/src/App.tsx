import { Label, PrimaryButton } from '@fluentui/react';
import { AppProps } from '@sopheon/shell-api';
import React from 'react';
import { useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';
import OnboardingInfo from './onboardingInfo';

export type Props = AppProps<AppStateProps, AppDispatchProps>;

const App: React.FunctionComponent<Props> = ({ currentStep, nextStep }: Props) => {
  const { formatMessage } = useIntl();

  return (
    <div>
      <Label>
        <OnboardingInfo currentStep={currentStep} nextStep={nextStep} />
        <PrimaryButton
          text={currentStep === 2 ? formatMessage({ id: 'continue' }) : formatMessage({ id: 'getStarted' })}
          aria-label={currentStep === 2 ? formatMessage({ id: 'continue' }) : formatMessage({ id: 'getStarted' })}
          onClick={() => nextStep()}
        />
      </Label>
    </div>
  );
};

export default App;
