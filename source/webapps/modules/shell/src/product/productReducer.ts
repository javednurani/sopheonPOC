import {
  Action,
  createAction,
  createPayloadAction,
  EnvironmentScopedApiRequestDto,
  FetchStatus,
  GetProductsAction,
  OnboardingSagaActionTypes,
  PayloadAction,
  Product,
} from '@sopheon/shell-api';
import { Reducer } from 'redux';

//#region  Action Types

// REDUCER ACTION TYPES

// eslint-disable-next-line no-shadow
enum ProductActionTypes {
  GET_PRODUCTS_REQUEST = 'SHELL/GET_PRODUCTS_REQUEST',
  GET_PRODUCTS_SUCCESS = 'SHELL/GET_PRODUCTS_SUCCESS',
  GET_PRODUCTS_FAILURE = 'SHELL/GET_PRODUCTS_FAILURE',
}

export type GetProductsRequestAction = Action<ProductActionTypes.GET_PRODUCTS_REQUEST>;
export type GetProductsSuccessAction = PayloadAction<ProductActionTypes.GET_PRODUCTS_SUCCESS, Product[]>;
export type GetProductsFailureAction = PayloadAction<ProductActionTypes.GET_PRODUCTS_FAILURE, Error>;

export type ProductReducerActions =
  GetProductsRequestAction
  | GetProductsSuccessAction
  | GetProductsFailureAction;

//#endregion

//#region  Action Creators

// REDUCER ACTIONS

export const getProductsRequest = (): GetProductsRequestAction => createAction(ProductActionTypes.GET_PRODUCTS_REQUEST);
export const getProductsSuccess = (products: Product[]): GetProductsSuccessAction =>
  createPayloadAction(ProductActionTypes.GET_PRODUCTS_SUCCESS, products);
export const getProductsFailure = (error: Error): GetProductsFailureAction => createPayloadAction(ProductActionTypes.GET_PRODUCTS_FAILURE, error);

// SAGA ACTIONS

export const getProducts = (requestDto: EnvironmentScopedApiRequestDto): GetProductsAction => createPayloadAction(OnboardingSagaActionTypes.GET_PRODUCTS, requestDto);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export type ProductStateShape = {
  products: Product[];
  getProductsFetchStatus: FetchStatus;
};

export const initialState: ProductStateShape = {
  products: [],
  getProductsFetchStatus: FetchStatus.NotActive,
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

// ACTION SWITCH

export const productReducer: Reducer<ProductStateShape, ProductReducerActions> = (state = initialState, action) => {
  switch (action.type) {
    case ProductActionTypes.GET_PRODUCTS_REQUEST:
      return getProductsRequestHandler(state);
    case ProductActionTypes.GET_PRODUCTS_SUCCESS:
      return getProductsSuccessHandler(state, action.payload);
    case ProductActionTypes.GET_PRODUCTS_FAILURE:
      return getProductsFailureHandler(state, action.payload);
    default:
      return state;
  }
};

//#endregion
