import {
  DatePicker,
  DayOfWeek,
  DefaultButton,
  Dropdown,
  FontWeights,
  IButtonStyles,
  IconButton,
  IDatePickerStrings,
  IDropdownOption,
  IDropdownStyles,
  IIconProps,
  mergeStyleSets,
  PrimaryButton,
  Stack,
  TextField,
} from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { useEffect, useState } from 'react';
import { useIntl } from 'react-intl';

import { UpdateProductAction } from './product/productReducer';
import { Attributes, PatchOperation, Product, ProductItemTypes, Status, UpdateProductModel } from './types';

export interface IAddTaskProps {
  hideModal: () => void;
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  environmentKey: string;
  accessToken: string;
  products: Product[];
}

const AddTask: React.FunctionComponent<IAddTaskProps> = ({ hideModal, updateProduct, environmentKey, accessToken, products }: IAddTaskProps) => {
  const theme = useTheme();
  const { formatMessage } = useIntl();

  const [taskName, setTaskName] = useState('');
  const [taskNotes, setTaskNotes] = useState('');
  const [taskDueDate, setTaskDueDate] = useState({ date: new Date() });

  const [selectedItemStatusDropdown, setSelectedItemStatusDropdown] = React.useState<IDropdownOption>();

  const [saveButtonDisabled, setSaveButtonDisabled] = React.useState(false);

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
      marginTop: '4px',
      marginRight: '2px',
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
      margin: '10px 0 15px 0',
      maxWidth: '300px',
    },
  });

  // STATUS DROPDOWN

  const statusDropdownStyles: Partial<IDropdownStyles> = { dropdown: { width: 300 } };

  const statusDropdownOptions = [
    // TODO, keys = int values per domain enum?
    { key: Status.NotStarted, text: 'Not Started' },
    { key: Status.InProgress, text: 'In Progress' },
    { key: Status.Assigned, text: 'Assigned' },
    { key: Status.Complete, text: 'Complete' },
  ];

  const handleSaveButtonClick = () => {
    // make API call
    const productPatchData: PatchOperation[] = [
      {
        op: 'add',
        path: '/Items',
        value: [
          {
            name: taskName,
            productItemTypeId: ProductItemTypes.TASK,
            stringAttributeValues: [
              {
                attributeId: Attributes.NOTES,
                value: taskNotes,
              },
            ],
            utcDateTimeAttributeValues: [
              {
                attributeId: Attributes.DUEDATE,
                value: taskDueDate.date.toDateString(),
              },
            ],
            enumCollectionAttributeValues: [
              {
                attributeId: Attributes.STATUS,
                value: [
                  {
                    enumAttributeOptionId: selectedItemStatusDropdown?.key || Status.NotStarted,
                  },
                ],
              },
            ],
          },
        ],
      },
    ];

    const updateProductDto: UpdateProductModel = {
      ProductPatchData: productPatchData,
      ProductKey: products[0].key || 'BAD_PRODUCT_KEY',
      EnvironmentKey: environmentKey,
      AccessToken: accessToken,
    };
    updateProduct(updateProductDto);

    // reset form?
    hideModal();
  };

  // CANCEL BUTTON
  const handleCancelButtonClick = () => {
    // reset form?
    hideModal();
  };

  // display component for dialog testing
  // return (
  //   <Stack>
  //     <Stack.Item>
  //       <p>Hello from a display only component :)</p>
  //     </Stack.Item>
  //   </Stack>
  // );

  const handleTaskNameChange = (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string | undefined): void => {
    setTaskName(newValue || '');
  };

  const handleTaskNotesChange = (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string | undefined): void => {
    setTaskNotes(newValue || '');
  };

  const handleTaskDueDateChange = (date: Date | null | undefined) => {
    if (date) {
      // eslint-disable-next-line object-shorthand
      setTaskDueDate({ date: date });
    }
  };

  const handleStatusDropdownChange = (event: React.FormEvent<HTMLDivElement>, item: IDropdownOption | undefined): void => {
    setSelectedItemStatusDropdown(item);
  };

  return (
    <>
      <div className={contentStyles.header}>
        <span id="AddTaskModal">New Task</span>
        <IconButton styles={iconButtonStyles} iconProps={cancelIcon} ariaLabel="Close popup modal" onClick={hideModal} />
      </div>
      <div className={contentStyles.body}>
        <Stack>
          <Stack.Item>
            <TextField onChange={handleTaskNameChange} required label="Name" />
          </Stack.Item>
          <Stack.Item>
            <TextField onChange={handleTaskNotesChange} multiline resizable={false} label="Notes" />
          </Stack.Item>
          <Stack.Item>
            <DatePicker
              className={datePickerClass.control}
              firstDayOfWeek={firstDayOfWeek}
              placeholder="Select a date..."
              ariaLabel="Select a date"
              // DatePicker uses English strings by default. For localized apps, you must override this prop.
              strings={datePickerStrings}
              label="Due Date"
              onSelectDate={handleTaskDueDateChange}
            />
          </Stack.Item>
          <Stack.Item>
            <Dropdown
              label="Status"
              selectedKey={selectedItemStatusDropdown ? selectedItemStatusDropdown.key : Status.NotStarted}
              // eslint-disable-next-line react/jsx-no-bind
              onChange={handleStatusDropdownChange}
              placeholder="Select a status"
              options={statusDropdownOptions}
              styles={statusDropdownStyles}
            />
          </Stack.Item>
          <Stack.Item>
            <PrimaryButton text="Save" onClick={handleSaveButtonClick} disabled={saveButtonDisabled} />
            <DefaultButton text="Cancel" onClick={handleCancelButtonClick} />
          </Stack.Item>
        </Stack>
      </div>
    </>
  );
};

export default AddTask;
