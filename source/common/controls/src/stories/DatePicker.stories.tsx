import { Meta, Story } from '@storybook/react';
import React from 'react';

import DatePicker, { DatePickerProps } from '../components/DatePicker';

export default {
  title: 'Components/DatePicker',
  component: DatePicker,
} as Meta;

const Template: Story<DatePickerProps> = args => <DatePicker {...args} />;

export const Primary = Template.bind({});
Primary.args = { disabled: false, value: new Date(2021, 5, 2) };

export const Disabled = Template.bind({});
Disabled.args = { disabled: true };
