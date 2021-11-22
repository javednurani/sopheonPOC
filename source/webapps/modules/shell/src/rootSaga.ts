import { all, fork } from 'redux-saga/effects';

import authSaga from './authentication/authSaga';

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export default function* rootSaga() {
  yield all([fork(authSaga)]);
}
