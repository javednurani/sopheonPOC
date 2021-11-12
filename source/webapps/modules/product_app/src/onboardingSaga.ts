import { all, call, fork, put, takeEvery } from 'redux-saga/effects';

// eslint-disable-next-line max-len
import {
  CreateProductAction,
  createProductFailure,
  createProductRequest,
  createProductSuccess,
  getProductsAction,
  getProductsFailure,
  getProductsRequest,
  getProductsSuccess,
  nextStep,
  OnboardingSagaActionTypes,
  UpdateProductAction,
  updateProductFailure,
  updateProductRequest,
  updateProductSuccess,
} from './onboardingInfoReducer';
import { createProduct, getProducts, updateProduct } from './onboardingService';

export function* watchOnCreateProduct(): Generator {
  yield takeEvery(OnboardingSagaActionTypes.CREATE_PRODUCT, onCreateProduct);
}

export function* watchOnUpdateProduct(): Generator {
  yield takeEvery(OnboardingSagaActionTypes.UPDATE_PRODUCT, onUpdateProduct);
}

export function* watchOnGetProduct(): Generator {
  yield takeEvery(OnboardingSagaActionTypes.GET_PRODUCTS, onGetProducts);
}

export function* onCreateProduct(action: CreateProductAction): Generator {
  try {
    yield put(nextStep());
    yield put(createProductRequest());
    //@ts-ignore TODO Cloud-1920, fix this ignore and a console error
    const { data } = yield call(createProduct(action.payload));
    yield put(createProductSuccess(data));
  } catch (error) {
    yield put(createProductFailure(error));
  }
}

export function* onUpdateProduct(action: UpdateProductAction): Generator {
  try {
    yield put(nextStep());
    yield put(updateProductRequest());
    //@ts-ignore TODO Cloud-1920, fix this ignore and a console error
    const { data } = yield call(updateProduct(action.payload));
    yield put(updateProductSuccess(data));
  } catch (error) {
    yield put(updateProductFailure(error));
  }
}

export function* onGetProducts(action: getProductsAction): Generator {
  try {
    yield put(getProductsRequest());
    //@ts-ignore TODO Cloud-1920, fix this ignore and a console error
    const { data } = yield call(getProducts(action.payload));
    yield put(getProductsSuccess(data));
  } catch (error) {
    yield put(getProductsFailure(error));
  }
}

export default function* onboardingSaga(): Generator {
  yield all([fork(watchOnCreateProduct), fork(watchOnUpdateProduct)]);
}
