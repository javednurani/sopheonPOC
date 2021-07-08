import { ITheme } from '@fluentui/react';

export type State = {
  shell: ShellState;
};

type ShellState = {
  theme: ThemeShape;
};

export interface ThemeShape {
  theme: ITheme;
}

export interface LanguageShape {
  locale: string;
  messages: Record<string, string>;
  direction: string;
}

export interface NavBarItem {
  resourceKey: string;
  routeUrl: string;
  iconName: string;
}
