import { all, call, fork, put, takeEvery } from 'redux-saga/effects';

import { Attributes } from '../data/attributes';
import { ProductItemTypes } from '../data/productItemTypes';
import { Product, ToDoItem } from '../types';
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
  UpdateProductItemAction,
  updateProductItemFailure,
  updateProductItemRequest,
  updateProductItemSuccess,
  updateProductRequest,
  updateProductSuccess,
} from './productReducer';
import { createProduct, getProducts, updateProduct, updateProductItem } from './productService';

export function* watchOnGetProducts(): Generator {
  yield takeEvery(ProductSagaActionTypes.GET_PRODUCTS, onGetProducts);
}

const translateProductItemsToTasks = (productItems: unknown[]): ToDoItem[] =>
  productItems.filter(pi => pi.productItemTypeId === ProductItemTypes.TASK).map(td => translateProductItemToTask(td));

const translateProductItemToTask = (td: unknown): ToDoItem => {
  const dueDateString: string = td.utcDateTimeAttributeValues.filter(dtav => dtav.attributeId === Attributes.DUEDATE)[0].value;
  return {
    id: td.id,
    name: td.name,
    notes: td.stringAttributeValues.filter(sav => sav.attributeId === Attributes.NOTES)[0].value,
    dueDate: dueDateString ? new Date(dueDateString) : null,
    status: td.enumAttributeValues.filter(eav => eav.attributeId === Attributes.STATUS)[0].enumAttributeOptionId,
  };
};

const translateEnumCollectionAttributeValuesToIndustryIds = (enumCollectionAttributeValues: unknown[]): number[] =>
  enumCollectionAttributeValues.find(ecav => ecav.attributeId === Attributes.INDUSTRIES).value.map(val => val.enumAttributeOptionId);

export function* onGetProducts(action: GetProductsAction): Generator {
  try {
    yield put(getProductsRequest());
    const { data } = yield call(getProducts, action.payload); // TODO , type response

    const transformedProductsData: Product[] = data.map(d => ({
      id: d.id,
      key: d.key,
      name: d.name,
      industries: translateEnumCollectionAttributeValuesToIndustryIds(d.enumCollectionAttributeValues),
      kpis: d.keyPerformanceIndicators,
      goals: d.goals,
      todos: translateProductItemsToTasks(d.items),
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
    const { data } = yield call(createProduct, action.payload); // TODO , type response

    const createdProduct: Product = {
      id: data.id,
      key: data.key,
      name: data.name,
      industries: translateEnumCollectionAttributeValuesToIndustryIds(data.enumCollectionAttributeValues),
      goals: data.goals,
      kpis: data.keyPerformanceIndicators,
      todos: translateProductItemsToTasks(data.items),
    };

    yield put(createProductSuccess(createdProduct));
  } catch (error) {
    yield put(createProductFailure(error));
  }
}

export function* watchOnUpdateProduct(): Generator {
  yield takeEvery(ProductSagaActionTypes.UPDATE_PRODUCT, onUpdateProduct);
}

export function* watchOnUpdateProductItem(): Generator {
  yield takeEvery(ProductSagaActionTypes.UPDATE_PRODUCT_ITEM, onUpdateProductItem);
}

export function* onUpdateProduct(action: UpdateProductAction): Generator {
  try {
    yield put(updateProductRequest());
    const { data } = yield call(updateProduct, action.payload); // TODO , type response

    const updatedProduct: Product = {
      id: data.id,
      key: data.key,
      name: data.name,
      industries: translateEnumCollectionAttributeValuesToIndustryIds(data.enumCollectionAttributeValues),
      kpis: data.keyPerformanceIndicators,
      goals: data.goals,
      todos: translateProductItemsToTasks(data.items),
    };

    yield put(updateProductSuccess(updatedProduct));
  } catch (error) {
    yield put(updateProductFailure(error));
  }
}

export function* onUpdateProductItem(action: UpdateProductItemAction): Generator {
  try {
    yield put(updateProductItemRequest());
    const { data } = yield call(updateProductItem, action.payload); // TODO , type response
    const updatedProductItem = translateProductItemToTask(data);
    yield put(updateProductItemSuccess(updatedProductItem));
  } catch (error) {
    yield put(updateProductItemFailure(error));
  }
}

export default function* productSaga(): Generator {
  yield all([fork(watchOnGetProducts), fork(watchOnCreateProduct), fork(watchOnUpdateProduct), fork(watchOnUpdateProductItem)]);
}
