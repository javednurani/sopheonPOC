import { delay, put, takeLatest } from 'redux-saga/effects';

import { CounterSagaActionTypes, incrementCounterByAmount } from './counterReducer';

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export function* incrementCounter() {
  yield delay(2500); // emulate async api call
  yield put(incrementCounterByAmount(5));
}

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export default function* counterSaga() {
  yield takeLatest(CounterSagaActionTypes.INCREMENT_ASYNC, incrementCounter);
}
