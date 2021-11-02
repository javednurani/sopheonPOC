import { applyMiddleware, combineReducers, createStore } from 'redux';
import { composeWithDevTools } from 'redux-devtools-extension';
import createSagaMiddleware from 'redux-saga';

import { shell } from './rootReducer';
import { State } from './types';

// namespace shell reducers into shell group
// shell = shell concern only, no direct read/write access to MFEs exposed through shell-api
function createNamespacedReducer() {
  return combineReducers<State>({
    shell, // namespaced state for shared store "shell.___"
  });
}

export const sagaMiddleware = createSagaMiddleware();

export const store = createStore(createNamespacedReducer(), composeWithDevTools(applyMiddleware(sagaMiddleware)));

export type RootState = ReturnType<typeof store.getState>;
