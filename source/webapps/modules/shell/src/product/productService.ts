import { CreateUpdateProductDto, EnvironmentScopedApiRequestDto, Product } from '@sopheon/shell-api';
import axios, { AxiosRequestConfig } from 'axios';

import { productSettings } from '../settings/productSettings';

const API_URL_BASE: string = productSettings.ProductManagementApiUrlBase;
const API_URL_PATH_GET_PRODUCT: string = productSettings.getProductsUrlPath;
const API_URL_PATH_CREATE_PRODUCT: string = productSettings.CreateProductUrlPath;
const API_URL_PATH_UPDATE_PRODUCT: string = productSettings.UpdateProductUrlPath;

export const getProducts: (requestDto: EnvironmentScopedApiRequestDto) => Promise<Product[]> = async requestDto => {
  const getProductsUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_GET_PRODUCT}`
    .replace(productSettings.TokenEnvironmentKey, requestDto.EnvironmentKey);

  const config: AxiosRequestConfig = {
    headers: {
      'Authorization': `Bearer ${requestDto.AccessToken}`
    }
  };

  return await axios.get(getProductsUrlWithEnvironment, config);
};

export const createProduct: (productDto: CreateUpdateProductDto) => Promise<Product> = async productDto => {
  const createProductUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_CREATE_PRODUCT}`
    .replace(productSettings.TokenEnvironmentKey, productDto.EnvironmentKey);

  const config: AxiosRequestConfig = {
    headers: {
      'Authorization': `Bearer ${productDto.AccessToken}`
    }
  };

  return await axios.post(createProductUrlWithEnvironment, productDto.Product, config);
};

export const updateProduct: (productDto: CreateUpdateProductDto) => Promise<Product> = async productDto => {
  const updateProductUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_UPDATE_PRODUCT}`
    .replace(productSettings.TokenEnvironmentKey, productDto.EnvironmentKey)
    .replace(productSettings.TokenProductKey, productDto.Product.Key || ''); // TODO, nullable Key? null check ?

  const config: AxiosRequestConfig = {
    headers: {
      'Authorization': `Bearer ${productDto.AccessToken}`
    }
  };

  return await axios.patch(updateProductUrlWithEnvironment, productDto.Product, config);
};
