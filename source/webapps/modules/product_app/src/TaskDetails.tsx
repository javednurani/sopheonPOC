import {
  DatePicker,
  DayOfWeek,
  DefaultButton,
  Dialog,
  DialogFooter,
  DialogType,
  Dropdown,
  FontIcon,
  FontWeights,
  IButtonStyles,
  IconButton,
  IDatePickerStrings,
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
import React, { useEffect, useState } from 'react';
import { useIntl } from 'react-intl';

import { Status } from './data/status';
import ExpandablePanel from './ExpandablePanel';
import HistoryList from './HistoryList';
import ProductApi from './product/productApi';
import { CreateTaskAction, DeleteTaskAction, UpdateProductAction, UpdateProductItemAction, UpdateTaskAction } from './product/productReducer';
import { DeleteTaskModel, HistoryItem, PostPutTaskModel, Product, Task, TaskDto, UpdateProductItemModel, UpdateProductModel } from './types';

export interface ITaskDetailsProps {
  hideModal: () => void;
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  environmentKey: string;
  accessToken: string;
  products: Product[];
  selectedTask: Task | null;
  updateProductItem: (productItem: UpdateProductItemModel) => UpdateProductItemAction;
  createTask: (task: PostPutTaskModel) => CreateTaskAction;
  updateTask: (task: PostPutTaskModel) => UpdateTaskAction;
  deleteTask: (task: DeleteTaskModel) => DeleteTaskAction;
}

export interface DateStateObject {
  date: Date | undefined;
}

const TaskDetails: React.FunctionComponent<ITaskDetailsProps> = ({
  hideModal,
  environmentKey,
  accessToken,
  products,
  selectedTask,
  createTask,
  updateTask,
  deleteTask,
}: ITaskDetailsProps) => {
  const { name, id, notes, dueDate, status } = selectedTask ?? {};

  const theme = useTheme();
  const { formatMessage } = useIntl();

  const [taskName, setTaskName] = useState(name ?? '');

  const [taskHistory, setTaskHistory] = useState<HistoryItem[] | null>(null);

  const [taskNotes, setTaskNotes] = useState(notes ?? '');

  const [taskDueDate, setTaskDueDate] = useState<DateStateObject>({ date: dueDate ?? undefined });

  const [selectedItemStatusDropdown, setSelectedItemStatusDropdown] = useState(status ?? Status.NotStarted);

  const [saveButtonDisabled, setSaveButtonDisabled] = useState(false);

  const [taskNameDirty, setTaskNameDirty] = useState(false);

  const [formDirty, setFormDirty] = useState(false);

  const [hideDiscardDialog, { toggle: toggleHideDiscardDialog }] = useBoolean(true);

  useEffect(() => {
    setSaveButtonDisabled(taskName.length === 0);
  }, [taskName]);

  const contentStyles = mergeStyleSets({
    header: [
      // eslint-disable-next-line deprecation/deprecation
      theme.fonts.xLargePlus,
      {
        flex: '1 1 auto',
        borderTop: `4px solid ${theme.palette.themePrimary}`,
        color: theme.palette.neutralPrimary,
        display: 'flex',
        alignItems: 'center',
        fontWeight: FontWeights.semibold,
        padding: '25px 50px 10px 50px',
      },
    ],
    body: {
      flex: '4 4 auto',
      padding: '10px 50px 25px 50px',
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

  // DATEPICKER

  const firstDayOfWeek = DayOfWeek.Sunday;

  const datePickerStrings: IDatePickerStrings = {
    months: [
      formatMessage({ id: 'calendar.janlong' }),
      formatMessage({ id: 'calendar.feblong' }),
      formatMessage({ id: 'calendar.marlong' }),
      formatMessage({ id: 'calendar.aprlong' }),
      formatMessage({ id: 'calendar.maylong' }),
      formatMessage({ id: 'calendar.junlong' }),
      formatMessage({ id: 'calendar.jullong' }),
      formatMessage({ id: 'calendar.auglong' }),
      formatMessage({ id: 'calendar.seplong' }),
      formatMessage({ id: 'calendar.octlong' }),
      formatMessage({ id: 'calendar.novlong' }),
      formatMessage({ id: 'calendar.declong' }),
    ],

    shortMonths: [
      formatMessage({ id: 'calendar.jan' }),
      formatMessage({ id: 'calendar.feb' }),
      formatMessage({ id: 'calendar.mar' }),
      formatMessage({ id: 'calendar.apr' }),
      formatMessage({ id: 'calendar.may' }),
      formatMessage({ id: 'calendar.jun' }),
      formatMessage({ id: 'calendar.jul' }),
      formatMessage({ id: 'calendar.aug' }),
      formatMessage({ id: 'calendar.sep' }),
      formatMessage({ id: 'calendar.oct' }),
      formatMessage({ id: 'calendar.nov' }),
      formatMessage({ id: 'calendar.dec' }),
    ],

    days: [
      formatMessage({ id: 'calendar.sunlong' }),
      formatMessage({ id: 'calendar.monlong' }),
      formatMessage({ id: 'calendar.tuelong' }),
      formatMessage({ id: 'calendar.wedlong' }),
      formatMessage({ id: 'calendar.thulong' }),
      formatMessage({ id: 'calendar.frilong' }),
      formatMessage({ id: 'calendar.satlong' }),
    ],

    shortDays: [
      formatMessage({ id: 'calendar.sun' }),
      formatMessage({ id: 'calendar.mon' }),
      formatMessage({ id: 'calendar.tue' }),
      formatMessage({ id: 'calendar.wed' }),
      formatMessage({ id: 'calendar.thu' }),
      formatMessage({ id: 'calendar.fri' }),
      formatMessage({ id: 'calendar.sat' }),
    ],

    goToToday: formatMessage({ id: 'calendar.gototoday' }),
    prevMonthAriaLabel: formatMessage({ id: 'calendar.gotoprevmonth' }),
    nextMonthAriaLabel: formatMessage({ id: 'calendar.gotonextmonth' }),
    prevYearAriaLabel: formatMessage({ id: 'calendar.gotoprevyear' }),
    nextYearAriaLabel: formatMessage({ id: 'calendar.gotonextyear' }),
    closeButtonAriaLabel: formatMessage({ id: 'calendar.closedatepicker' }),
    monthPickerHeaderAriaLabel: formatMessage({ id: 'calendar.selecttochangemonth' }),
    yearPickerHeaderAriaLabel: formatMessage({ id: 'calendar.selecttochangeyear' }),
  };

  const datePickerClass = mergeStyleSets({
    control: {
      margin: '0px 0 15px 0',
      width: '300px',
    },
  });

  // STATUS DROPDOWN

  const statusDropdownStyles: Partial<IDropdownStyles> = { dropdown: { width: 300 } };

  const statusDropdownOptions = [
    { key: Status.NotStarted, text: formatMessage({ id: 'status.notstarted' }) },
    { key: Status.InProgress, text: formatMessage({ id: 'status.inprogress' }) },
    { key: Status.Assigned, text: formatMessage({ id: 'status.assigned' }) },
    { key: Status.Complete, text: formatMessage({ id: 'status.complete' }) },
  ];

  const handleSaveButtonClick = () => {
    if (!id) {
      // create new task
      const task: TaskDto = {
        id: 0, // update type to have optional id instead of 0?  id is not consumed by service POST call
        name: taskName,
        notes: taskNotes,
        status: selectedItemStatusDropdown,
        dueDate: taskDueDate.date ? taskDueDate.date.toDateString() : null,
      };

      const createTaskModel: PostPutTaskModel = {
        ProductKey: products[0].key || 'BAD_PRODUCT_KEY',
        EnvironmentKey: environmentKey,
        AccessToken: accessToken,
        Task: task,
      };
      createTask(createTaskModel);
    } else {
      // updating existing task
      const task: TaskDto = {
        id: id,
        name: taskName,
        notes: taskNotes,
        status: selectedItemStatusDropdown,
        dueDate: taskDueDate.date ? taskDueDate.date.toDateString() : null,
      };

      const updateTaskModel: PostPutTaskModel = {
        ProductKey: products[0].key || 'BAD_PRODUCT_KEY',
        EnvironmentKey: environmentKey,
        AccessToken: accessToken,
        Task: task,
      };
      updateTask(updateTaskModel);
    }
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

  // DELETE ICON
  const pointerCursorStyle: React.CSSProperties = {
    cursor: 'pointer',
  };

  const handleDeleteIconClick = () => {
    if (id) {
      const deleteTaskModel: DeleteTaskModel = {
        ProductKey: products[0].key || 'BAD_PRODUCT_KEY',
        EnvironmentKey: environmentKey,
        AccessToken: accessToken,
        TaskId: id,
      };
      deleteTask(deleteTaskModel);
    }
    hideModal();
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

  // EVENT HANDLERS

  const handleTaskNameChange = (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string | undefined): void => {
    // TODO 1693 - possible taskName.errorMessage display pattern, remove if unneeded
    if (!taskNameDirty) {
      setTaskNameDirty(true);
      setFormDirty(true);
    }
    setTaskName(newValue || '');
  };

  const handleTaskNotesChange = (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string | undefined): void => {
    setTaskNotes(newValue || '');
    setFormDirty(true);
  };

  const handleTaskDueDateChange = (date: Date | null | undefined) => {
    if (date) {
      // eslint-disable-next-line object-shorthand
      setTaskDueDate({ date: date });
      setFormDirty(true);
    }
  };

  const handleStatusDropdownChange = (event: React.FormEvent<HTMLDivElement>, item: IDropdownOption | undefined): void => {
    setSelectedItemStatusDropdown(item?.key as Status);
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

  const handleHistoryExpandClick = () => {
    if (!taskHistory) {
      ProductApi.getTaskHistory(environmentKey, accessToken, products[0].key as string, id as number).then(historyItems =>
        setTaskHistory(historyItems)
      );
    }
  };

  return (
    <>
      <div className={contentStyles.header}>
        <span id="TaskDetailsModal">
          <Text variant="xxLarge">{formatMessage({ id: selectedTask ? 'toDo.edittask' : 'toDo.newtask' })}</Text>
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
                  onChange={handleTaskNameChange}
                  required
                  maxLength={150}
                  label={formatMessage({ id: 'name' })}
                  value={taskName}
                  // TODO 1693 - possible taskName.errorMessage display pattern, remove if unneeded
                  onGetErrorMessage={value => (taskNameDirty && !value ? formatMessage({ id: 'fieldisrequired' }) : undefined)}
                />
              </Stack.Item>
              <Stack.Item>
                <DatePicker
                  value={taskDueDate.date}
                  className={datePickerClass.control}
                  firstDayOfWeek={firstDayOfWeek}
                  placeholder={formatMessage({ id: 'calendar.selectadate' })}
                  ariaLabel={formatMessage({ id: 'calendar.selectadate' })}
                  // DatePicker uses English strings by default. For localized apps, you must override this prop.
                  strings={datePickerStrings}
                  label={formatMessage({ id: 'toDo.duedate' })}
                  onSelectDate={handleTaskDueDateChange}
                  formatDate={(date: Date | undefined): string => `${date ? date.getMonth() + 1 : ''}/${date?.getDate()}/${date?.getFullYear()}`}
                />
              </Stack.Item>
            </Stack>
          </Stack.Item>
          <Stack.Item>
            <Stack horizontal styles={stackStyles} tokens={nestedStackTokens}>
              <Stack.Item styles={wideLeftStackItemStyles}>
                <TextField
                  placeholder={formatMessage({ id: 'toDo.tasknotesplaceholder' })}
                  onChange={handleTaskNotesChange}
                  multiline
                  maxLength={5000}
                  rows={13}
                  resizable={false}
                  label={formatMessage({ id: 'toDo.notes' })}
                  value={taskNotes}
                />
              </Stack.Item>
              <Stack.Item>
                <Dropdown
                  label={formatMessage({ id: 'status' })}
                  selectedKey={selectedItemStatusDropdown}
                  // eslint-disable-next-line react/jsx-no-bind
                  onChange={handleStatusDropdownChange}
                  placeholder={formatMessage({ id: 'toDo.selectastatus' })}
                  options={statusDropdownOptions}
                  styles={statusDropdownStyles}
                />
              </Stack.Item>
            </Stack>
          </Stack.Item>
          {id && (
            <Stack.Item>
              <ExpandablePanel title={formatMessage({ id: 'history.title' })} onExpand={handleHistoryExpandClick}>
                <HistoryList events={taskHistory} />
              </ExpandablePanel>
            </Stack.Item>
          )}
          <Stack.Item>
            <Stack horizontal styles={stackStyles} tokens={nestedStackTokens}>
              <Stack.Item grow styles={controlButtonStackItemStyles}>
                <PrimaryButton
                  style={saveButtonStyle}
                  text={formatMessage({ id: 'save' })}
                  onClick={handleSaveButtonClick}
                  disabled={saveButtonDisabled || !formDirty}
                />
                <DefaultButton text={formatMessage({ id: 'cancel' })} onClick={handleCancelButtonClick} />
              </Stack.Item>
              {id && (
                <Stack.Item styles={controlButtonStackItemStyles}>
                  <Text variant="xLarge">
                    <FontIcon style={pointerCursorStyle} onClick={handleDeleteIconClick} iconName="Delete" />
                  </Text>
                </Stack.Item>
              )}
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

export default TaskDetails;
