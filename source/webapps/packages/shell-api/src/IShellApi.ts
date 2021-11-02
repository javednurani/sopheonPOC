import { ComponentType } from 'react';
import { InferableComponentEnhancerWithProps } from 'react-redux';
import { Store } from 'redux';

import { InjectReducerMap, InjectSagaMap } from './store/types';

export type AppProps<TStateProps, TDispatchProps> = {
  // expose main-shell concerns, as state or dispatch (read or write), here
} & TStateProps &
  TDispatchProps; // include StateProps and DispatchProps handed to getConnector

export type ShellApiProps = {
  shellApi: IShellApi;
};

export interface IShellApi {
  getStore: Store;
  // wraps an App Component in HOC connected to store
  connectApp<TAppProps, TState, TStateProps, TDispatchProps>(
    App: ComponentType<TAppProps>,
    reducer?: InjectReducerMap,
    saga?: InjectSagaMap,
    mapStateProps?: (state: TState) => TStateProps,
    mapDispatchProps?: (state: TState) => TDispatchProps
  ): JSX.Element;

  // returns a Component Enhancer (connect), for HOC wrapping
  getConnector<TState, TStateProps, TDispatchProps>(
    mapStateProps?: (state: TState) => TStateProps,
    mapDispatchProps?: (state: TState) => TDispatchProps
    // eslint-disable-next-line @typescript-eslint/ban-types
  ): InferableComponentEnhancerWithProps<AppProps<TStateProps, TDispatchProps>, {}>;

  // add reducers to existing store
  injectReducer(injected: InjectReducerMap): void;

  // run saga in existing middleware
  injectSaga(injected: InjectSagaMap): void;
}
