import { CreateUpdateProductDto, Product } from '@sopheon/shell-api';
import axios, { AxiosRequestConfig } from 'axios';

import { settings } from './settings';

const API_URL_BASE: string = settings.ProductManagementApiUrlBase;
const API_URL_PATH_CREATE_PRODUCT: string = settings.CreateProductUrlPath;
const API_URL_PATH_UPDATE_PRODUCT: string = settings.UpdateProductUrlPath;

export const createProduct: (productDto: CreateUpdateProductDto) => Promise<Product> = async productDto => {
  const createProductUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_CREATE_PRODUCT}`
    .replace(settings.TokenEnvironmentKey, productDto.EnvironmentKey);

  const config: AxiosRequestConfig = {
    headers: {
      'Authorization': `Bearer ${productDto.AccessToken}`
    }
  };

  return await axios.post(createProductUrlWithEnvironment, productDto.Product, config);
};

export const updateProduct: (productDto: CreateUpdateProductDto) => Promise<Product> = async productDto => {
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
