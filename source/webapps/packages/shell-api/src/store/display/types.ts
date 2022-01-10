// REDUCER ACTION TYPES

import { Action } from '../types';

// eslint-disable-next-line no-shadow
export enum DisplayActionTypes {
  SHOW_HEADER = 'SHELL/DISPLAY/SHOW_HEADER',
  HIDE_HEADER = 'SHELL/DISPLAY/HIDE_HEADER',
}

export type ShowHeaderAction = Action<DisplayActionTypes.SHOW_HEADER>;
export type HideHeaderAction = Action<DisplayActionTypes.HIDE_HEADER>;
