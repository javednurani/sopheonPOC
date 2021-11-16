import { all, fork } from 'redux-saga/effects';

import authSaga from './authentication/authSaga';
import productSaga from './product/productSaga';

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export default function* rootSaga() {
  yield all([fork(authSaga), fork(productSaga)]);
}
