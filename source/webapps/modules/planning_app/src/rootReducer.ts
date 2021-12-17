import { combineReducers } from 'redux';

export const rootReducer = combineReducers({});

type AppState = unknown;

// The below NAMESPACE string, and RootState key, interact to provide Redux store nested namespacing.
//They should be identical, and reflective of module/microFrontEnd name, eg, 'app1', 'app3', environments'

export type RootState = {
  planningApp: AppState;
};

export const NAMESPACE = 'planning_app';
