import { InjectReducerMap, InjectSagaMap, ShellApiProps } from '@sopheon/shell-api';
import { FunctionComponent } from 'react';

import App from './App';
import { createProduct, CreateProductAction, updateProduct, UpdateProductAction } from './onboardingInfoReducer';
import { NAMESPACE, rootReducer, RootState } from './rootReducer';
import rootSaga from './rootSaga';
import { CreateUpdateProductDto } from './types';

export type AppStateProps = {
  currentStep: number
};

export type AppDispatchProps = {
  createProduct: (product: CreateUpdateProductDto) => CreateProductAction;
  updateProduct: (product: CreateUpdateProductDto) => UpdateProductAction;
};

const AppContainer: FunctionComponent<ShellApiProps> = ({ shellApi }: ShellApiProps) => {
  const mapAppStateProps = (state: RootState): AppStateProps => ({
    currentStep: state[NAMESPACE] ? state[NAMESPACE].onboardingInfo.currentStep : 1,
  });

  const mapAppDispatchProps = (state: RootState): AppDispatchProps => ({
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
