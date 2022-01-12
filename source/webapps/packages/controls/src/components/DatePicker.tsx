import { DatePicker as FluentDatePicker, DayOfWeek, IDatePickerStrings, mergeStyles } from '@fluentui/react';
import React from 'react';
import { useIntl } from 'react-intl';

export type DatePickerProps = {
  disabled?: boolean;
  label?: string;
  onSelectDate?: (date: Date | null | undefined) => void;
  required?: boolean;
  value?: Date;
  width?: number;
};

const DatePicker: React.FC<DatePickerProps> = ({ disabled, label, onSelectDate, required, value, width: _width }): JSX.Element => {
  const { formatMessage } = useIntl();

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
    isRequiredErrorMessage: formatMessage({ id: 'fieldisrequired' }),
  };

  const defaultLabel: string = formatMessage({ id: 'date' });

  const formatDate = (date: Date | undefined): string => (date ? date.toLocaleDateString() : '');

  const className: string = mergeStyles({ width: _width });

  return (
    <FluentDatePicker
      ariaLabel={formatMessage({ id: 'calendar.selectDate' })}
      className={className}
      disabled={disabled}
      firstDayOfWeek={DayOfWeek.Sunday}
      formatDate={formatDate}
      label={label ?? defaultLabel}
      onSelectDate={onSelectDate}
      placeholder={formatMessage({ id: 'calendar.selectDate' })}
      isRequired={required ?? false}
      strings={datePickerStrings}
      value={value}
    />
  );
};

export default DatePicker;
