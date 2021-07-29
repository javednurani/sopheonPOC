const isProd = process.env.NODE_ENV === 'production';

export type AppModule = {
  //module: string; // The module to be be imported, this is always should be './App' for apps
  scope: string; // The scope of the module
  url: string; // The URL to the remoteEntry.js

  displayNameResourceKey: string; // The resource key for display name of the app, used for instance in links
  //name: string; // The full name of the app: <scope>__<version>[__<label>]
  //namespace: string; // The Redux namespace for the app: <scope>[-<label>]
  routeName: string; // The routing path to the app: /<scope>[-<label>]
  iconName: string; // name of icon used in navbar
  rank: number; // The order the apps should be listed in the navbar. Rank -> Alphabetical.
  applicationDefinitionId?: number; // for AppModules that correspond to ApplicationDefinitions
  isHomePage?: boolean; // for the singular AppModule that lives at / root url
};

// INFO: this simulates a login/claims webservice that could return available apps for user
export const appModules: AppModule[] = [
  {
    displayNameResourceKey: 'nav.app',
    scope: 'appModule',
    url: isProd ? '/app/remoteEntry.js' : 'https://localhost:3001/remoteEntry.js',
    routeName: '/app',
    iconName: 'CircleAddition',
    rank: 1,
    applicationDefinitionId: 1,
  },
];