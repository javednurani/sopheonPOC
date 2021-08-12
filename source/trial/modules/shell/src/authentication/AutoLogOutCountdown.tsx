import { useMsal } from '@azure/msal-react';
import { DefaultButton, Dialog, DialogFooter, DialogType, PrimaryButton } from '@fluentui/react';
import React, { FunctionComponent, useEffect } from 'react';
import { useIntl } from 'react-intl';

import { showAutoLogOutWarningThreshholdSeconds } from '../settings/appSettings';

const AutoLogOutCountdown: FunctionComponent = () => {
  const { formatMessage } = useIntl();
  // example: https://developer.microsoft.com/en-us/fluentui#/controls/web/dialog
  const [seconds, setSeconds] = React.useState(showAutoLogOutWarningThreshholdSeconds);
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
        <PrimaryButton text={formatMessage({ id: 'yes' })} />
        <DefaultButton text={formatMessage({ id: 'no' })} />
      </DialogFooter>
    </Dialog>
  );
};

export default AutoLogOutCountdown;
