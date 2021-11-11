import { constants, messages } from '@sopheon/shared-ui';
import { InjectReducerMap, InjectSagaMap, IShellApi } from '@sopheon/shell-api';
import React, { ComponentType, useEffect } from 'react';
import { connect, Provider } from 'react-redux';
import { CombinedState, combineReducers, Reducer, ReducersMapObject, Store } from 'redux';
import { Saga } from 'redux-saga';

import { setEnvironmentKey } from './authentication/authReducer';
import { shell } from './rootReducer';
import { sagaMiddleware, store as mainShellStore } from './store';
import { changeTheme } from './themes/themeReducer/themeReducer';
import { LanguageShape, State } from './types';

// DEFAULT LANGUAGE STATE

const initialLanguageState: LanguageShape = {
  direction: constants.LOCALE_DIR_LTR,
  locale: 'en',
  messages: messages.en,
};

// REDUCER INJECTION HELPERS / UTILITY VARIABLES

const asyncReducers: ReducersMapObject = {};

const createRootReducer = (): Reducer => {
  const combinedReducers = {
    shell,
    ...asyncReducers,
  };

  return combineReducers<CombinedState<State>>(combinedReducers);
};

// SAGA INJECTION HELPERS / UTILITY VARIABLES

const asyncSagas: { [key: string]: Saga } = {};

export class ShellApi implements IShellApi {
  private readonly store: Store<State>;

  constructor(store: Store<State>) {
    this.store = store;
  }

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
    // INFO: using useEffect outside of a Function Component (though it is ultimately executed inside of a FC)
    // eslint-disable-next-line react-hooks/rules-of-hooks
    useEffect(() => {
      if (reducer) {
        this.injectReducer(reducer);
      }

      if (saga) {
        this.injectSaga(saga);
      }
    }, [reducer, saga]); // INFO: we don't think the dependencies have any effect?

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
      ...(mapStateProps && mapStateProps(this.store.getState() as unknown as TState)),
      environmentKey: this.store.getState().shell.auth.environmentKey,
      // the below stateProps are not exposed to MFE's via IShellApi. only used by, and private to, the shell
      theme: this.store.getState().shell.theme,
      language: initialLanguageState,
    });

    const mapDispatch = {
      ...(mapDispatchProps && mapDispatchProps(this.store.getState() as unknown as TState)),
      // the below dispatchProps are not exposed to MFE's via IShellApi. only used by, and private to, the shell
      changeTheme: (useDarkTheme: boolean) => changeTheme(useDarkTheme),
      setEnvironmentKey: (environmentKey: string) => setEnvironmentKey(environmentKey),
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
    if (!(injected.key in asyncSagas)) {
      asyncSagas[injected.key] = injected.saga;
      // set up saga listeners
      sagaMiddleware.run(injected.saga);
    }
  }
}

export const shellApi = new ShellApi(mainShellStore);
