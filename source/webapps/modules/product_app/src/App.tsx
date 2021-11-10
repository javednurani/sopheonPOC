import { Label } from '@fluentui/react';
import { AppProps } from '@sopheon/shell-api';
import React from 'react';

import { AppDispatchProps, AppStateProps } from './AppContainer';
import OnboardingInfo from './onboardingInfo';

export type Props = AppProps<AppStateProps, AppDispatchProps>;

const App: React.FunctionComponent<Props> = ({ currentStep, createProduct, updateProduct }: Props) => (
  <div>
    <Label>
      <OnboardingInfo currentStep={currentStep} createProduct={createProduct} updateProduct={updateProduct} />
    </Label>
  </div>
);

export default App;
