import { Action as ReduxAction, Reducer } from 'redux';
import { Saga } from 'redux-saga';

export type Action<Type, Meta = void> = ReduxAction<Type> & { meta?: Meta };
export type PayloadAction<Type, Payload, Meta = void> = Action<Type, Meta> & { readonly payload: Payload };

export type InjectReducerMap = {
  key: string;
  reducer: Reducer;
};

export type InjectSagaMap = {
  key: string;
  saga: Saga;
};

// eslint-disable-next-line no-shadow
export enum FetchStatus {
  NotActive = 1,
  InProgress,
  DoneFailure,
  DoneSuccess,
}

export * from './auth/types';
