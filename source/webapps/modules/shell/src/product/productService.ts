import { EnvironmentScopedApiRequestDto, Product } from '@sopheon/shell-api';
import axios, { AxiosRequestConfig } from 'axios';

import { productSettings } from '../settings/productSettings';

const API_URL_BASE: string = productSettings.ProductManagementApiUrlBase;
const API_URL_PATH_GET_PRODUCT: string = productSettings.getProductsUrlPath;

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
