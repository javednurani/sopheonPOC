import { all, fork } from 'redux-saga/effects';

import onboardingSaga from './onboardingSaga';

// eslint-disable-next-line @typescript-eslint/explicit-module-boundary-types
export default function* rootSaga() {
  yield all([fork(onboardingSaga)]);
}
