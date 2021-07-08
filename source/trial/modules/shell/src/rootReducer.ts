import { combineReducers } from 'redux';

import { themeReducer } from './themes/themeReducer/themeReducer';

export const shell = combineReducers({
  theme: themeReducer,
});
