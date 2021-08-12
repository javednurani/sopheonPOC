import { DefaultButton, Dialog, DialogFooter, DialogType, PrimaryButton } from '@fluentui/react';
import React, { FunctionComponent } from 'react';
import { useIntl } from 'react-intl';

const AutoLogOutCountdown: FunctionComponent = () => {
  const { formatMessage } = useIntl();
  // example: https://developer.microsoft.com/en-us/fluentui#/controls/web/dialog

  const dialogContentProps = {
    type: DialogType.normal,
    subText: 'Are you still working? You will be logged out in XX seconds.',
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
