// REDUCER ACTION TYPES

import { Action, PayloadAction } from '../types';

export interface ShowAnnouncementModel {
  message: string;
  durationSeconds: number;
}

// eslint-disable-next-line no-shadow
export enum DisplayActionTypes {
  SHOW_HEADER = 'SHELL/DISPLAY/SHOW_HEADER',
  HIDE_HEADER = 'SHELL/DISPLAY/HIDE_HEADER',
  SHOW_ANNOUNCEMENT = 'SHELL/DISPLAY/SHOW_ANNOUNCEMENT',
  HIDE_ANNOUNCEMENT = 'SHELL/DISPLAY/HIDE_ANNOUNCEMENT',
}

export type ShowHeaderAction = Action<DisplayActionTypes.SHOW_HEADER>;
export type HideHeaderAction = Action<DisplayActionTypes.HIDE_HEADER>;

export type ShowAnnouncementAction = PayloadAction<DisplayActionTypes.SHOW_ANNOUNCEMENT, ShowAnnouncementModel>;
export type HideAnnouncementAction = Action<DisplayActionTypes.HIDE_ANNOUNCEMENT>;
