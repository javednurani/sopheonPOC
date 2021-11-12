import { Action } from '../types';

// SAGA ACTION TYPES

// eslint-disable-next-line no-shadow
export enum AuthSagaActionTypes {
  GET_ACCESS_TOKEN = 'SHELL/GET_ACCESS_TOKEN'
}

export type GetAccessTokenAction = Action<AuthSagaActionTypes.GET_ACCESS_TOKEN>;
