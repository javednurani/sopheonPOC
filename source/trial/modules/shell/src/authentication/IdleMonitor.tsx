import { IPublicClientApplication } from '@azure/msal-browser';
import { useMsal } from '@azure/msal-react';
import { useBoolean } from '@fluentui/react-hooks';
import React, { FunctionComponent } from 'react';
import { useIdleTimer } from 'react-idle-timer';

import { IdleTimeoutSettings } from './../settings/appSettings';
import AutoLogOutCountdown from './AutoLogOutCountdown';

// Show AutoLogOutCountdown dialog if any accounts are logged in on idle
export const handleOnIdle = (msalInstance: IPublicClientApplication, toggleHideDialog: () => void): void => {
  if (msalInstance.getAllAccounts().length > 0) {
    toggleHideDialog();
  }
};

const IdleMonitor: FunctionComponent = () => {
  const { instance } = useMsal();
  const [hideDialog, { toggle: toggleHideDialog }] = useBoolean(true);

  useIdleTimer({
    timeout: (IdleTimeoutSettings.IdleLogOutSeconds - IdleTimeoutSettings.IdleLogOutWarningSeconds) * 1000, // time til show warning converted from s to ms
    onIdle: () => handleOnIdle(instance, toggleHideDialog),
    crossTab: {
      emitOnAllTabs: true, // prevents user from being logged out of a second app tab
    },
  });

  return <AutoLogOutCountdown hidden={hideDialog} />;
};

export default IdleMonitor;
