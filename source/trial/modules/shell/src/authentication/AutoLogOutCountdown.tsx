import { DefaultButton, Dialog, DialogFooter, PrimaryButton } from '@fluentui/react';
import React, { FunctionComponent } from 'react';

const AutoLogOutCountdown: FunctionComponent = () =>
// example: https://developer.microsoft.com/en-us/fluentui#/controls/web/dialog
  (
    <Dialog>
      <DialogFooter>
        <PrimaryButton text="Yes" />
        <DefaultButton text="No" />
      </DialogFooter>
    </Dialog>
  );

export default AutoLogOutCountdown;
