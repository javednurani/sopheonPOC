import { isProd } from './environmentSettings';

const appRawSettings: Record<string, string> = {
  IdleLogOutSeconds: '10', // '^IdleLogOutSeconds^'
  IdleLogOutSecondsDev: '20', // 1 hour log out '3600'
  IdleLogOutWarningSeconds: '5', // '^IdleLogOutWarningSeconds^'
  IdleLogOutWarningSecondsDev: '10', // 60 second warning '60'
};

// these collapsed settings incorporate the current environment
export const IdleTimeoutSettings: Record<string, number> = {
  IdleLogOutSeconds: isProd ? parseInt(appRawSettings.IdleLogOutSeconds, 10) : parseInt(appRawSettings.IdleLogOutSecondsDev, 10),
  IdleLogOutWarningSeconds: isProd ? parseInt(appRawSettings.IdleLogOutWarningSeconds, 10) : parseInt(appRawSettings.IdleLogOutWarningSecondsDev, 10),
};
