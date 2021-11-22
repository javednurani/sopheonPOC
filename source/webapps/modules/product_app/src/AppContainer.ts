import {
  FetchStatus,
  InjectReducerMap,
  InjectSagaMap,
  ShellApiProps,
} from '@sopheon/shell-api';
import { FunctionComponent } from 'react';

import App from './App';
import { nextStep, NextStepAction } from './onboarding/onboardingReducer';
import { createProduct, CreateProductAction, getProducts, GetProductsAction, updateProduct, UpdateProductAction } from './product/productReducer';
import { NAMESPACE, rootReducer, RootState } from './rootReducer';
import rootSaga from './rootSaga';
import { CreateUpdateProductDto, EnvironmentScopedApiRequestDto, Product } from './types';

export type AppStateProps = {
  currentStep: number;
  products: Product[];
  getProductsFetchStatus: FetchStatus;
};

export type AppDispatchProps = {
  nextStep: () => NextStepAction;
  getProducts: (requestDto: EnvironmentScopedApiRequestDto) => GetProductsAction;
  createProduct: (product: CreateUpdateProductDto) => CreateProductAction;
  updateProduct: (product: CreateUpdateProductDto) => UpdateProductAction;
};

const AppContainer: FunctionComponent<ShellApiProps> = ({ shellApi }: ShellApiProps) => {
  const mapAppStateProps = (state: RootState): AppStateProps => ({
    currentStep: state[NAMESPACE] ? state[NAMESPACE].onboarding.currentStep : 1,
    products: state[NAMESPACE] ? state[NAMESPACE].product.products : [],
    getProductsFetchStatus: state[NAMESPACE] ? state[NAMESPACE].product.getProductsFetchStatus : FetchStatus.NotActive,
  });

  const mapAppDispatchProps = (state: RootState): AppDispatchProps => ({
    nextStep: () => nextStep(),
    getProducts: (requestDto: EnvironmentScopedApiRequestDto) => getProducts(requestDto),
    createProduct: (product: CreateUpdateProductDto) => createProduct(product),
    updateProduct: (product: CreateUpdateProductDto) => updateProduct(product),
  });

  const appReducerMap: InjectReducerMap = {
    key: NAMESPACE,
    reducer: rootReducer,
  };

  const appSagaMap: InjectSagaMap = {
    key: NAMESPACE,
    saga: rootSaga,
  };

  return shellApi.connectApp(App, appReducerMap, appSagaMap, mapAppStateProps, mapAppDispatchProps);
};

export default AppContainer;
