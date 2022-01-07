import { all, call, fork, put, takeEvery } from 'redux-saga/effects';

import { Attributes } from '../data/attributes';
import { Product, ProductScopedMilestone, ProductScopedTask, Task } from '../types';
// eslint-disable-next-line max-len
import {
  CreateMilestoneAction,
  createMilestoneFailure,
  createMilestoneRequest,
  createMilestoneSuccess,
  CreateProductAction,
  createProductFailure,
  createProductRequest,
  createProductSuccess,
  CreateTaskAction,
  createTaskFailure,
  createTaskRequest,
  createTaskSuccess,
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
  UpdateTaskAction,
  updateTaskFailure,
  updateTaskRequest,
  updateTaskSuccess,
} from './productReducer';
import { createMilestone, createProduct, createTask, getProducts, updateProduct, updateProductItem, updateTask } from './productService';

// TRANSLATION HELPERS, TODO, MOVE OUT OF SAGA
const translateEnumCollectionAttributeValuesToIndustryIds = (enumCollectionAttributeValues: unknown[]): number[] =>
  enumCollectionAttributeValues.find(ecav => ecav.attributeId === Attributes.INDUSTRIES).value.map(val => val.enumAttributeOptionId);

// INFO, Task object from service and Task object in SPA are similar, but we at least need the Date() translation...possibly we can reduce this
const translateTasksFromService = (tasks: unknown[]): Task[] =>
  tasks.map(task => ({
    id: task.id,
    name: task.name,
    notes: task.notes,
    dueDate: task.dueDate ? new Date(task.dueDate) : null,
    status: task.status,
  }));

// END TRANSLATION HELPERS

export function* watchOnGetProducts(): Generator {
  yield takeEvery(ProductSagaActionTypes.GET_PRODUCTS, onGetProducts);
}

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
      tasks: translateTasksFromService(d.tasks),
      milestones: d.milestones,
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
      tasks: translateTasksFromService(data.tasks),
      milestones: [],
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
    const { data } = yield call(updateProduct, action.payload); // TODO , type response

    const updatedProduct: Product = {
      id: data.id,
      key: data.key,
      name: data.name,
      industries: translateEnumCollectionAttributeValuesToIndustryIds(data.enumCollectionAttributeValues),
      kpis: data.keyPerformanceIndicators,
      goals: data.goals,
      tasks: translateTasksFromService(data.tasks),
      milestones: [],
    };

    yield put(updateProductSuccess(updatedProduct));
  } catch (error) {
    yield put(updateProductFailure(error));
  }
}

// INFO, updateProductItem pipe was used to add Tasks under old E-A-V pattern. this can be removed.
// some references to obsolete/removed helpers (eg translateProductItemToTask) would need to be reworked.

export function* watchOnUpdateProductItem(): Generator {
  yield takeEvery(ProductSagaActionTypes.UPDATE_PRODUCT_ITEM, onUpdateProductItem);
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

export function* watchOnCreateTask(): Generator {
  yield takeEvery(ProductSagaActionTypes.CREATE_TASK, onCreateTask);
}

export function* onCreateTask(action: CreateTaskAction): Generator {
  try {
    yield put(createTaskRequest());
    const { data } = yield call(createTask, action.payload);
    const createdTask: ProductScopedTask = {
      task: {
        id: data.id,
        name: data.name,
        notes: data.notes,
        dueDate: data.dueDate ? new Date(data.dueDate) : null,
        status: data.status,
      },
      ProductKey: action.payload.ProductKey, // used for assignment to correct Product in Redux store
    };
    yield put(createTaskSuccess(createdTask));
  } catch (error) {
    yield put(createTaskFailure(error));
  }
}

export function* watchOnUpdateTask(): Generator {
  yield takeEvery(ProductSagaActionTypes.UPDATE_TASK, onUpdateTask);
}

export function* onUpdateTask(action: UpdateTaskAction): Generator {
  try {
    yield put(updateTaskRequest());
    const { data } = yield call(updateTask, action.payload);
    const updatedTask: ProductScopedTask = {
      task: {
        id: data.id,
        name: data.name,
        notes: data.notes,
        dueDate: data.dueDate ? new Date(data.dueDate) : null,
        status: data.status,
      },
      ProductKey: action.payload.ProductKey, // used for assignment to correct Product in Redux store
    };
    yield put(updateTaskSuccess(updatedTask));
  } catch (error) {
    yield put(updateTaskFailure(error));
  }
}

export function* watchOnCreateMilestone(): Generator {
  yield takeEvery(ProductSagaActionTypes.CREATE_MILESTONE, onCreateMilestone);
}

export function* onCreateMilestone(action: CreateMilestoneAction): Generator {
  try {
    yield put(createMilestoneRequest());
    const { data } = yield call(createMilestone, action.payload);
    const createdMilestone: ProductScopedMilestone = {
      milestone: {
        id: data.id,
        name: data.name,
        notes: data.notes,
        date: data.date,
      },
      ProductKey: action.payload.ProductKey, // used for assignment to correct Product in Redux store
    };
    yield put(createMilestoneSuccess(createdMilestone));
  } catch (error) {
    yield put(createMilestoneFailure(error));
  }
}

export default function* productSaga(): Generator {
  yield all([
    fork(watchOnGetProducts),
    fork(watchOnCreateProduct),
    fork(watchOnUpdateProduct),
    fork(watchOnUpdateProductItem),
    fork(watchOnCreateTask),
    fork(watchOnUpdateTask),
    fork(watchOnCreateMilestone),
  ]);
}
