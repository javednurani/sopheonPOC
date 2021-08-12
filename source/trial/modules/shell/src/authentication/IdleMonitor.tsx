import { IPublicClientApplication } from '@azure/msal-browser';
import { AuthenticatedTemplate, useMsal } from '@azure/msal-react';
import React, { FunctionComponent } from 'react';
import { useIdleTimer } from 'react-idle-timer';

import { IdleTimeoutSettings } from './../settings/appSettings';
import AutoLogOutCountdown from './AutoLogOutCountdown';

// log out any active accounts then the idle timeout limit is reached
export const handleOnIdle = (instance: IPublicClientApplication): void => {
  if (instance.getAllAccounts().length > 0) {
    instance.logoutRedirect();
  }
};

const IdleMonitor: FunctionComponent = () => {
  const { instance } = useMsal();

  useIdleTimer({
    timeout: (IdleTimeoutSettings.IdleLogOutSeconds - IdleTimeoutSettings.IdleLogOutWarningSeconds) * 1000, // time til show warning converted from s to ms
    onIdle: () => handleOnIdle(instance),
    crossTab: {
      emitOnAllTabs: true, // prevents user from being logged out of a second app tab
    },
  });

  return (
    <AuthenticatedTemplate>
      <div data-testid="idleCountdown">
        <AutoLogOutCountdown />
      </div>
    </AuthenticatedTemplate>
  );
};

export default IdleMonitor;
