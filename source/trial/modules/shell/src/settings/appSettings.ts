import { isDev } from './environmentSettings';

const appRawSettings: Record<string, string> = {
  IdleLogOutSeconds: '^IdleLogOutSeconds^',
  IdleLogOutWarningSeconds: '^IdleLogOutWarningSeconds^',
};

// these collapsed settings incorporate the current environment
export const IdleTimeoutSettings: Record<string, number> = {
  IdleLogOutSeconds: isDev ? 3600 : parseInt(appRawSettings.IdleLogOutSeconds, 10), // 1 hour timeout
  IdleLogOutWarningSeconds: isDev ? 60 : parseInt(appRawSettings.IdleLogOutWarningSeconds, 10), // show auto log out warning at 10 seconds left
};
