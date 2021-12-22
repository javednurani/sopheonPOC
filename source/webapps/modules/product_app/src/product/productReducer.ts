import { Action, createAction, createPayloadAction, FetchStatus, PayloadAction } from '@sopheon/shell-api';
import { Reducer } from 'redux';

import { CreateProductModel, CreateTaskModel, EnvironmentScopedApiRequestModel, Product, ToDoItem, UpdateProductItemModel, UpdateProductModel } from '../types';

//#region  Action Types

// REDUCER ACTION TYPES

// eslint-disable-next-line no-shadow
enum ProductActionTypes {
  GET_PRODUCTS_REQUEST = 'PRODUCT/PRODUCT/GET_PRODUCTS_REQUEST',
  GET_PRODUCTS_SUCCESS = 'PRODUCT/PRODUCT/GET_PRODUCTS_SUCCESS',
  GET_PRODUCTS_FAILURE = 'PRODUCT/PRODUCT/GET_PRODUCTS_FAILURE',

  CREATE_PRODUCT_REQUEST = 'PRODUCT/PRODUCT/CREATE_PRODUCT_REQUEST',
  CREATE_PRODUCT_SUCCESS = 'PRODUCT/PRODUCT/CREATE_PRODUCT_SUCCESS',
  CREATE_PRODUCT_FAILURE = 'PRODUCT/PRODUCT/CREATE_PRODUCT_FAILURE',

  UPDATE_PRODUCT_REQUEST = 'PRODUCT/PRODUCT/UPDATE_PRODUCT_REQUEST',
  UPDATE_PRODUCT_SUCCESS = 'PRODUCT/PRODUCT/UPDATE_PRODUCT_SUCCESS',
  UPDATE_PRODUCT_FAILURE = 'PRODUCT/PRODUCT/UPDATE_PRODUCT_FAILURE',

  UPDATE_PRODUCT_ITEM_REQUEST = 'PRODUCT/PRODUCT/UPDATE_PRODUCT_ITEM_REQUEST',
  UPDATE_PRODUCT_ITEM_SUCCESS = 'PRODUCT/PRODUCT/UPDATE_PRODUCT_ITEM_SUCCESS',
  UPDATE_PRODUCT_ITEM_FAILURE = 'PRODUCT/PRODUCT/UPDATE_PRODUCT_ITEM_FAILURE',

  CREATE_TASK_REQUEST = 'PRODUCT/PRODUCT/CREATE_TASK_REQUEST',
  CREATE_TASK_SUCCESS = 'PRODUCT/PRODUCT/CREATE_TASK_SUCCESS',
  CREATE_TASK_FAILURE = 'PRODUCT/PRODUCT/CREATE_TASK_FAILURE',
}

export type GetProductsRequestAction = Action<ProductActionTypes.GET_PRODUCTS_REQUEST>;
export type GetProductsSuccessAction = PayloadAction<ProductActionTypes.GET_PRODUCTS_SUCCESS, Product[]>;
export type GetProductsFailureAction = PayloadAction<ProductActionTypes.GET_PRODUCTS_FAILURE, Error>;

export type CreateProductRequestAction = Action<ProductActionTypes.CREATE_PRODUCT_REQUEST>;
export type CreateProductSuccessAction = PayloadAction<ProductActionTypes.CREATE_PRODUCT_SUCCESS, Product>;
export type CreateProductFailureAction = PayloadAction<ProductActionTypes.CREATE_PRODUCT_FAILURE, Error>;

export type UpdateProductRequestAction = Action<ProductActionTypes.UPDATE_PRODUCT_REQUEST>;
export type UpdateProductSuccessAction = PayloadAction<ProductActionTypes.UPDATE_PRODUCT_SUCCESS, Product>;
export type UpdateProductFailureAction = PayloadAction<ProductActionTypes.UPDATE_PRODUCT_FAILURE, Error>;

export type UpdateProductItemRequestAction = Action<ProductActionTypes.UPDATE_PRODUCT_ITEM_REQUEST>;
export type UpdateProductItemSuccessAction = PayloadAction<ProductActionTypes.UPDATE_PRODUCT_ITEM_SUCCESS, ToDoItem>; // TODO: ProductItem vs ToDo?
export type UpdateProductItemFailureAction = PayloadAction<ProductActionTypes.UPDATE_PRODUCT_ITEM_FAILURE, Error>;

