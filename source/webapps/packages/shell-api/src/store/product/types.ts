import { PayloadAction } from '../types';

// TODO - (for domain/DTO types like Product), this file should serve as a temporary stub for Types for DTOs in the Sopheon.CloudNative.Products WebAPI project
// In Cloud-2147, we should generate TS Types from Sopheon.CloudNative.Products.AspNetCore OpenAPI schema

export interface Product {
  Key: string | null;
  Name: string;
  Description: string;
}

export type CreateUpdateProductDto = {
  Product: Product;
} & EnvironmentScopedApiRequestDto;

export type EnvironmentScopedApiRequestDto = {
  EnvironmentKey: string;
  AccessToken: string;
}

// SAGA ACTION TYPES

// eslint-disable-next-line no-shadow
export enum ProductSagaActionTypes {
  CREATE_PRODUCT = 'SHELL/CREATE_PRODUCT',
  UPDATE_PRODUCT = 'SHELL/UPDATE_PRODUCT',
  GET_PRODUCTS = 'SHELL/GET_PRODUCTS'
}

export type GetProductsAction = PayloadAction<ProductSagaActionTypes.GET_PRODUCTS, EnvironmentScopedApiRequestDto>;
export type CreateProductAction = PayloadAction<ProductSagaActionTypes.CREATE_PRODUCT, CreateUpdateProductDto>;
export type UpdateProductAction = PayloadAction<ProductSagaActionTypes.UPDATE_PRODUCT, CreateUpdateProductDto>;
