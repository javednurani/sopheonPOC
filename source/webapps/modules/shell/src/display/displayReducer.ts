import { createAction, DisplayActionTypes, HideHeaderAction, ShowHeaderAction } from '@sopheon/shell-api';
import { Reducer } from 'redux';

import { DisplayShape } from '../types';
//#region Action Types

// REDUCER ACTION TYPES

export type DisplayActions = HideHeaderAction | ShowHeaderAction;

//#endregion

//#region Action Creators
export const showHeader = (): ShowHeaderAction => createAction(DisplayActionTypes.SHOW_HEADER);
export const hideHeader = (): HideHeaderAction => createAction(DisplayActionTypes.HIDE_HEADER);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export const initialState: DisplayShape = {
  headerShown: true,
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

// ACTION SWITCH

export const displayReducer: Reducer<DisplayShape, DisplayActions> = (state = initialState, action) => {
  switch (action.type) {
    case DisplayActionTypes.SHOW_HEADER:
      return showHeaderHandler(state);
    case DisplayActionTypes.HIDE_HEADER:
      return hideHeaderHandler(state);
    default:
      return state;
  }
};

//#endregion
