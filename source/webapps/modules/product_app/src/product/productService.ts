import axios, { AxiosRequestConfig } from 'axios';

import { settings } from '../settings';
import { CreateProductModel, CreateUpdateProductModel, EnvironmentScopedApiRequestModel, Product } from '../types';

const API_URL_BASE: string = settings.ProductManagementApiUrlBase;
const API_URL_PATH_GET_PRODUCT: string = settings.getProductsUrlPath;
const API_URL_PATH_CREATE_PRODUCT: string = settings.CreateProductUrlPath;
const API_URL_PATH_UPDATE_PRODUCT: string = settings.UpdateProductUrlPath;

export const getProducts: (requestDto: EnvironmentScopedApiRequestModel) => Promise<Product[]> = async requestDto => {
  const getProductsUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_GET_PRODUCT}`
    .replace(settings.TokenEnvironmentKey, requestDto.EnvironmentKey);

  const config: AxiosRequestConfig = {
    headers: {
      'Authorization': `Bearer ${requestDto.AccessToken}`
    }
  };

  return await axios.get(getProductsUrlWithEnvironment, config);
};

export const createProduct: (productDto: CreateProductModel) => Promise<Product> = async createProductModel => {
  const createProductUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_CREATE_PRODUCT}`
    .replace(settings.TokenEnvironmentKey, createProductModel.EnvironmentKey);

  const config: AxiosRequestConfig = {
    headers: {
      'Authorization': `Bearer ${createProductModel.AccessToken}`
    }
  };

  return await axios.post(createProductUrlWithEnvironment, createProductModel.Product, config);
};

export const updateProduct: (productDto: CreateUpdateProductModel) => Promise<Product> = async productDto => {
  const updateProductUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_UPDATE_PRODUCT}`
    .replace(settings.TokenEnvironmentKey, productDto.EnvironmentKey)
    .replace(settings.TokenProductKey, productDto.Product.Key || ''); // TODO, nullable Key? null check ?

  const config: AxiosRequestConfig = {
    headers: {
      'Authorization': `Bearer ${productDto.AccessToken}`
    }
  };

  return await axios.patch(updateProductUrlWithEnvironment, productDto.Product, config);
};
