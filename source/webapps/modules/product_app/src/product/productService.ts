import axios, { AxiosRequestConfig } from 'axios';

import { settings } from '../settings';
import {
  CreateProductModel,
  DeleteTaskModel,
  EnvironmentScopedApiRequestModel,
  MilestoneDto,
  PostMilestoneModel,
  PostPutTaskModel,
  Product,
  TaskDto,
  UpdateProductItemModel,
  UpdateProductModel,
} from '../types';

const API_URL_BASE: string = settings.ProductManagementApiUrlBase;
const API_URL_PATH_GET_PRODUCT: string = settings.getProductsUrlPath;
const API_URL_PATH_CREATE_PRODUCT: string = settings.CreateProductUrlPath;
const API_URL_PATH_UPDATE_PRODUCT: string = settings.UpdateProductUrlPath;

export const getProducts: (requestDto: EnvironmentScopedApiRequestModel) => Promise<Product[]> = async requestDto => {
  const getProductsUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_GET_PRODUCT}`.replace(settings.TokenEnvironmentKey, requestDto.EnvironmentKey);

  const config: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${requestDto.AccessToken}`,
    },
  };

  return await axios.get(getProductsUrlWithEnvironment, config);
};

export const createProduct: (createProductModel: CreateProductModel) => Promise<Product> = async createProductModel => {
  const createProductUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_CREATE_PRODUCT}`.replace(
    settings.TokenEnvironmentKey,
    createProductModel.EnvironmentKey
  );

  const config: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${createProductModel.AccessToken}`,
    },
  };

  return await axios.post(createProductUrlWithEnvironment, createProductModel.Product, config);
};

export const updateProduct: (updateProductModel: UpdateProductModel) => Promise<Product> = async updateProductModel => {
  const updateProductUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_UPDATE_PRODUCT}`
    .replace(settings.TokenEnvironmentKey, updateProductModel.EnvironmentKey)
    .replace(settings.TokenProductKey, updateProductModel.ProductKey || ''); // TODO, nullable Key? null check ?

  const config: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${updateProductModel.AccessToken}`,
    },
  };

  return await axios.patch(updateProductUrlWithEnvironment, updateProductModel.ProductPatchData, config);
};

export const updateProductItem: (updateProductItemModel: UpdateProductItemModel) => Promise<Product> = async updateProductItemModel => {
  const updateProductItemUrlWithEnvironment = `${API_URL_BASE}${settings.UpdateProductItemUrlPath}`
    .replace(settings.TokenEnvironmentKey, updateProductItemModel.EnvironmentKey)
    .replace(settings.TokenProductKey, updateProductItemModel.ProductKey || '') // TODO, nullable Key? null check ?
    .replace(settings.TokenProductItemId, updateProductItemModel.ProductItem.id.toString());

  const config: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${updateProductItemModel.AccessToken}`,
    },
  };

  return await axios.put(updateProductItemUrlWithEnvironment, updateProductItemModel.ProductItem, config);
};

export const createTask: (createTaskModel: PostPutTaskModel) => Promise<TaskDto> = async createTaskModel => {
  const createTaskUrlWithEnvironment = `${API_URL_BASE}${settings.CreateTaskUrlPath}`
    .replace(settings.TokenEnvironmentKey, createTaskModel.EnvironmentKey)
    .replace(settings.TokenProductKey, createTaskModel.ProductKey || ''); // TODO, nullable Key? null check ?;

  const config: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${createTaskModel.AccessToken}`,
    },
  };

  return await axios.post(createTaskUrlWithEnvironment, createTaskModel.Task, config);
};

export const updateTask: (updateTaskModel: PostPutTaskModel) => Promise<TaskDto> = async updateTaskModel => {
  const updateTaskUrlWithEnvironment = `${API_URL_BASE}${settings.UpdateDeleteTaskUrlPath}`
    .replace(settings.TokenEnvironmentKey, updateTaskModel.EnvironmentKey)
    .replace(settings.TokenProductKey, updateTaskModel.ProductKey || '') // TODO, nullable Key? null check ?;
    .replace(settings.TokenTaskId, updateTaskModel.Task.id.toString());

  const config: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${updateTaskModel.AccessToken}`,
    },
  };

  return await axios.put(updateTaskUrlWithEnvironment, updateTaskModel.Task, config);
};

export const createMilestone: (createMilestoneModel: PostMilestoneModel) => Promise<MilestoneDto> = async createMilestoneModel => {
  const createMilestoneUrlWithEnvironment = `${API_URL_BASE}${settings.CreateMilestoneUrlPath}`
    .replace(settings.TokenEnvironmentKey, createMilestoneModel.EnvironmentKey)
    .replace(settings.TokenProductKey, createMilestoneModel.ProductKey);

  const config: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${createMilestoneModel.AccessToken}`,
    },
  };

  return await axios.post(createMilestoneUrlWithEnvironment, createMilestoneModel.Milestone, config);
};

export const deleteTask: (deleteTaskModel: DeleteTaskModel) => Promise<TaskDto> = async deleteTaskModel => {
  const deleteTaskUrlWithEnvironment = `${API_URL_BASE}${settings.UpdateDeleteTaskUrlPath}`
    .replace(settings.TokenEnvironmentKey, deleteTaskModel.EnvironmentKey)
    .replace(settings.TokenProductKey, deleteTaskModel.ProductKey || '') // TODO, nullable Key? null check ?;
    .replace(settings.TokenTaskId, deleteTaskModel.TaskId.toString());

  const config: AxiosRequestConfig = {
    headers: {
      Authorization: `Bearer ${deleteTaskModel.AccessToken}`,
    },
  };

  return await axios.delete(deleteTaskUrlWithEnvironment, config);
};
