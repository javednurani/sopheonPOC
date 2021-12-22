import { Status } from './data/status';

export type UpdateProductModel = {
  ProductPatchData: PatchOperation[];
} & EnvironmentScopedApiRequestModel &
  ProductScopedApiRequestModel;

export type UpdateProductItemModel = {
  ProductItem: ProductItemDto;
} & EnvironmentScopedApiRequestModel &
  ProductScopedApiRequestModel;

export type CreateProductModel = {
  Product: ProductPostDto;
} & EnvironmentScopedApiRequestModel;

export type CreateTaskModel = {
  Task: TaskDto;
} & EnvironmentScopedApiRequestModel &
  ProductScopedApiRequestModel;

export type EnvironmentScopedApiRequestModel = {
  EnvironmentKey: string;
  AccessToken: string;
};

export type ProductScopedApiRequestModel = {
  ProductKey: string;
};

// DTO definitions from Sopheon.CloudNative.Products
// TODO Cloud-2147, generate from OpenAPI spec

// TODO - (for domain/DTO types like Product), this file should serve as a temporary stub for Types for DTOs in the Sopheon.CloudNative.Products WebAPI project
// In Cloud-2147, we should generate TS Types from Sopheon.CloudNative.Products.AspNetCore OpenAPI schema

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
  productKey: string | undefined; // INFO: used for Redux state assignment after create Task API call
  name: string;
  notes: string | null;
  dueDate: Date | null;
  status: Status;
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

export interface TaskDto {
  id: number | undefined;
  name: string;
  notes: string | null;
  status: number | null;
  dueDate: string | undefined;
}

// TODO: Tech Debt - these dtos come directly from our data model, which I don't think our UI should know about
export interface Int32AttributeValueDto {
  AttributeId: number;
  Value: number | null;
}

export interface EnumAttributeOptionValueDto {
  enumAttributeOptionId: number;
}
export interface EnumCollectionAttributeValueDto {
  attributeId: number;
  value: EnumAttributeOptionValueDto[];
}

export interface ProductPostDto {
  Name: string;
  EnumCollectionAttributeValues: EnumCollectionAttributeValueDto[];
}

export interface EnumAttributeValueDto {
  attributeId: number;
  enumAttributeOptionId: number;
}

export interface UtcDateAttributeValueDto {
  attributeId: number;
  value: string | undefined;
}

export interface StringAttributeValueDto {
  attributeId: number;
  value: string;
}

export interface ProductItemDto {
  id: number;
  name?: string;
  utcDateTimeAttributeValues?: UtcDateAttributeValueDto[];
  stringAttributeValues?: StringAttributeValueDto[];
  enumAttributeValues?: EnumAttributeValueDto[];
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
