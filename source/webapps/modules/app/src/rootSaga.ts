import { all, fork } from 'redux-saga/effects';

import counterSaga from './counterSaga';

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export default function* rootSaga() {
  yield all([fork(counterSaga)]);
}
