import { CreateProductAction, OnboardingSagaActionTypes, UpdateProductAction } from '@sopheon/shell-api';
import { all, call, fork, put, takeEvery } from 'redux-saga/effects';

// eslint-disable-next-line max-len
import {
  createProductFailure,
  createProductRequest,
  createProductSuccess,
  nextStep,
  updateProductFailure,
  updateProductRequest,
  updateProductSuccess,
} from './onboardingInfoReducer';
import { createProduct, updateProduct } from './onboardingService';

export function* watchOnCreateProduct(): Generator {
  yield takeEvery(OnboardingSagaActionTypes.CREATE_PRODUCT, onCreateProduct);
}

export function* watchOnUpdateProduct(): Generator {
  yield takeEvery(OnboardingSagaActionTypes.UPDATE_PRODUCT, onUpdateProduct);
}

export function* onCreateProduct(action: CreateProductAction): Generator {
  try {
    yield put(nextStep());
    yield put(createProductRequest());
    const { data } = yield call(createProduct, action.payload);
    yield put(createProductSuccess(data));
  } catch (error) {
    yield put(createProductFailure(error));
  }
}

export function* onUpdateProduct(action: UpdateProductAction): Generator {
  try {
    yield put(nextStep());
    yield put(updateProductRequest());
    const { data } = yield call(updateProduct, action.payload);
    yield put(updateProductSuccess(data));
  } catch (error) {
    yield put(updateProductFailure(error));
  }
}

export default function* onboardingSaga(): Generator {
  yield all([fork(watchOnCreateProduct), fork(watchOnUpdateProduct)]);
}
