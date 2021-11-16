import { InjectReducerMap, InjectSagaMap, ShellApiProps } from '@sopheon/shell-api';
import { FunctionComponent } from 'react';

import App from './App';
import { createProduct, CreateProductAction, getProducts, GetProductsAction, updateProduct, UpdateProductAction } from './onboardingInfoReducer';
import { NAMESPACE, rootReducer, RootState } from './rootReducer';
import rootSaga from './rootSaga';
import { CreateUpdateProductDto, EnvironmentScopedApiRequestDto, Product } from './types';

export type AppStateProps = {
  currentStep: number;
  products: Product[];
};

export type AppDispatchProps = {
  getProducts: (requestDto: EnvironmentScopedApiRequestDto) => GetProductsAction;
  createProduct: (product: CreateUpdateProductDto) => CreateProductAction;
  updateProduct: (product: CreateUpdateProductDto) => UpdateProductAction;
};

const AppContainer: FunctionComponent<ShellApiProps> = ({ shellApi }: ShellApiProps) => {
  const mapAppStateProps = (state: RootState): AppStateProps => ({
    currentStep: state[NAMESPACE] ? state[NAMESPACE].onboardingInfo.currentStep : 1,
    products: state[NAMESPACE] ? state[NAMESPACE].onboardingInfo.products : []
  });

  const mapAppDispatchProps = (state: RootState): AppDispatchProps => ({
    getProducts: (requestDto: EnvironmentScopedApiRequestDto) => getProducts(requestDto),
    createProduct: (product: CreateUpdateProductDto) => createProduct(product),
    updateProduct: (product: CreateUpdateProductDto) => updateProduct(product)
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
