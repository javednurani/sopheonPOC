import { GetProductsAction, OnboardingSagaActionTypes } from '@sopheon/shell-api';
import { all, call, fork, put, takeEvery } from 'redux-saga/effects';

// eslint-disable-next-line max-len
import {
  getProductsFailure,
  getProductsRequest,
  getProductsSuccess
} from './productReducer';
import { getProducts } from './productService';

export function* watchOnGetProduct(): Generator {
  yield takeEvery(OnboardingSagaActionTypes.GET_PRODUCTS, onGetProducts);
}

export function* onGetProducts(action: GetProductsAction): Generator {
  try {
    yield put(getProductsRequest());
    const { data } = yield call(getProducts, action.payload);
    yield put(getProductsSuccess(data));
  } catch (error) {
    yield put(getProductsFailure(error));
  }
}

export default function* productSaga(): Generator {
  yield all([fork(watchOnGetProduct)]);
}
