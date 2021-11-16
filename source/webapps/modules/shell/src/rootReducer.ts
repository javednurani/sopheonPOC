import { combineReducers } from 'redux';

import { authReducer } from './authentication/authReducer';
import { productReducer } from './product/productReducer';
import { themeReducer } from './themes/themeReducer/themeReducer';

export const shell = combineReducers({
  auth: authReducer,
  product: productReducer,
  theme: themeReducer,
});
