import { Action, createAction, createPayloadAction, PayloadAction } from '@sopheon/shell-api';
import { Reducer } from 'redux';

//#region  Action Types

// REDUCER ACTION TYPES

// eslint-disable-next-line no-shadow
enum CounterActionTypes {
  INCREMENT_COUNTER = 'APP/INCREMENT_COUNTER',
  DECREMENT_COUNTER = 'APP/DECREMENT_COUNTER',
  INCREMENT_COUNTER_BY_AMOUNT = 'APP/INCREMENT_COUNTER_BY_AMOUNT',
}

export type IncrementCounterAction = Action<CounterActionTypes.INCREMENT_COUNTER>;
export type DecrementCounterAction = Action<CounterActionTypes.DECREMENT_COUNTER>;
type IncrementCounterByAmountAction = PayloadAction<CounterActionTypes.INCREMENT_COUNTER_BY_AMOUNT, number>;

export type CounterReducerActions = IncrementCounterAction | DecrementCounterAction | IncrementCounterByAmountAction;

// SAGA ACTION TYPES

// eslint-disable-next-line no-shadow
export enum CounterSagaActionTypes {
  INCREMENT_ASYNC = 'APP/INCREMENT_ASYNC',
}

export type IncrementCounterAsyncAction = Action<CounterSagaActionTypes.INCREMENT_ASYNC>;
//#endregion

//#region  Action Creators

// REDUCER ACTIONS

export const incrementCounter = (): IncrementCounterAction => createAction(CounterActionTypes.INCREMENT_COUNTER);
export const decrementCounter = (): DecrementCounterAction => createAction(CounterActionTypes.DECREMENT_COUNTER);
export const incrementCounterByAmount = (amount: number): IncrementCounterByAmountAction =>
  createPayloadAction(CounterActionTypes.INCREMENT_COUNTER_BY_AMOUNT, amount);

// SAGA ACTIONS

export const incrementCounterAsync = (): IncrementCounterAsyncAction => createAction(CounterSagaActionTypes.INCREMENT_ASYNC);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export type CounterStateShape = {
  value: number;
};

export const initialState: CounterStateShape = {
  value: 0,
};

// HANDLERS

const setValue = (state: CounterStateShape, valueToSet: number): CounterStateShape => ({
  ...state,
  value: valueToSet,
});

// ACTION SWITCH

export const counterReducer: Reducer<CounterStateShape, CounterReducerActions> = (state = initialState, action) => {
  switch (action.type) {
    case CounterActionTypes.INCREMENT_COUNTER:
      return setValue(state, state.value + 1);
    case CounterActionTypes.DECREMENT_COUNTER:
      return setValue(state, state.value - 1);
    case CounterActionTypes.INCREMENT_COUNTER_BY_AMOUNT:
      return setValue(state, state.value + action.payload);
    default:
      return state;
  }
};

//#endregion
