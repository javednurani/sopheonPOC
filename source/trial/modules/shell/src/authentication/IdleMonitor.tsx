import { IPublicClientApplication } from '@azure/msal-browser';
import { AuthenticatedTemplate, useMsal } from '@azure/msal-react';
import React, { FunctionComponent, useEffect, useState } from 'react';
import { useIdleTimer } from 'react-idle-timer';

import { autoLogOutTime, showAutoLogOutWarningThreshhold } from '../settings/appSettings';

// log out any active accounts then the idle timeout limit is reached
export const handleOnIdle = (instance: IPublicClientApplication): void => {
  if (instance.getAllAccounts().length > 0) {
    instance.logoutRedirect();
  }
};

const IdleMonitor: FunctionComponent = () => {
  const { instance } = useMsal();
  const [autoLogOutCountdown, setRemaining] = useState(autoLogOutTime);

  const { getRemainingTime } = useIdleTimer({
    timeout: autoLogOutTime,
    onIdle: () => handleOnIdle(instance),
    crossTab: {
      emitOnAllTabs: true, // prevents user from being logged out of a second app tab
    },
  });

  useEffect(() => {
    // update remaining time display every second
    setInterval(() => {
      setRemaining(getRemainingTime());
    }, 1000);
  }, [getRemainingTime]);

  return (
    <AuthenticatedTemplate>
      <div data-testid="idleCountdown">
        {autoLogOutCountdown < showAutoLogOutWarningThreshhold ? `Auto Log Out in ${Math.ceil(autoLogOutCountdown / 1000)} seconds...` : ''}
      </div>
    </AuthenticatedTemplate>
  );
};

export default IdleMonitor;
