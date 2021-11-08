import { Label, PrimaryButton } from '@fluentui/react';
import { AppProps } from '@sopheon/shell-api';
import React from 'react';
import { FormattedMessage, useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';
import OnboardingInfo from './onboardingInfo';

export type Props = AppProps<AppStateProps, AppDispatchProps>;

const App: React.FunctionComponent<Props> = ({ currentStep, nextStep }: Props) => {
  const { formatMessage } = useIntl();

  return (
    <div>
      <Label>
        <FormattedMessage id={'app.welcome'} />
        <OnboardingInfo currentStep={currentStep} nextStep={nextStep} />
        <PrimaryButton text={formatMessage({ id: 'next' })} aria-label={formatMessage({ id: 'next' })} onClick={() => nextStep()} />
      </Label>
    </div>
  );
};

export default App;
