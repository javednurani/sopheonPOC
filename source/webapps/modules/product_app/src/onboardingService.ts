import axios from 'axios';

import { settings } from './settings';
import { Product } from './types';

// TODO Cloud-2148 remove hardcoded EnvironmentKey
const ENVIRONMENT_KEY_STUB = '00000000-0000-0000-0000-000000000001';

const API_URL_BASE: string = settings.ProductManagementApiUrlBase;
const API_URL_PATH_CREATE_PRODUCT: string = settings.CreateProductUrlPath;
const API_URL_PATH_UPDATE_PRODUCT: string = settings.UpdateProductUrlPath;

export const createProduct: (product: Product) => Promise<Product> = async product => {
  const createProductUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_CREATE_PRODUCT}`
    .replace(settings.TokenEnvironmentKey, ENVIRONMENT_KEY_STUB);
  return await axios.post(createProductUrlWithEnvironment, product);
};

export const updateProduct: (product: Product) => Promise<Product> = async product => {
  const updateProductUrlWithEnvironment = `${API_URL_BASE}${API_URL_PATH_UPDATE_PRODUCT}`
    .replace(settings.TokenEnvironmentKey, ENVIRONMENT_KEY_STUB)
    .replace(settings.TokenProductKey, product.Key || ''); // TODO, nullable Key? null check ?
  return await axios.patch(updateProductUrlWithEnvironment, product);
};

