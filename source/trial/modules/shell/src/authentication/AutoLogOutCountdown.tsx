import { useMsal } from '@azure/msal-react';
import { DefaultButton, Dialog, DialogFooter, DialogType, PrimaryButton } from '@fluentui/react';
import React, { FunctionComponent, useEffect, useState } from 'react';

import { showAutoLogOutWarningThreshholdSeconds } from '../settings/appSettings';

const AutoLogOutCountdown: FunctionComponent = () => {
  // example: https://developer.microsoft.com/en-us/fluentui#/controls/web/dialog
  const [seconds, setSeconds] = useState(showAutoLogOutWarningThreshholdSeconds);
  const { instance } = useMsal();

  useEffect(() => {
    if (seconds > 0) {
      setTimeout(() => setSeconds(seconds - 1), 1000);
    } else {
      instance.logout();
    }
  });
  const dialogContentProps = {
    type: DialogType.normal,
    subText: `Are you still working? You will be logged out in ${seconds} seconds.`,
  };

  return (
    <Dialog hidden={false} dialogContentProps={dialogContentProps}>
      <DialogFooter>
        <PrimaryButton text="Yes" />
        <DefaultButton text="No" />
      </DialogFooter>
    </Dialog>
  );
};

export default AutoLogOutCountdown;
