
// REDUCER ACTION TYPES

import { Action } from '../types';

// eslint-disable-next-line no-shadow
export enum DisplayActionTypes {
  SHOW_HEADER_FOOTER = 'SHELL/DISPLAY/SHOW_HEADER_FOOTER',
  HIDE_HEADER_FOOTER = 'SHELL/DISPLAY/HIDE_HEADER_FOOTER',
}

export type ShowHeaderFooterAction = Action<DisplayActionTypes.SHOW_HEADER_FOOTER>;
export type HideHeaderFooterAction = Action<DisplayActionTypes.HIDE_HEADER_FOOTER>;
