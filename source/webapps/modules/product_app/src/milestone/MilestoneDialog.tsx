import {
  DefaultButton,
  Dialog,
  DialogFooter,
  DialogType,
  FontWeights,
  IButtonStyles,
  IconButton,
  IIconProps,
  IStackItemStyles,
  IStackStyles,
  IStackTokens,
  mergeStyleSets,
  PrimaryButton,
  Stack,
  Text,
  TextField,
} from '@fluentui/react';
import { useBoolean } from '@fluentui/react-hooks';
import { useTheme } from '@fluentui/react-theme-provider';
import { DatePicker } from '@sopheon/controls';
import React, { useEffect, useState } from 'react';
import { useIntl } from 'react-intl';

import { UpdateProductAction } from '../product/productReducer';
import { UpdateProductModel } from '../types';

export interface IMilestoneDialogProps {
  hideModal: () => void;
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  environmentKey: string;
  accessToken: string;
  // TODO: may need product key?
}

const MilestoneDialog: React.FunctionComponent<IMilestoneDialogProps> = ({ hideModal, environmentKey, accessToken }: IMilestoneDialogProps) => {
  const theme = useTheme();
  const { formatMessage } = useIntl();

  const [saveButtonDisabled, setSaveButtonDisabled] = useState(false);
  const [hideDiscardDialog, { toggle: toggleHideDiscardDialog }] = useBoolean(true);

  const [name, setName] = useState<string | undefined>(undefined);
  const [date, setDate] = useState<Date | undefined>(undefined);
  const [notes, setNotes] = useState<string | undefined>(undefined);

  const [nameDirty, setNameDirty] = useState(false);
  const [formDirty, setFormDirty] = useState(false);

  useEffect(() => {
    setSaveButtonDisabled(name ? false : true);
  }, [name]);

  const contentStyles = mergeStyleSets({
    header: [
      theme.fonts.xxLarge,
      {
        flex: '1 1 auto',
        borderTop: `4px solid ${theme.palette.themePrimary}`,
        color: theme.palette.neutralPrimary,
        display: 'flex',
        alignItems: 'center',
        fontWeight: FontWeights.semibold,
        padding: '32px',
      },
    ],
    body: {
      flex: '4 4 auto',
      padding: '32px',
      overflowY: 'hidden',
      selectors: {
        'p': { margin: '14px 0' },
        'p:first-child': { marginTop: 0 },
        'p:last-child': { marginBottom: 0 },
      },
    },
  });

  const iconButtonStyles: Partial<IButtonStyles> = {
    root: {
      color: theme.palette.neutralPrimary,
      marginLeft: 'auto',
    },
    rootHovered: {
      color: theme.palette.neutralDark,
    },
  };

  const cancelIcon: IIconProps = { iconName: 'Cancel' };

  const handleSaveButtonClick = () => {
    //hideModal();
    exitModalWithDiscardDialog();
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

  const discardDialogModalPropsStyles = { main: { maxWidth: 450 } };

  const discardDialogModalProps = {
    isBlocking: true,
    styles: discardDialogModalPropsStyles,
    dragOptions: undefined,
  };

  const discardDialogContentProps = {
    type: DialogType.normal,
    title: formatMessage({ id: 'toDo.discardthistask' }),
    subText: formatMessage({ id: 'unsaveddata' }),
  };

  const confirmDiscard = (): void => {
    toggleHideDiscardDialog();
    hideModal();
  };

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

  const stackStyles: IStackStyles = {
    root: {
      height: '100%',
      width: '100%',
    },
  };

  const mainStackTokens: IStackTokens = {
    childrenGap: 5,
  };

  const nestedStackTokens: IStackTokens = {
    childrenGap: 10,
  };

  const wideLeftStackItemStyles: IStackItemStyles = {
    root: {
      justifyContent: 'left',
      width: '66%',
      padding: '0px 30px 0px 0px',
    },
  };

  const fullWidthControlStyle: React.CSSProperties = {
    width: '100%',
  };

  const saveButtonStyle: React.CSSProperties = {
    marginRight: '10px',
  };

  const controlButtonStackItemStyles: IStackItemStyles = {
    root: {
      margin: '30px 0px 0px 0px',
    },
  };

  return (
    <>
      <div className={contentStyles.header}>
        <span id="TaskDetailsModal">
          <Text variant="xxLarge">{formatMessage({ id: 'milestone.newmilestone' })}</Text>
        </span>
        <IconButton styles={iconButtonStyles} iconProps={cancelIcon} ariaLabel={formatMessage({ id: 'closemodal' })} onClick={handleCloseIconClick} />
      </div>
      <div className={contentStyles.body}>
        <Stack styles={stackStyles} tokens={mainStackTokens}>
          <Stack.Item>
            <Stack horizontal styles={stackStyles} tokens={nestedStackTokens}>
              <Stack.Item styles={wideLeftStackItemStyles}>
                <TextField
                  style={fullWidthControlStyle}
                  placeholder={formatMessage({ id: 'toDo.tasknameplaceholder' })}
                  onChange={handleNameChange}
                  required
                  maxLength={150}
                  label={formatMessage({ id: 'name' })}
                  value={name}
                  onGetErrorMessage={value => (nameDirty && !value ? formatMessage({ id: 'fieldisrequired' }) : undefined)}
                />
              </Stack.Item>
              <Stack.Item>
                <DatePicker value={date} onSelectDate={handleDateChange} />
              </Stack.Item>
            </Stack>
          </Stack.Item>
          <Stack.Item>
            <Stack horizontal styles={stackStyles} tokens={nestedStackTokens}>
              <Stack.Item styles={wideLeftStackItemStyles}>
                <TextField
                  placeholder={formatMessage({ id: 'milestone.notesplaceholder' })}
                  onChange={handleNotesChange}
                  multiline
                  maxLength={5000}
                  rows={13}
                  resizable={false}
                  label={formatMessage({ id: 'milestone.notes' })}
                  value={notes}
                />
              </Stack.Item>
            </Stack>
          </Stack.Item>
          <Stack.Item>
            <Stack horizontal styles={stackStyles} tokens={nestedStackTokens}>
              <Stack.Item styles={controlButtonStackItemStyles}>
                <PrimaryButton
                  style={saveButtonStyle}
                  text={formatMessage({ id: 'save' })}
                  onClick={handleSaveButtonClick}
                  disabled={saveButtonDisabled || !formDirty}
                />
                <DefaultButton text={formatMessage({ id: 'cancel' })} onClick={handleCancelButtonClick} />
              </Stack.Item>
            </Stack>
          </Stack.Item>
        </Stack>
      </div>
      <Dialog
        hidden={hideDiscardDialog}
        onDismiss={toggleHideDiscardDialog}
        dialogContentProps={discardDialogContentProps}
        modalProps={discardDialogModalProps}
      >
        <DialogFooter>
          <PrimaryButton onClick={confirmDiscard} text={formatMessage({ id: 'discard' })} />
          <DefaultButton onClick={toggleHideDiscardDialog} text={formatMessage({ id: 'cancel' })} />
        </DialogFooter>
      </Dialog>
    </>
  );
};

export default MilestoneDialog;
