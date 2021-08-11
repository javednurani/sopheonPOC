import { DefaultButton, Dialog, DialogFooter, DialogType, PrimaryButton } from '@fluentui/react';
import React, { FunctionComponent } from 'react';

const AutoLogOutCountdown: FunctionComponent = () => {
  // example: https://developer.microsoft.com/en-us/fluentui#/controls/web/dialog

  const dialogContentProps = {
    type: DialogType.normal,
    title: 'Title??',   // TODO: cloud-1446
    subText: 'Are you still working? You will be logged out in XX seconds.',    // TODO: cloud-1446
  };

  return (
    <Dialog
      dialogContentProps={dialogContentProps}
    >
      <DialogFooter>
        <PrimaryButton text="Yes" />
        <DefaultButton text="No" />
      </DialogFooter>
    </Dialog>
  );
};

export default AutoLogOutCountdown;
