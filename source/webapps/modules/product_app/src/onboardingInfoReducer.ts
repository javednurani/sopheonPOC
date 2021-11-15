import { Action, createAction, createPayloadAction, FetchStatus, PayloadAction } from '@sopheon/shell-api';
import { Reducer } from 'redux';

import { CreateUpdateProductDto, Product } from './types';

//#region  Action Types

// REDUCER ACTION TYPES

// eslint-disable-next-line no-shadow
enum OnboardingActionTypes {
  NEXT_STEP = 'ONBOARDING/NEXT_STEP',

  CREATE_PRODUCT_REQUEST = 'ONBOARDING/CREATE_PRODUCT_REQUEST',
  CREATE_PRODUCT_SUCCESS = 'ONBOARDING/CREATE_PRODUCT_SUCCESS',
  CREATE_PRODUCT_FAILURE = 'ONBOARDING/CREATE_PRODUCT_FAILURE',

  UPDATE_PRODUCT_REQUEST = 'ONBOARDING/UPDATE_PRODUCT_REQUEST',
  UPDATE_PRODUCT_SUCCESS = 'ONBOARDING/UPDATE_PRODUCT_SUCCESS',
  UPDATE_PRODUCT_FAILURE = 'ONBOARDING/UPDATE_PRODUCT_FAILURE',

  GET_PRODUCTS_REQUEST = 'ONBOARDING/GET_PRODUCTS_REQUEST',
  GET_PRODUCTS_SUCCESS = 'ONBOARDING/GET_PRODUCTS_SUCCESS',
  GET_PRODUCTS_FAILURE = 'ONBOARDING/GET_PRODUCTS_FAILURE',
}

export type NextStepAction = Action<OnboardingActionTypes.NEXT_STEP>;

export type CreateProductRequestAction = Action<OnboardingActionTypes.CREATE_PRODUCT_REQUEST>;
export type CreateProductSuccessAction = PayloadAction<OnboardingActionTypes.CREATE_PRODUCT_SUCCESS, Product>;
export type CreateProductFailureAction = PayloadAction<OnboardingActionTypes.CREATE_PRODUCT_FAILURE, Error>;

export type UpdateProductRequestAction = Action<OnboardingActionTypes.UPDATE_PRODUCT_REQUEST>;
export type UpdateProductSuccessAction = PayloadAction<OnboardingActionTypes.UPDATE_PRODUCT_SUCCESS, Product>;
export type UpdateProductFailureAction = PayloadAction<OnboardingActionTypes.UPDATE_PRODUCT_FAILURE, Error>;

export type GetProductsRequestAction = Action<OnboardingActionTypes.GET_PRODUCTS_REQUEST>;
export type GetProductsSuccessAction = PayloadAction<OnboardingActionTypes.GET_PRODUCTS_SUCCESS, Product[]>;
export type GetProductsFailureAction = PayloadAction<OnboardingActionTypes.GET_PRODUCTS_FAILURE, Error>;

export type OnboardingReducerActions =
  | NextStepAction
  | CreateProductRequestAction
  | CreateProductSuccessAction
  | CreateProductFailureAction
  | UpdateProductRequestAction
  | UpdateProductSuccessAction
  | UpdateProductFailureAction
  | GetProductsRequestAction
  | GetProductsSuccessAction
  | GetProductsFailureAction;

// SAGA ACTION TYPES

// eslint-disable-next-line no-shadow
export enum OnboardingSagaActionTypes {
  CREATE_PRODUCT = 'ONBOARDING/CREATE_PRODUCT',
  UPDATE_PRODUCT = 'ONBOARDING/UPDATE_PRODUCT',
  GET_PRODUCTS = 'ONBOARDING/GET_PRODUCTS'
}

export type GetProductsAction = PayloadAction<OnboardingSagaActionTypes.GET_PRODUCTS, Product[]>;
export type CreateProductAction = PayloadAction<OnboardingSagaActionTypes.CREATE_PRODUCT, CreateUpdateProductDto>;
export type UpdateProductAction = PayloadAction<OnboardingSagaActionTypes.UPDATE_PRODUCT, CreateUpdateProductDto>;

//#endregion

//#region  Action Creators

// REDUCER ACTIONS

export const nextStep = (): NextStepAction => createAction(OnboardingActionTypes.NEXT_STEP);

export const createProductRequest = (): CreateProductRequestAction => createAction(OnboardingActionTypes.CREATE_PRODUCT_REQUEST);
export const createProductSuccess = (product: Product): CreateProductSuccessAction =>
  createPayloadAction(OnboardingActionTypes.CREATE_PRODUCT_SUCCESS, product);
export const createProductFailure = (error: Error): CreateProductFailureAction =>
  createPayloadAction(OnboardingActionTypes.CREATE_PRODUCT_FAILURE, error);

export const updateProductRequest = (): UpdateProductRequestAction => createAction(OnboardingActionTypes.UPDATE_PRODUCT_REQUEST);
export const updateProductSuccess = (product: Product): UpdateProductSuccessAction =>
  createPayloadAction(OnboardingActionTypes.UPDATE_PRODUCT_SUCCESS, product);
