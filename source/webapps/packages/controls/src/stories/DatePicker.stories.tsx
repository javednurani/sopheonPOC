import { Meta, Story } from '@storybook/react';
import React from 'react';
import { IntlProvider } from 'react-intl';

import DatePicker, { DatePickerProps } from '../components/DatePicker';
import { messages } from './assets/messages_duplicated';

export default {
  title: 'Components/DatePicker',
  component: DatePicker,
  argTypes: {
    disabled: { control: { type: 'boolean' }, defaultValue: false },
    value: { control: { type: 'date' } },
    onChange: { action: 'changed' },
    required: { control: { type: 'boolean' }, defaultValue: false },
  },
} as Meta;

const locale = 'en';

const Template: Story<DatePickerProps> = (args: DatePickerProps) => (
  <IntlProvider locale={locale} messages={messages[locale]}>
    <DatePicker {...args} />
  </IntlProvider>
);

export const Primary = Template.bind({});
Primary.args = { disabled: false, value: new Date('2021-05-02') };

export const Disabled = Template.bind({});
Disabled.args = { disabled: true, value: new Date('2021-06-30') };

export const label = Template.bind({});
const labelExampleProps: DatePickerProps = { label: 'Custom Label', value: new Date('2022-1-11') };
label.args = labelExampleProps;

export const required = Template.bind({});
const requiredExampleProps: DatePickerProps = { required: true, value: new Date('2022-1-11') };
required.args = requiredExampleProps;

export const width = Template.bind({});
const widthExampleProps: DatePickerProps = { width: 300, value: new Date('2022-1-11') };
width.args = widthExampleProps;
