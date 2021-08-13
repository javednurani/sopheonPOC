import { isProd } from './environmentSettings';

const appRawSettings: Record<string, string> = {
  IdleLogOutSeconds: '^IdleLogOutSeconds^',
  IdleLogOutSecondsDev: '3600', // 1 hour log out
  IdleLogOutWarningSeconds: '^IdleLogOutWarningSeconds^',
  IdleLogOutWarningSecondsDev: '60', // 60 second warning
};

// these collapsed settings incorporate the current environment
export const IdleTimeoutSettings: Record<string, number> = {
  IdleLogOutSeconds: isProd ? parseInt(appRawSettings.IdleLogOutSeconds, 10) : parseInt(appRawSettings.IdleLogOutSeconds, 10),
  IdleLogOutWarningSeconds: isProd ? parseInt(appRawSettings.IdleLogOutWarningSeconds, 10) : parseInt(appRawSettings.IdleLogOutWarningSecondsDev, 10),
};
