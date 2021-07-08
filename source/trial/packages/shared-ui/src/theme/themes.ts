import { createTheme } from '@fluentui/react';
import { Theme } from '@fluentui/theme';

// Themes were generated using
// https://fabricweb.z5.web.core.windows.net/pr-deploy-site/refs/heads/7.0/theming-designer/index.html

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

const darkTheme: Theme = createTheme({
  palette: {
    themePrimary: '#c798eb',
    themeLighterAlt: '#fdfbfe',
    themeLighter: '#f6eefc',
    themeLight: '#eedff9',
    themeTertiary: '#ddc0f3',
    themeSecondary: '#cda4ed',
    themeDarkAlt: '#b389d3',
    themeDark: '#9774b2',
    themeDarker: '#6f5583',
    neutralLighterAlt: '#323232',
    neutralLighter: '#3a3a3a',
    neutralLight: '#484848',
    neutralQuaternaryAlt: '#505050',
    neutralQuaternary: '#575757',
    neutralTertiaryAlt: '#747474',
    neutralTertiary: '#c8c8c8',
    neutralSecondary: '#d0d0d0',
    neutralPrimaryAlt: '#dadada',
    neutralPrimary: '#ffffff',
    neutralDark: '#f4f4f4',
    black: '#f8f8f8',
    white: '#2a2a2a',
  },
});
darkTheme.id = 'darkTheme';

export { lightTheme, darkTheme };
