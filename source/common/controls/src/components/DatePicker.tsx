import { DatePicker as FluentDatePicker } from '@fluentui/react';
import React from 'react';

export type DatePickerProps = {
  value?: Date;
  disabled?: boolean;
};

const DatePicker: React.FC<DatePickerProps> = ({ value, disabled }): JSX.Element => <FluentDatePicker value={value} disabled={disabled} />;

export default DatePicker;