export type CreateTaskRequestAction = Action<ProductActionTypes.CREATE_TASK_REQUEST>;
export type CreateTaskSuccessAction = PayloadAction<ProductActionTypes.CREATE_TASK_SUCCESS, ToDoItem>;
export type CreateTaskFailureAction = PayloadAction<ProductActionTypes.CREATE_TASK_FAILURE, Error>;

export type ProductReducerActions =
  | GetProductsRequestAction
  | GetProductsSuccessAction
  | GetProductsFailureAction
  | CreateProductRequestAction
  | CreateProductSuccessAction
  | CreateProductFailureAction
  | UpdateProductRequestAction
  | UpdateProductSuccessAction
  | UpdateProductFailureAction
  | UpdateProductItemRequestAction
  | UpdateProductItemSuccessAction
  | UpdateProductItemFailureAction
  | CreateTaskRequestAction
  | CreateTaskSuccessAction
  | CreateTaskFailureAction;

// SAGA ACTION TYPES

// eslint-disable-next-line no-shadow
export enum ProductSagaActionTypes {
  CREATE_PRODUCT = 'PRODUCT/PRODUCT/CREATE_PRODUCT',
  UPDATE_PRODUCT = 'PRODUCT/PRODUCT/UPDATE_PRODUCT',
  GET_PRODUCTS = 'PRODUCT/PRODUCT/GET_PRODUCTS',
  UPDATE_PRODUCT_ITEM = 'PRODUCT/PRODUCT/UPDATE_PRODUCT_ITEM',
  CREATE_TASK = 'PRODUCT/PRODUCT/CREATE_TASK'
}

export type GetProductsAction = PayloadAction<ProductSagaActionTypes.GET_PRODUCTS, EnvironmentScopedApiRequestModel>;
export type CreateProductAction = PayloadAction<ProductSagaActionTypes.CREATE_PRODUCT, CreateProductModel>;
export type UpdateProductAction = PayloadAction<ProductSagaActionTypes.UPDATE_PRODUCT, UpdateProductModel>;
export type UpdateProductItemAction = PayloadAction<ProductSagaActionTypes.UPDATE_PRODUCT_ITEM, UpdateProductItemModel>;
export type CreateTaskAction = PayloadAction<ProductSagaActionTypes.CREATE_TASK, CreateTaskModel>;

//#endregion

//#region  Action Creators

// REDUCER ACTIONS

export const getProductsRequest = (): GetProductsRequestAction => createAction(ProductActionTypes.GET_PRODUCTS_REQUEST);
export const getProductsSuccess = (products: Product[]): GetProductsSuccessAction =>
  createPayloadAction(ProductActionTypes.GET_PRODUCTS_SUCCESS, products);
export const getProductsFailure = (error: Error): GetProductsFailureAction => createPayloadAction(ProductActionTypes.GET_PRODUCTS_FAILURE, error);

export const createProductRequest = (): CreateProductRequestAction => createAction(ProductActionTypes.CREATE_PRODUCT_REQUEST);
export const createProductSuccess = (product: Product): CreateProductSuccessAction =>
  createPayloadAction(ProductActionTypes.CREATE_PRODUCT_SUCCESS, product);
export const createProductFailure = (error: Error): CreateProductFailureAction =>
  createPayloadAction(ProductActionTypes.CREATE_PRODUCT_FAILURE, error);

export const updateProductRequest = (): UpdateProductRequestAction => createAction(ProductActionTypes.UPDATE_PRODUCT_REQUEST);
export const updateProductSuccess = (product: Product): UpdateProductSuccessAction =>
  createPayloadAction(ProductActionTypes.UPDATE_PRODUCT_SUCCESS, product);
export const updateProductFailure = (error: Error): UpdateProductFailureAction =>
  createPayloadAction(ProductActionTypes.UPDATE_PRODUCT_FAILURE, error);

