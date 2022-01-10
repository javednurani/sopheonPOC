import React, { ComponentType, useEffect } from 'react';
import { connect, InferableComponentEnhancerWithProps, Provider } from 'react-redux';
import { Action as ReduxAction, combineReducers, createStore, Reducer, ReducersMapObject, Store } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import { Saga } from 'redux-saga';

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// THIS IS A TEMPORARY FILE TO SHARE FILES WITH THE TEMPLATE APP
// THESE FILES ARE NORMALLY ACCESSIBLE IN THE @sopheon/shell-api PACKAGE
// ONCE THIS APP IS ADDED TO A SHELL WITH THAT PACKAGE
// THIS FILE SHOULD BE DELETED AND IMPORTS REPLACED WITH THE PACKAGE
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

export type Action<Type, Meta = void> = ReduxAction<Type> & { meta?: Meta };
export type PayloadAction<Type, Payload, Meta = void> = Action<Type, Meta> & { readonly payload: Payload };

export const createAction = <Type extends string, Meta>(type: Type, meta?: Meta): Action<Type, Meta> => ({
  type,
  meta,
});

export const createPayloadAction = <Type extends string, Payload, Meta>(
  type: Type,
  payload: Payload,
  meta?: Meta
): PayloadAction<Type, Payload, Meta> => ({
  ...createAction(type, meta),
  payload,
});

export type InjectReducerMap = {
  key: string;
  reducer: Reducer;
};

export type InjectSagaMap = {
  key: string;
  saga: Saga;
};

export type AppProps<TStateProps, TDispatchProps> = {
  // expose main-shell concerns, as state or dispatch (read or write), here
} & TStateProps &
  TDispatchProps;

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

// --------------------------------------
// ISOLATED SHELL API

//#region IsolatedShellAPI

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
    }, []);

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

//#endregion
