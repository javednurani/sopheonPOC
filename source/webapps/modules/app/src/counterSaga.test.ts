import { CallEffect, delay, ForkEffect, put, PutEffect, takeLatest } from 'redux-saga/effects';

import { CounterSagaActionTypes, incrementCounterByAmount } from './counterReducer';
import counterSaga, { incrementCounter } from './counterSaga';

describe('IncrementCounterSaga', () => {
  test('Steps yield expected Effects', () => {
    const gen: Generator<CallEffect<true> | PutEffect<{ payload: number; type: string }>, void, unknown> = incrementCounter();
    expect(gen.next().value).toEqual(delay(2500));
    expect(gen.next().value).toEqual(put(incrementCounterByAmount(5)));
    expect(gen.next().done).toEqual(true);
  });
});

describe('RootSaga', () => {
  test('RootSaga Action-ChildSaga mappings are correct', () => {
    const gen: Generator<ForkEffect<never>, void, unknown> = counterSaga();
    expect(gen.next().value).toEqual(takeLatest(CounterSagaActionTypes.INCREMENT_ASYNC, incrementCounter));
    expect(gen.next().done).toEqual(true);
  });
});