export const updateProductItemRequest = (): UpdateProductItemRequestAction => createAction(ProductActionTypes.UPDATE_PRODUCT_ITEM_REQUEST);
export const updateProductItemSuccess = (productItem: ToDoItem): UpdateProductItemSuccessAction =>
  createPayloadAction(ProductActionTypes.UPDATE_PRODUCT_ITEM_SUCCESS, productItem);
export const updateProductItemFailure = (error: Error): UpdateProductItemFailureAction =>
  createPayloadAction(ProductActionTypes.UPDATE_PRODUCT_ITEM_FAILURE, error);

export const createTaskRequest = (): CreateTaskRequestAction => createAction(ProductActionTypes.CREATE_TASK_REQUEST);
export const createTaskSuccess = (task: ToDoItem): CreateTaskSuccessAction =>
  createPayloadAction(ProductActionTypes.CREATE_TASK_SUCCESS, task);
export const createTaskFailure = (error: Error): CreateTaskFailureAction =>
  createPayloadAction(ProductActionTypes.CREATE_TASK_FAILURE, error);


// SAGA ACTIONS

export const getProducts = (requestDto: EnvironmentScopedApiRequestModel): GetProductsAction =>
  createPayloadAction(ProductSagaActionTypes.GET_PRODUCTS, requestDto);
export const createProduct = (product: CreateProductModel): CreateProductAction =>
  createPayloadAction(ProductSagaActionTypes.CREATE_PRODUCT, product);
export const updateProduct = (product: UpdateProductModel): UpdateProductAction =>
  createPayloadAction(ProductSagaActionTypes.UPDATE_PRODUCT, product);
export const updateProductItem = (productItem: UpdateProductItemModel): UpdateProductItemAction =>
  createPayloadAction(ProductSagaActionTypes.UPDATE_PRODUCT_ITEM, productItem);
export const createTask = (task: CreateTaskModel): CreateTaskAction =>
  createPayloadAction(ProductSagaActionTypes.CREATE_TASK, task);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export type ProductStateShape = {
  products: Product[];
  getProductsFetchStatus: FetchStatus;
  createProductFetchStatus: FetchStatus;
  updateProductFetchStatus: FetchStatus;
  createTaskFetchStatus: FetchStatus; // INFO, this call could be made frequently. is there value in tracking the Fetch status?
};

export const initialState: ProductStateShape = {
  products: [],
  getProductsFetchStatus: FetchStatus.NotActive,
  createProductFetchStatus: FetchStatus.NotActive,
  updateProductFetchStatus: FetchStatus.NotActive,
  createTaskFetchStatus: FetchStatus.NotActive,
};

// HANDLERS

const getProductsRequestHandler = (state: ProductStateShape) => ({
  ...state,
  getProductsFetchStatus: FetchStatus.InProgress,
});

const getProductsSuccessHandler = (state: ProductStateShape, productsToSet: Product[]) => ({
  ...state,
  products: productsToSet,
  getProductsFetchStatus: FetchStatus.DoneSuccess,
});

const getProductsFailureHandler = (state: ProductStateShape, error: Error) => {
  console.log(error);
  return {
    ...state,
    getProductsFetchStatus: FetchStatus.DoneFailure,
  };
};

const createProductRequestHandler = (state: ProductStateShape) => ({
  ...state,
  createProductFetchStatus: FetchStatus.InProgress,
});

const createProductSuccessHandler = (state: ProductStateShape, createdProduct: Product) => ({
  ...state,
  products: [...state.products, createdProduct],
  createProductFetchStatus: FetchStatus.DoneSuccess,
});

const createProductFailureHandler = (state: ProductStateShape, error: Error) => {
  console.log(error);
  return {
    ...state,
    createProductFetchStatus: FetchStatus.DoneFailure,
  };
};

const updateProductRequestHandler = (state: ProductStateShape) => ({
  ...state,
  updateProductFetchStatus: FetchStatus.InProgress,
});

// TODO - 'update state' code here will need to be reworked per the Product API Post endpoint behavior
const updateProductSuccessHandler = (state: ProductStateShape, updatedProduct: Product) => {
  const stateProducts = [...state.products];
  const updatedProducts = stateProducts.map(existingProduct => (existingProduct.key !== updatedProduct.key ? existingProduct : updatedProduct));

  return {
    ...state,
    products: updatedProducts,
    updateProductFetchStatus: FetchStatus.DoneSuccess,
  };
};

