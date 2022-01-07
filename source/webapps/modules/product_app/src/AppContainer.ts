import { FetchStatus, InjectReducerMap, InjectSagaMap, ShellApiProps } from '@sopheon/shell-api';
import { FunctionComponent } from 'react';

import App from './App';
import { nextStep, NextStepAction } from './onboarding/onboardingReducer';
import {
  createMilestone,
  CreateMilestoneAction,
  createProduct,
  CreateProductAction,
  createTask,
  CreateTaskAction,
  getProducts,
  GetProductsAction,
  updateProduct,
  UpdateProductAction,
  updateProductItem,
  UpdateProductItemAction,
  updateTask,
  UpdateTaskAction,
} from './product/productReducer';
import { NAMESPACE, rootReducer, RootState } from './rootReducer';
import rootSaga from './rootSaga';
import {
  CreateProductModel,
  EnvironmentScopedApiRequestModel,
  PostMilestoneModel,
  PostPutTaskModel,
  Product,
  UpdateProductItemModel,
  UpdateProductModel,
} from './types';

export type AppStateProps = {
  currentStep: number;
  products: Product[];
  getProductsFetchStatus: FetchStatus;
};

export type AppDispatchProps = {
  nextStep: () => NextStepAction;
  getProducts: (requestDto: EnvironmentScopedApiRequestModel) => GetProductsAction;
  createProduct: (product: CreateProductModel) => CreateProductAction;
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  updateProductItem: (product: UpdateProductItemModel) => UpdateProductItemAction;
  createTask: (task: PostPutTaskModel) => CreateTaskAction;
  updateTask: (task: PostPutTaskModel) => UpdateTaskAction;
  createMilestone: (milestone: PostMilestoneModel) => CreateMilestoneAction;
};

const AppContainer: FunctionComponent<ShellApiProps> = ({ shellApi }: ShellApiProps) => {
  const mapAppStateProps = (state: RootState): AppStateProps => ({
    currentStep: state[NAMESPACE] ? state[NAMESPACE].onboarding.currentStep : 1,
    products: state[NAMESPACE] ? state[NAMESPACE].product.products : [],
    getProductsFetchStatus: state[NAMESPACE] ? state[NAMESPACE].product.getProductsFetchStatus : FetchStatus.NotActive,
  });

  const mapAppDispatchProps = (state: RootState): AppDispatchProps => ({
    nextStep: () => nextStep(),
    getProducts: (requestDto: EnvironmentScopedApiRequestModel) => getProducts(requestDto),
    createProduct: (product: CreateProductModel) => createProduct(product),
    updateProduct: (product: UpdateProductModel) => updateProduct(product),
    updateProductItem: (productItem: UpdateProductItemModel) => updateProductItem(productItem),
    createTask: (task: PostPutTaskModel) => createTask(task),
    updateTask: (task: PostPutTaskModel) => updateTask(task),
    createMilestone: (milestone: PostMilestoneModel) => createMilestone(milestone),
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
