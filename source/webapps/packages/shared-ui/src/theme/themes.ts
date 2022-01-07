import { createTheme } from '@fluentui/react';
import { Theme } from '@fluentui/theme';

// Themes were generated using
// https://fabricweb.z5.web.core.windows.net/pr-deploy-site/refs/heads/7.0/theming-designer/index.html

const defaultFontStyle = { fontFamily: "'Source Sans Pro', sans-serif" };

const lightTheme: Theme = createTheme({
  defaultFontStyle: defaultFontStyle,
  palette: {
    themePrimary: '#33596f',
    themeLighterAlt: '#f4f7f9',
    themeLighter: '#d4e1e8',
    themeLight: '#b2c8d4',
    themeTertiary: '#7295a9',
    themeSecondary: '#446b81',
    themeDarkAlt: '#2e5165',
    themeDark: '#274455',
    themeDarker: '#1d323f',
    neutralLighterAlt: '#faf9f8',
    neutralLighter: '#f3f2f1',
    neutralLight: '#edebe9',
    neutralQuaternaryAlt: '#e1dfdd',
    neutralQuaternary: '#d0d0d0',
    neutralTertiaryAlt: '#c8c6c4',
    neutralTertiary: '#91a3b0',
    neutralSecondary: '#2c3d4a',
    neutralPrimaryAlt: '#596e7d',
    neutralPrimary: '#0b1217',
    neutralDark: '#2c3d4a',
    black: '#1a2730',
    white: '#ffffff',
  },
});
lightTheme.id = 'lightTheme';

const darkTheme: Theme = createTheme({
  defaultFontStyle: defaultFontStyle,
  palette: {
    themePrimary: '#61a39f',
    themeLighterAlt: '#f7fbfb',
    themeLighter: '#e1f0ef',
    themeLight: '#c8e3e2',
    themeTertiary: '#98c8c5',
    themeSecondary: '#71aeaa',
    themeDarkAlt: '#58938f',
    themeDark: '#4a7c79',
    themeDarker: '#375b59',
    neutralLighterAlt: '#1c1c1c',
    neutralLighter: '#252525',
    neutralLight: '#343434',
    neutralQuaternaryAlt: '#3d3d3d',
    neutralQuaternary: '#454545',
    neutralTertiaryAlt: '#656565',
    neutralTertiary: '#f5f5f5',
    neutralSecondary: '#f6f6f6',
    neutralPrimaryAlt: '#f8f8f8',
    neutralPrimary: '#f0f0f0',
    neutralDark: '#fbfbfb',
    black: '#fdfdfd',
    white: '#121212',
  },
});
darkTheme.id = 'darkTheme';

const isDarkTheme = (theme: Theme): boolean => theme.id?.includes(darkTheme.id ?? 'darkTheme') || false;

export { lightTheme, darkTheme, isDarkTheme };
