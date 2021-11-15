// TODO - this file should serve as a temporary stub for Types for DTOs in the Sopheon.CloudNative.Products WebAPI project
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
