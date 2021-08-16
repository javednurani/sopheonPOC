import { useMsal } from '@azure/msal-react';
import { DefaultButton, Dialog, DialogFooter, DialogType, PrimaryButton } from '@fluentui/react';
import React, { FunctionComponent, useEffect } from 'react';
import { useIntl } from 'react-intl';

import { IdleTimeoutSettings } from './../settings/appSettings';
export interface AutoLogoutCountdownProps {
  hidden?: boolean | undefined;
  toggleHidden: () => void;
}

const AutoLogOutCountdown: FunctionComponent<AutoLogoutCountdownProps> = ({ hidden, toggleHidden }: AutoLogoutCountdownProps) => {
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
    subText: formatMessage({ id: 'auth.logoutwarning' }, { time: seconds }),
  };

  const onYesButtonClick = () => {
    toggleHidden();
    setTimeout(() => setSeconds(IdleTimeoutSettings.IdleLogOutWarningSeconds), 1000);
  };

  const onNoButtonClick = () => {
    instance.logout();
  };

  return (
    <Dialog hidden={hidden} dialogContentProps={dialogContentProps}>
      <DialogFooter>
        <PrimaryButton text={formatMessage({ id: 'yes' })} onClick={onYesButtonClick} />
        <DefaultButton text={formatMessage({ id: 'no' })} onClick={onNoButtonClick} />
      </DialogFooter>
    </Dialog>
  );
};

export default AutoLogOutCountdown;
