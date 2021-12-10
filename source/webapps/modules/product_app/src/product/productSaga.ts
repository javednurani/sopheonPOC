import { all, call, fork, put, takeEvery } from 'redux-saga/effects';

import { Attributes, Product, ProductItemTypes, ToDoItem } from '../types';
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

const productItemsToTasksHelper = (productItems: unknown[]): ToDoItem[] => {
  const todos = productItems.filter(pi => pi.productItemTypeId === ProductItemTypes.TASK);

  const typedTodos: ToDoItem[] = todos.map(td => ({
    name: td.name,
    notes: td.stringAttributeValues.filter(sav => sav.attributeId === Attributes.NOTES)[0].value,
    dueDate: new Date(td.utcDateTimeAttributeValues.filter(dtav => dtav.attributeId === Attributes.DUEDATE)[0].value),
    status: td.enumCollectionAttributeValues.filter(ecav => ecav.attributeId === Attributes.STATUS)[0].value[0].enumAttributeOptionId,
  }));

  return typedTodos;
};

export function* onGetProducts(action: GetProductsAction): Generator {
  try {
    yield put(getProductsRequest());
    const { data } = yield call(getProducts, action.payload);

    const transformedProductsData: Product[] = data.map(d => ({
      id: d.id,
      key: d.key,
      name: d.name,
      industries: d.int32AttributeValues.filter(iav => iav.attributeId === Attributes.INDUSTRIES).map(iav => iav.value),
      kpis: d.keyPerformanceIndicators,
      goals: d.goals,
      todos: productItemsToTasksHelper(d.items)
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
      industries: data.int32AttributeValues.filter(iav => iav.attributeId === Attributes.INDUSTRIES).map(iav => iav.value),
      goals: data.goals,
      kpis: data.keyPerformanceIndicators,
      todos: productItemsToTasksHelper(data.items)
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
      industries: data.int32AttributeValues.filter(iav => iav.attributeId === Attributes.INDUSTRIES).map(iav => iav.value),
      kpis: data.keyPerformanceIndicators,
      goals: data.goals,
      todos: productItemsToTasksHelper(data.items)
    };


    yield put(updateProductSuccess(updatedProduct));
  } catch (error) {
    yield put(updateProductFailure(error));
  }
}


export default function* productSaga(): Generator {
  yield all([fork(watchOnGetProducts), fork(watchOnCreateProduct), fork(watchOnUpdateProduct)]);
}
