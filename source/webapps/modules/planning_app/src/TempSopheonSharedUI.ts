import { createTheme } from '@fluentui/react';
import { Theme } from '@fluentui/theme';
import { Reducer, Store } from 'redux';
import { Saga } from 'redux-saga';

// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
// THIS IS A TEMPORARY FILE TO SHARE FILES WITH THE TEMPLATE APP
// THESE FILES ARE NORMALLY ACCESSIBLE IN THE @sopheon/shared-ui PACKAGE
// ONCE THIS APP IS ADDED TO A SHELL WITH THAT PACKAGE
// THIS FILE SHOULD BE DELETED AND IMPORTS REPLACED WITH THE PACKAGE
// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

export interface IEnhancedStore extends Store {
  injectReducer?: (key: string, asyncReducer: Reducer<any>) => void;
  injectSaga?: (key: string, asyncSaga: Saga<any>) => void;
  asyncReducers?: { [key: string]: Reducer };
  asyncSagas?: { [key: string]: Saga };
}

const lightTheme: Theme = createTheme({
  palette: {
    themePrimary: '#72379e',
    themeLighterAlt: '#f8f5fb',
    themeLighter: '#e5d7ef',
    themeLight: '#cfb6e2',
    themeTertiary: '#a478c5',
    themeSecondary: '#8049aa',
    themeDarkAlt: '#66328e',
    themeDark: '#562a78',
    themeDarker: '#401f59',
    neutralLighterAlt: '#faf9f8',
    neutralLighter: '#f3f2f1',
    neutralLight: '#edebe9',
    neutralQuaternaryAlt: '#e1dfdd',
    neutralQuaternary: '#d0d0d0',
    neutralTertiaryAlt: '#c8c6c4',
    neutralTertiary: '#595959',
    neutralSecondary: '#373737',
    neutralPrimaryAlt: '#2f2f2f',
    neutralPrimary: '#000000',
    neutralDark: '#151515',
    black: '#0b0b0b',
    white: '#ffffff',
  },
});
lightTheme.id = 'lightTheme';

const messages: Record<string, Record<string, string>> = {
  en: {
    'app.welcome': 'welcome to app',
    'app.add5': 'Add 5',
    'app.add5_aria': 'Increment value by 5',
    'aria.increment': 'Increment',
    'aria.decrement': 'Decremement',
    'fallback.loading': 'Loading...',
  },
};

export { lightTheme, messages };