export const updateProductFailure = (error: Error): UpdateProductFailureAction =>
  createPayloadAction(OnboardingActionTypes.UPDATE_PRODUCT_FAILURE, error);

export const getProductsRequest = (): GetProductsRequestAction => createAction(OnboardingActionTypes.GET_PRODUCTS_REQUEST);
export const getProductsSuccess = (products: Product[]): GetProductsSuccessAction =>
  createPayloadAction(OnboardingActionTypes.GET_PRODUCTS_SUCCESS, products);
export const getProductsFailure = (error: Error): GetProductsFailureAction => createPayloadAction(OnboardingActionTypes.GET_PRODUCTS_FAILURE, error);

// SAGA ACTIONS

export const getProducts = (products: Product[]): GetProductsAction => createPayloadAction(OnboardingSagaActionTypes.GET_PRODUCTS, products);
export const createProduct = (product: CreateUpdateProductDto): CreateProductAction => createPayloadAction(OnboardingSagaActionTypes.CREATE_PRODUCT, product);
export const updateProduct = (product: CreateUpdateProductDto): UpdateProductAction => createPayloadAction(OnboardingSagaActionTypes.UPDATE_PRODUCT, product);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export type OnboardingStateShape = {
  currentStep: number;
  product: Product | null;
  createProductFetchStatus: FetchStatus;
  updateProductFetchStatus: FetchStatus;
  getProductsFetchStatus: FetchStatus;
};

export const initialState: OnboardingStateShape = {
  currentStep: 2,
  product: null,
  createProductFetchStatus: FetchStatus.NotActive,
  updateProductFetchStatus: FetchStatus.NotActive,
  getProductsFetchStatus: FetchStatus.NotActive,
};

// HANDLERS

const setValue = (state: OnboardingStateShape, valueToSet: number): OnboardingStateShape => ({
  ...state,
  currentStep: valueToSet,
});

const createProductRequestHandler = (state: OnboardingStateShape) => ({
  ...state,
  createProductFetchStatus: FetchStatus.InProgress,
});

const createProductSuccessHandler = (state: OnboardingStateShape, productToSet: Product) => ({
  ...state,
  product: productToSet,
  createProductFetchStatus: FetchStatus.DoneSuccess,
});

const createProductFailureHandler = (state: OnboardingStateShape, error: Error) => {
  console.log(error);
  return {
    ...state,
    createProductFetchStatus: FetchStatus.DoneFailure,
  };
};

const updateProductRequestHandler = (state: OnboardingStateShape) => ({
  ...state,
  updateProductFetchStatus: FetchStatus.InProgress,
});

const updateProductSuccessHandler = (state: OnboardingStateShape, productToSet: Product) => ({
  ...state,
  product: productToSet,
  createProductFetchStatus: FetchStatus.DoneSuccess,
});

const updateProductFailureHandler = (state: OnboardingStateShape, error: Error) => {
  console.log(error);
  return {
    ...state,
    updateProductFetchStatus: FetchStatus.DoneFailure,
  };
};

const getProductsRequestHandler = (state: OnboardingStateShape) => ({
  ...state,
  getProductsFetchStatus: FetchStatus.InProgress,
});

const getProductsSuccessHandler = (state: OnboardingStateShape, productsToSet: Product[]) => ({
  ...state,
  products: productsToSet,
  getProductsFetchStatus: FetchStatus.DoneSuccess,
});

const getProductsFailureHandler = (state: OnboardingStateShape, error: Error) => {
  console.log(error);
  return {
    ...state,
    getProductsFetchStatus: FetchStatus.DoneFailure,
  };
};

// ACTION SWITCH

export const onboardingInfoReducer: Reducer<OnboardingStateShape, OnboardingReducerActions> = (state = initialState, action) => {
  switch (action.type) {
    case OnboardingActionTypes.NEXT_STEP:
      return setValue(state, state.currentStep + 1);
    case OnboardingActionTypes.CREATE_PRODUCT_REQUEST:
      return createProductRequestHandler(state);
    case OnboardingActionTypes.CREATE_PRODUCT_SUCCESS:
      return createProductSuccessHandler(state, action.payload);
    case OnboardingActionTypes.CREATE_PRODUCT_FAILURE:
      return createProductFailureHandler(state, action.payload);
    case OnboardingActionTypes.UPDATE_PRODUCT_REQUEST:
      return updateProductRequestHandler(state);
    case OnboardingActionTypes.UPDATE_PRODUCT_SUCCESS:
      return updateProductSuccessHandler(state, action.payload);
    case OnboardingActionTypes.UPDATE_PRODUCT_FAILURE:
      return updateProductFailureHandler(state, action.payload);
    case OnboardingActionTypes.GET_PRODUCTS_REQUEST:
      return getProductsRequestHandler(state);
    case OnboardingActionTypes.GET_PRODUCTS_SUCCESS:
      return getProductsSuccessHandler(state, action.payload);
    case OnboardingActionTypes.GET_PRODUCTS_FAILURE:
      return getProductsFailureHandler(state, action.payload);
    default:
      return state;
  }
};

//#endregion
