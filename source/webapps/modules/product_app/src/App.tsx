import { Label } from '@fluentui/react';
import { AppProps } from '@sopheon/shell-api';
import React from 'react';
import { FormattedMessage } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';
import OnboardingInfo from './onboardingInfo';

export type Props = AppProps<AppStateProps, AppDispatchProps>;

const App: React.FunctionComponent<Props> = ({ currentStep, nextStep }: Props) => (
  <div>
    <Label>
      <FormattedMessage id={'app.welcome'} />
      <OnboardingInfo currentStep={currentStep} nextStep={nextStep} />
    </Label>
  </div>
);

export default App;
