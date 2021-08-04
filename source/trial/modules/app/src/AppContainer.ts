import { InjectReducerMap, InjectSagaMap, ShellApiProps } from '@sopheon/shell-api';
import { FunctionComponent } from 'react';

import App from './App';
import {
  decrementCounter,
  DecrementCounterAction,
  incrementCounter,
  IncrementCounterAction,
  incrementCounterAsync,
  IncrementCounterAsyncAction,
} from './counterReducer';
import { NAMESPACE, rootReducer, RootState } from './rootReducer';
import rootSaga from './rootSaga';

export type AppStateProps = {
  counterValue: number;
};

export type AppDispatchProps = {
  incrementCounter: () => IncrementCounterAction;
  decrementCounter: () => DecrementCounterAction;
  incrementCounterAsync: () => IncrementCounterAsyncAction;
};

const AppContainer: FunctionComponent<ShellApiProps> = ({ shellApi }: ShellApiProps) => {
  const mapAppStateProps = (state: RootState): AppStateProps => ({
    counterValue: state[NAMESPACE] ? state[NAMESPACE].counter.value : 0,
  });

  const mapAppDispatchProps = (state: RootState): AppDispatchProps => ({
    incrementCounter: () => incrementCounter(),
    decrementCounter: () => decrementCounter(),
    incrementCounterAsync: () => incrementCounterAsync(),
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