const updateProductFailureHandler = (state: ProductStateShape, error: Error) => {
  console.log(error);
  return {
    ...state,
    updateProductFetchStatus: FetchStatus.DoneFailure,
  };
};

const updateProductItemRequestHandler = (state: ProductStateShape) => ({
  ...state,
  updateProductItemFetchStatus: FetchStatus.InProgress,
});

const updateProductItemSuccessHandler = (state: ProductStateShape, updatedProductItem: ToDoItem) => {
  const stateProducts = [...state.products];
  const updatedProducts = stateProducts.map(existingProduct => {
    existingProduct.todos = existingProduct.todos.map(todo => (todo.id === updatedProductItem.id ? updatedProductItem : todo));
    return existingProduct; // we're not updating the product, just it's todos
  });

  return {
    ...state,
    products: updatedProducts,
    updateProductItemFetchStatus: FetchStatus.DoneSuccess,
  };
};

const updateProductItemFailureHandler = (state: ProductStateShape, error: Error) => {
  console.log(error);
  return {
    ...state,
    updateProductItemFetchStatus: FetchStatus.DoneFailure,
  };
};


const createTaskRequestHandler = (state: ProductStateShape) => ({
  ...state,
  createTaskFetchStatus: FetchStatus.InProgress,
});

const createTaskSuccessHandler = (state: ProductStateShape, createdTask: ToDoItem) => {
  const updatedProducts = [...state.products];
  updatedProducts.forEach(existingProduct => {
    if (existingProduct.key === createdTask.productKey) {
      existingProduct.todos.push(createdTask); // task has a redundant productKey, could delete property from object, or make a new object here
    }
  });
  return {
    ...state,
    products: updatedProducts,
    createTaskFetchStatus: FetchStatus.DoneSuccess,
  };
};

const createTaskFailureHandler = (state: ProductStateShape, error: Error) => {
  console.log(error);
  return {
    ...state,
    updateProductItemFetchStatus: FetchStatus.DoneFailure,
  };
};


// ACTION SWITCH

export const productReducer: Reducer<ProductStateShape, ProductReducerActions> = (state = initialState, action) => {
  switch (action.type) {
    case ProductActionTypes.GET_PRODUCTS_REQUEST:
      return getProductsRequestHandler(state);
    case ProductActionTypes.GET_PRODUCTS_SUCCESS:
      return getProductsSuccessHandler(state, action.payload);
    case ProductActionTypes.GET_PRODUCTS_FAILURE:
      return getProductsFailureHandler(state, action.payload);
    case ProductActionTypes.CREATE_PRODUCT_REQUEST:
      return createProductRequestHandler(state);
    case ProductActionTypes.CREATE_PRODUCT_SUCCESS:
      return createProductSuccessHandler(state, action.payload);
    case ProductActionTypes.CREATE_PRODUCT_FAILURE:
      return createProductFailureHandler(state, action.payload);
    case ProductActionTypes.UPDATE_PRODUCT_REQUEST:
      return updateProductRequestHandler(state);
    case ProductActionTypes.UPDATE_PRODUCT_SUCCESS:
      return updateProductSuccessHandler(state, action.payload);
    case ProductActionTypes.UPDATE_PRODUCT_FAILURE:
      return updateProductFailureHandler(state, action.payload);
    case ProductActionTypes.UPDATE_PRODUCT_ITEM_REQUEST:
      return updateProductItemRequestHandler(state);
    case ProductActionTypes.UPDATE_PRODUCT_ITEM_SUCCESS:
      return updateProductItemSuccessHandler(state, action.payload);
    case ProductActionTypes.UPDATE_PRODUCT_ITEM_FAILURE:
      return updateProductItemFailureHandler(state, action.payload);
    case ProductActionTypes.CREATE_TASK_REQUEST:
      return createTaskRequestHandler(state);
    case ProductActionTypes.CREATE_TASK_SUCCESS:
      return createTaskSuccessHandler(state, action.payload);
    case ProductActionTypes.CREATE_TASK_FAILURE:
      return createTaskFailureHandler(state, action.payload);
    default:
      return state;
  }
};

//#endregion
