// TODO - (for domain/DTO types like Product), this file should serve as a temporary stub for Types for DTOs in the Sopheon.CloudNative.Products WebAPI project
// In Cloud-2147, we should generate TS Types from Sopheon.CloudNative.Products.AspNetCore OpenAPI schema

export interface Product {
  Id: number | null;
  Key: string | null;
  Name: string;
  Industries: number[];
  Goals: string[];
  KPIs: string[];
}

// eslint-disable-next-line no-shadow
export enum Attributes {
  INDUSTRIES = -1,
}

export type UpdateProductModel = {
  ProductPatchData: PatchOperation[];
} & EnvironmentScopedApiRequestModel
  & ProductScopedApiRequestModel;

export type CreateProductModel = {
  Product: ProductPostDto;
} & EnvironmentScopedApiRequestModel;

export type EnvironmentScopedApiRequestModel = {
  EnvironmentKey: string;
  AccessToken: string;
}

export type ProductScopedApiRequestModel = {
  ProductKey: string;
};

// DTO definitions from Sopheon.CloudNative.Products
// TODO Cloud-2147, generate from OpenAPI spec
export interface Int32AttributeValueDto {
  AttributeId: number;
  Value: number | null;
}

export interface ProductPostDto {
  Name: string;
  IntAttributeValues: Int32AttributeValueDto[];
}

export interface PatchOperation {
  op: string;
  path: string;
  value: unknown[];
}

export interface ProductGoalDto {
  name: string;
}

export interface KeyPerformanceIndicatorDto {
  attribute: AttributeDto;
}

export interface AttributeDto {
  attributeValueTypeId: number;
  name: string;
}
