import { PRODUCT_MANAGEMENT_API_BASE_URL } from '../../../devEnvSettings';

const isDev = process.env.NODE_ENV === 'development';

// INFO: ^token^ is meant to be replaced in Deploy CI
const productManagementApiUrlBase = isDev ? PRODUCT_MANAGEMENT_API_BASE_URL : '^ProductManagementApiBaseUrl^';

// INFO: {*token*} is meant to be replaced in React SPA
const tokenEnvironmentKey = '{*environmentKey*}';
const tokenProductKey = '{*productKey*}';
const tokenProductItemId = '{*productItemKey*}';

// TODO, unify this file and product_app/settings.ts

// Public settings object
export const settings: Record<string, string> = {
  ProductManagementApiUrlBase: productManagementApiUrlBase,
  CreateProductUrlPath: `/Environments/${tokenEnvironmentKey}/Products`,
  getProductsUrlPath: `/Environments/${tokenEnvironmentKey}/Products`,
  UpdateProductUrlPath: `/Environments/${tokenEnvironmentKey}/Products/${tokenProductKey}`,
  UpdateProductItemUrlPath: `/Environments/${tokenEnvironmentKey}/ProductItems/Products/${tokenProductKey}/Items/${tokenProductItemId}`, // TODO: route is funky CLOUD-2469
  TokenEnvironmentKey: tokenEnvironmentKey,
  TokenProductKey: tokenProductKey,
  TokenProductItemId: tokenProductItemId,
};
