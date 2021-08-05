import { AuthenticatedTemplate, useMsal } from '@azure/msal-react';
import React, { FunctionComponent, useEffect, useState } from 'react';
import { useIdleTimer } from 'react-idle-timer';

const IdleMonitor: FunctionComponent = () => {
  const { instance } = useMsal();
  const autoLogOutTime = 15 * 1000; // 15 seconds
  const showAutoLogOutWarningThreshhold = 10 * 1000; // show auto log out warning at 10 seconds left
  const [autoLogOutCountdown, setRemaining] = useState(autoLogOutTime);

  // log out any active accounts then the idle timeout limit is reached
  const handleOnIdle = () => {
    if (instance.getAllAccounts().length > 0) {
      instance.logoutRedirect();
    }
  };

  const { getRemainingTime } = useIdleTimer({
    timeout: autoLogOutTime,
    onIdle: handleOnIdle,
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
      {autoLogOutCountdown < showAutoLogOutWarningThreshhold ? `Auto Log Out in ${Math.ceil(autoLogOutCountdown / 1000)} seconds...` : ''}
    </AuthenticatedTemplate>
  );
};

export default IdleMonitor;
