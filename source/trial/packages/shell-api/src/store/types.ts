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
