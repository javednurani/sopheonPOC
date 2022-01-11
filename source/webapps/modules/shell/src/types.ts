import { ITheme } from '@fluentui/react';
import { ShowAnnouncementModel } from '@sopheon/shell-api';

export type State = {
  shell: ShellState;
};

type ShellState = {
  theme: ThemeShape;
  auth: AuthShape;
  display: DisplayShape;
};

export interface ThemeShape {
  theme: ITheme;
}

export interface AuthShape {
  environmentKey: string | null;
  accessToken: string | null;
}

export interface LanguageShape {
  locale: string;
  messages: Record<string, string>;
  direction: string;
}

export interface DisplayShape {
  headerShown: boolean;
  announcementShown: boolean;
  announcementContent: ShowAnnouncementModel | null;
}
export interface NavBarItem {
  resourceKey: string;
  routeUrl: string;
  iconName: string;
}
