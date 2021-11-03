import { IChoiceGroupOption } from '@fluentui/react';
import { Meta, Story } from '@storybook/react';
import React from 'react';

import RadioButtonGroup, { RadioButtonGroupProps } from '../components/RadioButtonGroup';

export default {
  title: 'Components/RadioButtonGroup',
  component: RadioButtonGroup,
  argTypes: {
    options: { control: { type: 'object' } },
    selectedKey: { control: { type: 'text' }, defaultValue: '' },
    disabled: {
      control: { type: 'boolean' },
      defaultValue: false,
    },
    //onChange: { action: 'changed' },
  },
} as Meta;

const Template: Story<RadioButtonGroupProps> = args => <RadioButtonGroup {...args} />;

const listOptions: IChoiceGroupOption[] = [
  { key: 'A', text: 'Option A' },
  { key: 'B', text: 'Option B' },
  { key: 'C', text: 'Option C', disabled: true },
  { key: 'D', text: 'Option D' },
];

export const Primary = Template.bind({});
Primary.args = { options: listOptions, selectedKey: 'B' };

export const Disabled = Template.bind({});
Disabled.args = { options: listOptions, selectedKey: 'A', disabled: true };
