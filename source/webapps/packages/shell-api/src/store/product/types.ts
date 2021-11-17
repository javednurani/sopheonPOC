import { PayloadAction } from '../types';

// TODO - (for domain/DTO types like Product), this file should serve as a temporary stub for Types for DTOs in the Sopheon.CloudNative.Products WebAPI project
// In Cloud-2147, we should generate TS Types from Sopheon.CloudNative.Products.AspNetCore OpenAPI schema

export interface Product {
  Id: string | null;
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
export enum OnboardingSagaActionTypes { // TODO, rename - remove onboarding (full file)
  CREATE_PRODUCT = 'ONBOARDING/CREATE_PRODUCT',
  UPDATE_PRODUCT = 'ONBOARDING/UPDATE_PRODUCT',
  GET_PRODUCTS = 'ONBOARDING/GET_PRODUCTS'
}

export type GetProductsAction = PayloadAction<OnboardingSagaActionTypes.GET_PRODUCTS, EnvironmentScopedApiRequestDto>;
export type CreateProductAction = PayloadAction<OnboardingSagaActionTypes.CREATE_PRODUCT, CreateUpdateProductDto>;
export type UpdateProductAction = PayloadAction<OnboardingSagaActionTypes.UPDATE_PRODUCT, CreateUpdateProductDto>;
