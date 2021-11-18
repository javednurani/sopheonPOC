import { InjectReducerMap, InjectSagaMap, ShellApiProps } from '@sopheon/shell-api';
import { FunctionComponent } from 'react';

import App from './App';
import { nextStep, NextStepAction } from './onboardingInfoReducer';
import { NAMESPACE, rootReducer, RootState } from './rootReducer';
import rootSaga from './rootSaga';

export type AppStateProps = {
  currentStep: number
};

export type AppDispatchProps = { nextStep: () => NextStepAction };

const AppContainer: FunctionComponent<ShellApiProps> = ({ shellApi }: ShellApiProps) => {
  const mapAppStateProps = (state: RootState): AppStateProps => ({
    currentStep: state[NAMESPACE] ? state[NAMESPACE].onboardingInfo.currentStep : 1,
  });

  const mapAppDispatchProps = (state: RootState): AppDispatchProps => ({
    nextStep: () => nextStep(),
  });

  const appReducerMap: InjectReducerMap = {
    key: NAMESPACE,
    reducer: rootReducer,
  };

  const appSagaMap: InjectSagaMap = {
    key: NAMESPACE,
    saga: rootSaga,
  };

  return shellApi.connectApp(App, appReducerMap, appSagaMap, mapAppStateProps, mapAppDispatchProps);
};

export default AppContainer;
