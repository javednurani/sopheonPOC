
import { createAction, DisplayActionTypes, HideHeaderFooterAction, ShowHeaderFooterAction } from '@sopheon/shell-api';
import { Reducer } from 'redux';

import { DisplayShape } from '../types';
//#region Action Types

// REDUCER ACTION TYPES

export type DisplayActions = HideHeaderFooterAction | ShowHeaderFooterAction;

//#endregion

//#region Action Creators
export const showHeaderFooter = (): ShowHeaderFooterAction => createAction(DisplayActionTypes.SHOW_HEADER_FOOTER);
export const hideHeaderFooter = (): HideHeaderFooterAction => createAction(DisplayActionTypes.HIDE_HEADER_FOOTER);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export const initialState: DisplayShape = {
  headerFooterAreShown: true,
};

// HANDLERS

const showHeaderFooterHandler = (state: DisplayShape) => ({
  ...state,
  headerFooterAreShown: true,
});

const hideHeaderFooterHandler = (state: DisplayShape) => ({
  ...state,
  headerFooterAreShown: false,
});

// ACTION SWITCH

export const displayReducer: Reducer<DisplayShape, DisplayActions> = (state = initialState, action) => {
  switch (action.type) {
    case DisplayActionTypes.SHOW_HEADER_FOOTER:
      return showHeaderFooterHandler(state);
    case DisplayActionTypes.HIDE_HEADER_FOOTER:
      return hideHeaderFooterHandler(state);
    default:
      return state;
  }
};

//#endregion
