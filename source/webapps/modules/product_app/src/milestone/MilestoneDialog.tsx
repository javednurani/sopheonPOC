import {
  DefaultButton,
  Dialog,
  DialogFooter,
  DialogType,
  IconButton,
  IIconProps,
  IStackTokens,
  PrimaryButton,
  Stack,
  Text,
  TextField,
} from '@fluentui/react';
import { useBoolean } from '@fluentui/react-hooks';
import { DatePicker } from '@sopheon/controls';
import React, { useEffect, useState } from 'react';
import { useIntl } from 'react-intl';

import { CreateMilestoneAction } from '../product/productReducer';
import { MilestoneDto, PostMilestoneModel } from '../types';

export interface IMilestoneDialogProps {
  accessToken: string;
  createMilestone: (milestone: PostMilestoneModel) => CreateMilestoneAction;
  environmentKey: string;
  hideModal: () => void;
  productKey: string;
}

const MilestoneDialog: React.FunctionComponent<IMilestoneDialogProps> = ({
  accessToken,
  createMilestone,
  environmentKey,
  hideModal,
  productKey,
}: IMilestoneDialogProps) => {
  const { formatMessage } = useIntl();

  const [saveButtonDisabled, setSaveButtonDisabled] = useState(false);
  const [hideDiscardDialog, { toggle: toggleHideDiscardDialog }] = useBoolean(true);

  const [name, setName] = useState<string | undefined>(undefined);
  const [date, setDate] = useState<Date | undefined>(undefined);
  const [notes, setNotes] = useState<string | undefined>(undefined);

  const [nameDirty, setNameDirty] = useState(false);
  const [formDirty, setFormDirty] = useState(false);

  useEffect(() => {
    setSaveButtonDisabled(name && date ? false : true);
  }, [name, date]);

  const cancelIcon: IIconProps = { iconName: 'Cancel' };

  const stackChildGapTokens: IStackTokens = {
    childrenGap: 8,
  };

  const handleSaveButtonClick = () => {
    // create new milestone
    const milestone: MilestoneDto = {
      id: 0,
      name: name as string,
      notes: notes === undefined ? null : notes,
      date: date ? date.toDateString() : null,
    };
    const createMilestoneModel: PostMilestoneModel = {
      ProductKey: productKey,
      EnvironmentKey: environmentKey,
      AccessToken: accessToken,
      Milestone: milestone,
    };
    createMilestone(createMilestoneModel);
    hideModal();
  };

  // CANCEL BUTTON
  const handleCancelButtonClick = () => {
    exitModalWithDiscardDialog();
  };

  // CLOSE ICON
  const handleCloseIconClick = () => {
    exitModalWithDiscardDialog();
  };

  // DISCARD DIALOG

  const discardDialogContentProps = {
    type: DialogType.normal,
    title: formatMessage({ id: 'discardChanges' }),
    subText: formatMessage({ id: 'unsavedChangesLost' }),
  };

  const confirmDiscard = (): void => {
    toggleHideDiscardDialog();
    hideModal();
  };

  const confirmCancelDialog = (
    <Dialog hidden={hideDiscardDialog} onDismiss={toggleHideDiscardDialog} dialogContentProps={discardDialogContentProps}>
      <DialogFooter>
        <PrimaryButton onClick={confirmDiscard} text={formatMessage({ id: 'discard' })} />
        <DefaultButton onClick={toggleHideDiscardDialog} text={formatMessage({ id: 'cancel' })} />
      </DialogFooter>
    </Dialog>
  );

  const exitModalWithDiscardDialog = (): void => {
    if (formDirty) {
      toggleHideDiscardDialog();
    } else {
      hideModal();
    }
  };

  const handleNameChange = (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string | undefined): void => {
    setName(newValue);
    setNameDirty(true);
    setFormDirty(true);
  };

  const handleDateChange = (newValue: Date | null | undefined): void => {
    if (newValue) {
      setDate(newValue);
      setFormDirty(true);
    }
  };

  const handleNotesChange = (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string | undefined): void => {
    setNotes(newValue);
    setFormDirty(true);
  };

  return (
    <>
      <Stack tokens={stackChildGapTokens}>
        <Stack horizontal>
          <Stack.Item grow>
            <Text variant="xxLarge">{formatMessage({ id: 'milestone.newmilestone' })}</Text>
          </Stack.Item>
          <Stack.Item>
            <IconButton iconProps={cancelIcon} ariaLabel={formatMessage({ id: 'closemodal' })} onClick={handleCloseIconClick} />
          </Stack.Item>
        </Stack>
        <TextField
          placeholder={formatMessage({ id: 'milestone.namePlaceholder' })}
          onChange={handleNameChange}
          required
          maxLength={150}
          label={formatMessage({ id: 'name' })}
          value={name}
          onGetErrorMessage={value => (nameDirty && !value ? formatMessage({ id: 'fieldisrequired' }) : undefined)}
        />
        <DatePicker value={date} onSelectDate={handleDateChange} required={true} />
        <TextField
          placeholder={formatMessage({ id: 'milestone.notesPlaceholder' })}
          onChange={handleNotesChange}
          multiline
          maxLength={5000}
          rows={9}
          resizable={false}
          label={formatMessage({ id: 'milestone.notes' })}
          value={notes}
        />
        <Stack horizontal tokens={stackChildGapTokens}>
          <PrimaryButton text={formatMessage({ id: 'save' })} onClick={handleSaveButtonClick} disabled={saveButtonDisabled || !formDirty} />
          <DefaultButton text={formatMessage({ id: 'cancel' })} onClick={handleCancelButtonClick} />
        </Stack>
      </Stack>
      {confirmCancelDialog}
    </>
  );
};

export default MilestoneDialog;
