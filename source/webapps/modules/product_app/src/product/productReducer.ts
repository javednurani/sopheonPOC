import {
  Action,
  createAction,
  createPayloadAction,
  FetchStatus,
  PayloadAction,
} from '@sopheon/shell-api';
import { Reducer } from 'redux';

import {
  CreateProductModel,
  EnvironmentScopedApiRequestModel,
  Product,
  UpdateProductModel
} from '../types';

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

export type ProductReducerActions =
  GetProductsRequestAction
  | GetProductsSuccessAction
  | GetProductsFailureAction
  | CreateProductRequestAction
  | CreateProductSuccessAction
  | CreateProductFailureAction
  | UpdateProductRequestAction
  | UpdateProductSuccessAction
  | UpdateProductFailureAction;


// SAGA ACTION TYPES

// eslint-disable-next-line no-shadow
export enum ProductSagaActionTypes {
  CREATE_PRODUCT = 'PRODUCT/PRODUCT/CREATE_PRODUCT',
  UPDATE_PRODUCT = 'PRODUCT/PRODUCT/UPDATE_PRODUCT',
  GET_PRODUCTS = 'PRODUCT/PRODUCT/GET_PRODUCTS'
}

export type GetProductsAction = PayloadAction<ProductSagaActionTypes.GET_PRODUCTS, EnvironmentScopedApiRequestModel>;
export type CreateProductAction = PayloadAction<ProductSagaActionTypes.CREATE_PRODUCT, CreateProductModel>;
export type UpdateProductAction = PayloadAction<ProductSagaActionTypes.UPDATE_PRODUCT, UpdateProductModel>;


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


// SAGA ACTIONS

export const getProducts = (requestDto: EnvironmentScopedApiRequestModel): GetProductsAction => createPayloadAction(ProductSagaActionTypes.GET_PRODUCTS, requestDto);
export const createProduct = (product: CreateProductModel): CreateProductAction => createPayloadAction(ProductSagaActionTypes.CREATE_PRODUCT, product);
export const updateProduct = (product: UpdateProductModel): UpdateProductAction => createPayloadAction(ProductSagaActionTypes.UPDATE_PRODUCT, product);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export type ProductStateShape = {
  products: Product[];
  getProductsFetchStatus: FetchStatus;
  createProductFetchStatus: FetchStatus;
  updateProductFetchStatus: FetchStatus;
};

export const initialState: ProductStateShape = {
  products: [],
  getProductsFetchStatus: FetchStatus.NotActive,
  createProductFetchStatus: FetchStatus.NotActive,
  updateProductFetchStatus: FetchStatus.NotActive,
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
  const updatedProducts = stateProducts.map(existingProduct => ((existingProduct.key !== updatedProduct.key)
    ? existingProduct
    : updatedProduct));

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
    default:
      return state;
  }
};

//#endregion
