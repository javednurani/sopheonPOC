import { AuthSagaActionTypes, createAction, createPayloadAction, GetAccessTokenAction, PayloadAction } from '@sopheon/shell-api';
import { Reducer } from 'redux';

import { AuthShape } from '../types';

//#region Action Types

// REDUCER ACTION TYPES

// eslint-disable-next-line no-shadow
export enum AuthActionTypes {
  SET_ENVIRONMENTKEY = 'SHELL/SET_ENVIRONMENTKEY',
  SET_ACCESS_TOKEN = 'SHELL/SET_ACCESS_TOKEN',
}

export type SetEnvironmentKeyAction = PayloadAction<AuthActionTypes.SET_ENVIRONMENTKEY, string>;
export type SetAccessTokenAction = PayloadAction<AuthActionTypes.SET_ACCESS_TOKEN, string>;

export type AuthActions = SetEnvironmentKeyAction | SetAccessTokenAction;

//#endregion

//#region Action Creators

// REDUCER ACTIONS

export const setEnvironmentKey = (environmentKey: string): SetEnvironmentKeyAction => createPayloadAction(AuthActionTypes.SET_ENVIRONMENTKEY, environmentKey);
export const setAccessToken = (accessToken: string): SetAccessTokenAction => createPayloadAction(AuthActionTypes.SET_ACCESS_TOKEN, accessToken);

// SAGA ACTIONS

// TODO import saga action type from ShellApi
export const getAccessToken = (): GetAccessTokenAction => createAction(AuthSagaActionTypes.GET_ACCESS_TOKEN);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export const initialState: AuthShape = {
  environmentKey: null,
  accessToken: null,
};

// HANDLERS

const setEnvironmentKeyHandler = (state: AuthShape, environmentKeyToSet: string) => ({
  ...state,
  environmentKey: environmentKeyToSet,
});

const setAccessTokenHandler = (state: AuthShape, accessTokenToSet: string) => ({
  ...state,
  accessToken: accessTokenToSet,
});

// ACTION SWITCH

export const authReducer: Reducer<AuthShape, AuthActions> = (state = initialState, action) => {
  switch (action.type) {
    case AuthActionTypes.SET_ENVIRONMENTKEY:
      return setEnvironmentKeyHandler(state, action.payload);
    case AuthActionTypes.SET_ACCESS_TOKEN:
      return setAccessTokenHandler(state, action.payload);
    default:
      return state;
  }
};
