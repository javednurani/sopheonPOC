/* eslint-disable */
import {
  DefaultButton,
  Dialog,
  DialogFooter,
  DialogType,
  Dropdown,
  FontWeights,
  IButtonStyles,
  IconButton,
  IDropdownOption,
  IDropdownStyles,
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

import { Status } from '../data/status';
import ExpandablePanel from '../ExpandablePanel';
import HistoryList from '../HistoryList';
import ProductApi from '../product/productApi';
import { CreateTaskAction, UpdateProductAction, UpdateProductItemAction, UpdateTaskAction } from '../product/productReducer';
import { HistoryItem, PostPutTaskModel, Product, Task, TaskDto, UpdateProductItemModel, UpdateProductModel, Milestone } from '../types';

export interface IMilestoneDialogProps {
  hideModal: () => void;
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  environmentKey: string;
  accessToken: string;
  products: Product[];
}

export interface DateStateObject {
  date: Date | null;
}

const MilestoneDialog: React.FunctionComponent<IMilestoneDialogProps> = ({
  hideModal,
  environmentKey,
  accessToken,
  products,
}: IMilestoneDialogProps) => {
  const theme = useTheme();
  const { formatMessage } = useIntl();

  const [saveButtonDisabled, setSaveButtonDisabled] = useState(false);

  const [formDirty, setFormDirty] = useState(false);

  const [hideDiscardDialog, { toggle: toggleHideDiscardDialog }] = useBoolean(true);

  useEffect(() => {
    setSaveButtonDisabled(true);
  }, ['']);

  const contentStyles = mergeStyleSets({
    header: [
      // eslint-disable-next-line deprecation/deprecation
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

  /* 
    TODO: Setup EVENT HANDLERS here.

    MilestoneName, MilestoneDate, MilestoneNotes
  */

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
                  // onChange={handleMilestoneNameChange}
                  required
                  maxLength={150}
                  label={formatMessage({ id: 'name' })}
                  value={''} // milestoneName
                  // TODO 1693 - possible taskName.errorMessage display pattern, remove if unneeded
                  // onGetErrorMessage={value => (taskNameDirty && !value ? formatMessage({ id: 'fieldisrequired' }) : undefined)}
                />
              </Stack.Item>
              <Stack.Item>
                <DatePicker value={undefined} /> {/* TODO:  onSelectDate={handleMilestoneDateChange} */}
              </Stack.Item>
            </Stack>
          </Stack.Item>
          <Stack.Item>
            <Stack horizontal styles={stackStyles} tokens={nestedStackTokens}>
              <Stack.Item styles={wideLeftStackItemStyles}>
                <TextField
                  placeholder={formatMessage({ id: 'milestone.notesplaceholder' })}
                  // onChange={handleMilestoneNotesChange}
                  multiline
                  maxLength={5000}
                  rows={13}
                  resizable={false}
                  label={formatMessage({ id: 'milestone.notes' })}
                  value={''} // milestoneNotes
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
