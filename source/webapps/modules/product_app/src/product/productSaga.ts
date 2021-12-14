import { all, call, fork, put, takeEvery } from 'redux-saga/effects';

import { Attributes, Product } from '../types';
// eslint-disable-next-line max-len
import {
  CreateProductAction,
  createProductFailure,
  createProductRequest,
  createProductSuccess,
  GetProductsAction,
  getProductsFailure,
  getProductsRequest,
  getProductsSuccess,
  ProductSagaActionTypes,
  UpdateProductAction,
  updateProductFailure,
  updateProductRequest,
  updateProductSuccess
} from './productReducer';
import { createProduct, getProducts, updateProduct } from './productService';

export function* watchOnGetProducts(): Generator {
  yield takeEvery(ProductSagaActionTypes.GET_PRODUCTS, onGetProducts);
}

export function* onGetProducts(action: GetProductsAction): Generator {
  try {
    yield put(getProductsRequest());
    const { data } = yield call(getProducts, action.payload);

    const transformedProductsData = data.map(d => ({
      id: d.id,
      key: d.key,
      name: d.name,
      industries: d.intAttributeValues.filter(iav => iav.attributeId === Attributes.INDUSTRIES).map(iav => iav.value),
      kpis: d.keyPerformanceIndicators,
      goals: d.goals
    }));
    yield put(getProductsSuccess(transformedProductsData));
  } catch (error) {
    yield put(getProductsFailure(error));
  }
}

export function* watchOnCreateProduct(): Generator {
  yield takeEvery(ProductSagaActionTypes.CREATE_PRODUCT, onCreateProduct);
}

export function* onCreateProduct(action: CreateProductAction): Generator {
  try {
    yield put(createProductRequest());
    const { data } = yield call(createProduct, action.payload);

    const createdProduct: Product = {
      id: data.id,
      key: data.key,
      name: data.name,
      industries: data.intAttributeValues.filter(iav => iav.attributeId === Attributes.INDUSTRIES).map(iav => iav.value),
      goals: [],
      kpis: [],
    };

    yield put(createProductSuccess(createdProduct));
  } catch (error) {
    yield put(createProductFailure(error));
  }
}

export function* watchOnUpdateProduct(): Generator {
  yield takeEvery(ProductSagaActionTypes.UPDATE_PRODUCT, onUpdateProduct);
}

export function* onUpdateProduct(action: UpdateProductAction): Generator {
  try {
    yield put(updateProductRequest());
    const { data } = yield call(updateProduct, action.payload);

    const updatedProduct: Product = {
      id: data.id,
      key: data.key,
      name: data.name,
      industries: data.intAttributeValues.filter(iav => iav.attributeId === Attributes.INDUSTRIES).map(iav => iav.value),
      kpis: data.keyPerformanceIndicators,
      goals: data.goals
    };


    yield put(updateProductSuccess(updatedProduct));
  } catch (error) {
    yield put(updateProductFailure(error));
  }
}


export default function* productSaga(): Generator {
  yield all([fork(watchOnGetProducts), fork(watchOnCreateProduct), fork(watchOnUpdateProduct)]);
}
