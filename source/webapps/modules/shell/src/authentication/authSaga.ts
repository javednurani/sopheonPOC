import { AuthSagaActionTypes } from '@sopheon/shell-api';
import { all, call, fork, put, takeEvery } from 'redux-saga/effects';

import { getAccessToken } from './authHelpers';
import { setAccessToken } from './authReducer';

export function* watchOnGetAccessToken() {
  yield takeEvery(AuthSagaActionTypes.GET_ACCESS_TOKEN, onGetAccessToken);
}

export function* onGetAccessToken() {
  const accessToken = yield call(getAccessToken);
  yield put(setAccessToken(accessToken));
}

export default function* authSaga(): Generator {
  yield all([fork(watchOnGetAccessToken)]);
}
