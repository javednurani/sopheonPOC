import { counterReducer, CounterStateShape, decrementCounter, incrementCounter, incrementCounterByAmount, initialState } from './counterReducer';

describe('CounterReducer', () => {
  test('increment', () => {
    const nextState: CounterStateShape = counterReducer(initialState, incrementCounter());
    expect(nextState.value).toEqual(1);
  });

  test('decrement', () => {
    const nextState: CounterStateShape = counterReducer(initialState, decrementCounter());
    expect(nextState.value).toEqual(-1);
  });

  test('incrementByAmount', () => {
    const addThis = 7;
    const nextState: CounterStateShape = counterReducer(initialState, incrementCounterByAmount(addThis));
    expect(nextState.value).toEqual(addThis);
  });
});
