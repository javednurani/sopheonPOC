import { Meta, Story } from '@storybook/react';
import React from 'react';

import DatePicker, { DatePickerProps } from '../components/DatePicker';

export default {
  title: 'Components/DatePicker',
  component: DatePicker,
  argTypes: {
    disabled: { control: { type: 'boolean' }, defaultValue: false },
    value: { control: { type: 'date' } },
    onChange: { action: 'changed' },
  },
} as Meta;

const Template: Story<DatePickerProps> = args => <DatePicker {...args} />;

export const Primary = Template.bind({});
Primary.args = { disabled: false, value: new Date('2021-05-02') };

export const Disabled = Template.bind({});
Disabled.args = { disabled: true, value: new Date('2021-06-30') };
