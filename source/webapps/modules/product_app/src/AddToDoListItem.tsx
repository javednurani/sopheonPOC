import {
  DatePicker,
  DayOfWeek,
  Dropdown,
  FontWeights,
  IButtonStyles,
  IconButton,
  IDatePickerStrings,
  IDropdownOption,
  IDropdownStyles,
  IIconProps,
  mergeStyleSets,
  Stack,
  TextField,
} from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';
import { useIntl } from 'react-intl';

export interface IAddToDoListItemProps {
  hideModal: () => void;
}

const AddToDoListItem: React.FunctionComponent<IAddToDoListItemProps> = ({ hideModal }: IAddToDoListItemProps) => {
  const theme = useTheme();
  const { formatMessage } = useIntl();

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

  const dropdownStyles: Partial<IDropdownStyles> = { dropdown: { width: 300 } };

  const dropdownControlledExampleOptions = [
    { key: 'apple', text: 'Apple' },
    { key: 'banana', text: 'Banana' },
    { key: 'grape', text: 'Grape' },
    { key: 'broccoli', text: 'Broccoli' },
  ];

  const [selectedItem, setSelectedItem] = React.useState<IDropdownOption>();

  const onChange = (event: React.FormEvent<HTMLDivElement>, item: IDropdownOption | undefined): void => {
    setSelectedItem(item);
  };

  return (
    <>
      <div className={contentStyles.header}>
        <span id="AddToDoListItemModal">New Task</span>
        <IconButton styles={iconButtonStyles} iconProps={cancelIcon} ariaLabel="Close popup modal" onClick={hideModal} />
      </div>
      <div className={contentStyles.body}>
        <Stack>
          <Stack.Item>
            <TextField required label="Name" />
          </Stack.Item>
          <Stack.Item>
            <TextField multiline resizable={false} label="Notes" />
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
            />
          </Stack.Item>
          <Stack.Item>
            <Dropdown
              label="Status"
              selectedKey={selectedItem ? selectedItem.key : undefined}
              // eslint-disable-next-line react/jsx-no-bind
              onChange={onChange}
              placeholder="Select a status"
              options={dropdownControlledExampleOptions}
              styles={dropdownStyles}
            />
          </Stack.Item>
        </Stack>
      </div>
    </>
  );
};

export default AddToDoListItem;
