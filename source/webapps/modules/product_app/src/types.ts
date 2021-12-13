// TODO - (for domain/DTO types like Product), this file should serve as a temporary stub for Types for DTOs in the Sopheon.CloudNative.Products WebAPI project
// In Cloud-2147, we should generate TS Types from Sopheon.CloudNative.Products.AspNetCore OpenAPI schema

// eslint-disable-next-line no-shadow
export enum Attributes {
  INDUSTRIES = -1,
  NOTES = -2,
  DUEDATE = -3,
  STATUS = -4,
}

// eslint-disable-next-line no-shadow
export enum ProductItemTypes {
  TASK = -1,
}

export type UpdateProductModel = {
  ProductPatchData: PatchOperation[];
} & EnvironmentScopedApiRequestModel &
  ProductScopedApiRequestModel;

export type CreateProductModel = {
  Product: ProductPostDto;
} & EnvironmentScopedApiRequestModel;

export type EnvironmentScopedApiRequestModel = {
  EnvironmentKey: string;
  AccessToken: string;
};

export type ProductScopedApiRequestModel = {
  ProductKey: string;
};

// DTO definitions from Sopheon.CloudNative.Products
// TODO Cloud-2147, generate from OpenAPI spec

export interface Product {
  id: number | null;
  key: string | null;
  name: string;
  industries: number[];
  goals: Goal[];
  kpis: KeyPerformanceIndicator[];
  todos: ToDoItem[];
}

export interface ToDoItem {
  id: number;
  name: string;
  notes: string | null;
  dueDate: Date | null;
  status: Status;
}

// eslint-disable-next-line no-shadow
export enum Status {
  NotStarted = -1,
  InProgress = -2,
  Assigned = -3,
  Complete = -4,
}

export interface Goal {
  id: number;
  name: string;
  description: string;
}

export interface KeyPerformanceIndicator {
  keyPerformanceIndicatorId: number;
  attributeId: number;
  attribute: AttributeDto;
}

export interface Int32AttributeValueDto {
  AttributeId: number;
  Value: number | null;
}

export interface ProductPostDto {
  Name: string;
  Int32AttributeValues: Int32AttributeValueDto[];
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
  attributeId?: number;
  attributeValueTypeId: number;
  name: string;
}
