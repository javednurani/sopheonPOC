import React, { ComponentType, useEffect } from 'react';
import { connect, Provider } from 'react-redux';
import { combineReducers, createStore, Reducer, ReducersMapObject, Store } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import { Saga } from 'redux-saga';

import { IShellApi } from './IShellApi';
import { InjectReducerMap, InjectSagaMap } from './store/types';

// REDUCER INJECTION HELPERS / UTILITY VARIABLES

const asyncReducers: ReducersMapObject = {};

const createRootReducer = (): Reducer => {
  const combinedReducers = {
    /* INFO: shared state would go here */
    ...asyncReducers,
  };

  return combineReducers(combinedReducers);
};

// ISOLATED STORE CREATION
// this is only used when an MFE without a store (aka, a "presentational" MFE) needs a shellApi wrapper
// most connected MFE's that manage state will pass their store in to the IsolatedShellApi constructor

const createIsolatedStore = () => createStore(createRootReducer(), composeWithDevTools());

// SAGA INJECTION HELPERS / UTILITY VARIABLES

const asyncSagas: { [key: string]: Saga } = {};

export class IsolatedShellApi implements IShellApi {
  private readonly store: Store;

  constructor(store?: Store) {
    this.store = store || createIsolatedStore();
  }
  environmentKey: string;

  get getStore(): Store {
    return this.store;
  }

  connectApp<TAppProps, TState, TStateProps, TDispatchProps>(
    App: ComponentType<TAppProps>,
    reducer?: InjectReducerMap,
    saga?: InjectSagaMap,
    mapStateProps?: (state: TState) => TStateProps,
    mapDispatchProps?: (state: TState) => TDispatchProps
  ): JSX.Element {
    // INFO - using useEffect outside of a Function Component (though it is ultimately executed inside of a FC)
    // eslint-disable-next-line react-hooks/rules-of-hooks
    useEffect(() => {
      if (reducer) {
        this.injectReducer(reducer);
      } else {
        this.store.replaceReducer(createRootReducer());
      }

      if (saga) {
        this.injectSaga(saga);
      }
    }, [reducer, saga]); // INFO: we don't think the dependencies have any effect

    const connector = this.getConnector(mapStateProps, mapDispatchProps);
    //@ts-ignore
    const AppContainer = connector(App);

    const AppWrapper = (
      <Provider store={this.store}>
        {/*
          // @ts-ignore */}
        <AppContainer />
      </Provider>
    );

    return AppWrapper;
  }

  // eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
  getConnector<TState, TStateProps, TDispatchProps>(
    mapStateProps?: (state: TState) => TStateProps,
    mapDispatchProps?: (state: TState) => TDispatchProps
  ) {
    const mapState = () => ({
      ...(mapStateProps && mapStateProps(this.store.getState() as TState)),
      // stub out Shell-provided state (found in AppProps) here
      environmentKey: 'ISOLATED_SHELL_API_ENVIRONMENTKEY_STUB',
    });

    // INFO: some of these action creators are duplicated in main-shell reducers, could possibly consolidate
    const mapDispatch = {
      ...(mapDispatchProps && mapDispatchProps(this.store.getState() as TState)),
      // stub out Shell-provided dispatch (found in AppProps) here
    };

    return connect(mapState, mapDispatch);
  }

  injectReducer(injected: InjectReducerMap): void {
    if (!(injected.key in asyncReducers)) {
      asyncReducers[injected.key] = injected.reducer;
      this.store.replaceReducer(createRootReducer());
    }
  }

  injectSaga(injected: InjectSagaMap): void {
    // Isolated MFE's will run their own sagas, in their own store file
    // this function is just to mock out injectSaga's asyncSaga dictionary behavior
    if (!(injected.key in asyncSagas)) {
      asyncSagas[injected.key] = injected.saga;
    }
  }
}
