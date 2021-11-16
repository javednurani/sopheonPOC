import {
  Action,
  createAction,
  createPayloadAction,
  CreateProductAction,
  CreateUpdateProductDto,
  FetchStatus,
  OnboardingSagaActionTypes,
  PayloadAction,
  Product,
  UpdateProductAction,
} from '@sopheon/shell-api';
import { Reducer } from 'redux';

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
}

export type NextStepAction = Action<OnboardingActionTypes.NEXT_STEP>;

export type CreateProductRequestAction = Action<OnboardingActionTypes.CREATE_PRODUCT_REQUEST>;
export type CreateProductSuccessAction = PayloadAction<OnboardingActionTypes.CREATE_PRODUCT_SUCCESS, Product>;
export type CreateProductFailureAction = PayloadAction<OnboardingActionTypes.CREATE_PRODUCT_FAILURE, Error>;

export type UpdateProductRequestAction = Action<OnboardingActionTypes.UPDATE_PRODUCT_REQUEST>;
export type UpdateProductSuccessAction = PayloadAction<OnboardingActionTypes.UPDATE_PRODUCT_SUCCESS, Product>;
export type UpdateProductFailureAction = PayloadAction<OnboardingActionTypes.UPDATE_PRODUCT_FAILURE, Error>;

export type OnboardingReducerActions =
  | NextStepAction
  | CreateProductRequestAction
  | CreateProductSuccessAction
  | CreateProductFailureAction
  | UpdateProductRequestAction
  | UpdateProductSuccessAction
  | UpdateProductFailureAction;


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

// SAGA ACTIONS

export const createProduct = (product: CreateUpdateProductDto): CreateProductAction => createPayloadAction(OnboardingSagaActionTypes.CREATE_PRODUCT, product);
export const updateProduct = (product: CreateUpdateProductDto): UpdateProductAction => createPayloadAction(OnboardingSagaActionTypes.UPDATE_PRODUCT, product);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export type OnboardingStateShape = {
  currentStep: number;
  createProductFetchStatus: FetchStatus;
  updateProductFetchStatus: FetchStatus;
};

export const initialState: OnboardingStateShape = {
  currentStep: 2,
  createProductFetchStatus: FetchStatus.NotActive,
  updateProductFetchStatus: FetchStatus.NotActive,
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
  // TODO move createProduct to SHELL
  // products: [...state.products, productToSet],
  // createProductFetchStatus: FetchStatus.DoneSuccess,
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
  // product: productToSet, // TODO implement Update
  createProductFetchStatus: FetchStatus.DoneSuccess,
});

const updateProductFailureHandler = (state: OnboardingStateShape, error: Error) => {
  console.log(error);
  return {
    ...state,
    updateProductFetchStatus: FetchStatus.DoneFailure,
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
    default:
      return state;
  }
};

//#endregion
