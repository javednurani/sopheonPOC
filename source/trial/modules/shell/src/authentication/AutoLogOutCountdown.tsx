import { useMsal } from '@azure/msal-react';
import { DefaultButton, Dialog, DialogFooter, DialogType, PrimaryButton } from '@fluentui/react';
import React, { FunctionComponent, useEffect } from 'react';
import { useIntl } from 'react-intl';

import { IdleTimeoutSettings } from './../settings/appSettings';
export interface AutoLogoutCountdownProps {
  hidden?: boolean | undefined;
}

const AutoLogOutCountdown: FunctionComponent<AutoLogoutCountdownProps> = ({ hidden }: AutoLogoutCountdownProps) => {
  const { formatMessage } = useIntl();
  const [seconds, setSeconds] = React.useState(IdleTimeoutSettings.IdleLogOutWarningSeconds);
  const { instance } = useMsal();

  useEffect(() => {
    if (seconds > 0 && !hidden) {
      setTimeout(() => setSeconds(seconds - 1), 1000);
    } else if (seconds <= 0) {
      instance.logout();
    }
  });
  const dialogContentProps = {
    type: DialogType.normal,
    subText: `Are you still working? You will be logged out in ${seconds} seconds.`, // TODO: Resource this
  };

  return (
    <Dialog hidden={hidden} dialogContentProps={dialogContentProps}>
      <DialogFooter>
        <PrimaryButton text={formatMessage({ id: 'yes' })} />
        <DefaultButton text={formatMessage({ id: 'no' })} />
      </DialogFooter>
    </Dialog>
  );
};

export default AutoLogOutCountdown;
