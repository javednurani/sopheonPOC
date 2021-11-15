import { combineReducers } from 'redux';

import { authReducer } from './authentication/authReducer';
import { themeReducer } from './themes/themeReducer/themeReducer';

export const shell = combineReducers({
  theme: themeReducer,
  auth: authReducer,
});
