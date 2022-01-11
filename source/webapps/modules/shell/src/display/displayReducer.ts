import {
  createAction,
  createPayloadAction,
  DisplayActionTypes,
  HideAnnouncementAction,
  HideHeaderAction,
  ShowAnnouncementAction,
  ShowAnnouncementModel,
  ShowHeaderAction,
} from '@sopheon/shell-api';
import { Reducer } from 'redux';

import { DisplayShape } from '../types';
//#region Action Types

// REDUCER ACTION TYPES

export type DisplayActions = HideHeaderAction | ShowHeaderAction | ShowAnnouncementAction | HideAnnouncementAction;

//#endregion

//#region Action Creators
export const showHeader = (): ShowHeaderAction => createAction(DisplayActionTypes.SHOW_HEADER);
export const hideHeader = (): HideHeaderAction => createAction(DisplayActionTypes.HIDE_HEADER);

export const showAnnouncement = (announcement: ShowAnnouncementModel): ShowAnnouncementAction =>
  createPayloadAction(DisplayActionTypes.SHOW_ANNOUNCEMENT, announcement);
export const hideAnnouncement = (): HideAnnouncementAction => createAction(DisplayActionTypes.HIDE_ANNOUNCEMENT);
//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export const initialState: DisplayShape = {
  headerShown: true,
  announcementShown: false,
  announcementContent: null,
};

// HANDLERS

const showHeaderHandler = (state: DisplayShape) => ({
  ...state,
  headerShown: true,
});

const hideHeaderHandler = (state: DisplayShape) => ({
  ...state,
  headerShown: false,
});

const showAnnouncementHandler = (state: DisplayShape, announcement: ShowAnnouncementModel) => ({
  ...state,
  announcementShown: true,
  announcementContent: announcement,
});

const hideAnnouncementHandler = (state: DisplayShape) => ({
  ...state,
  announcementShown: false,
  announcementContent: null,
});

// ACTION SWITCH

export const displayReducer: Reducer<DisplayShape, DisplayActions> = (state = initialState, action) => {
  switch (action.type) {
    case DisplayActionTypes.SHOW_HEADER:
      return showHeaderHandler(state);
    case DisplayActionTypes.HIDE_HEADER:
      return hideHeaderHandler(state);
    case DisplayActionTypes.SHOW_ANNOUNCEMENT:
      return showAnnouncementHandler(state, action.payload);
    case DisplayActionTypes.HIDE_ANNOUNCEMENT:
      return hideAnnouncementHandler(state);
    default:
      return state;
  }
};

//#endregion
