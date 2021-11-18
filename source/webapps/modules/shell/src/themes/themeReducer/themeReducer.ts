import { ITheme, loadTheme } from '@fluentui/react';
import { darkTheme, lightTheme } from '@sopheon/shared-ui';
import { createPayloadAction, PayloadAction } from '@sopheon/shell-api';
import { Reducer } from 'redux';

import { ThemeShape } from '../../types';

//#region Action Types

// REDUCER ACTION TYPES

// eslint-disable-next-line no-shadow
export enum ThemeActionTypes {
  CHANGE_THEME = 'SHELL/THEME/CHANGE_THEME',
}

export type ChangeThemeAction = PayloadAction<ThemeActionTypes.CHANGE_THEME, boolean>;

export type ThemeActions = ChangeThemeAction;

//#endregion

//#region Action Creators

export const changeTheme = (useDarkTheme: boolean): ChangeThemeAction => createPayloadAction(ThemeActionTypes.CHANGE_THEME, useDarkTheme);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

const defaultTheme = lightTheme;

export const initialState: ThemeShape = {
  theme: loadTheme(defaultTheme),
};

// HANDLERS

const changeThemeHandler = (state: ThemeShape, useDarkTheme: boolean) => ({
  ...state,
  theme: getTheme(useDarkTheme),
});

// HELPERS

const getTheme: (useDarkTheme: boolean) => ITheme = useDarkTheme => {
  if (useDarkTheme) {
    return loadTheme(darkTheme);
  }

  return loadTheme(lightTheme);
};

// ACTION SWITCH

export const themeReducer: Reducer<ThemeShape, ThemeActions> = (state = initialState, action) => {
  switch (action.type) {
    case ThemeActionTypes.CHANGE_THEME:
      return changeThemeHandler(state, action.payload);
    default:
      return state;
  }
};

//#endregion
