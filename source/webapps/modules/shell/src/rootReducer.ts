import { combineReducers } from 'redux';

import { authReducer } from './authentication/authReducer';
import { displayReducer } from './display/displayReducer';
import { themeReducer } from './themes/themeReducer/themeReducer';

export const shell = combineReducers({
  auth: authReducer,
  theme: themeReducer,
  display: displayReducer,
});
