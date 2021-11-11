import { createPayloadAction, PayloadAction } from '@sopheon/shell-api';
import { Reducer } from 'redux';

import { AuthShape } from '../types';

//#region Action Types

// REDUCER ACTION TYPES

// eslint-disable-next-line no-shadow
export enum AuthActionTypes {
  SET_ENVIRONMENTKEY = 'SHELL/SET_ENVIRONMENTKEY',
}

export type SetEnvironmentKeyAction = PayloadAction<AuthActionTypes.SET_ENVIRONMENTKEY, string>;

export type AuthActions = SetEnvironmentKeyAction;

//#endregion

//#region Action Creators

export const setEnvironmentKey = (environmentKey: string): SetEnvironmentKeyAction => createPayloadAction(AuthActionTypes.SET_ENVIRONMENTKEY, environmentKey);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export const initialState: AuthShape = {
  environmentKey: null,
};

// HANDLERS

const setEnvironmentKeyHandler = (state: AuthShape, environmentKeyToSet: string) => ({
  ...state,
  environmentKey: environmentKeyToSet,
});

// ACTION SWITCH

export const authReducer: Reducer<AuthShape, AuthActions> = (state = initialState, action) => {
  switch (action.type) {
    case AuthActionTypes.SET_ENVIRONMENTKEY:
      return setEnvironmentKeyHandler(state, action.payload);
    default:
      return state;
  }
};
