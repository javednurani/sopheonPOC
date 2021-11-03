import { Meta, Story } from '@storybook/react';
import React from 'react';

import CheckBox, { CheckBoxProps } from '../components/CheckBox';

export default {
  title: 'Components/CheckBox',
  component: CheckBox,
  argTypes: {
    label: {
      control: { type: 'text' },
      defaultValue: '',
    },
    labelSide: {
      options: ['left', 'right'],
      control: { type: 'radio' },
    },
    disabled: {
      control: { type: 'boolean' },
      defaultValue: false,
    },
    checked: {
      control: { type: 'boolean' },
      defaultValue: false,
    },
    triState: {
      control: { type: 'boolean' },
      defaultValue: false,
    },
    onChange: { action: 'change' },
  },
} as Meta;

const Template: Story<CheckBoxProps> = args => <CheckBox {...args} />;

export const DefaultWithLabel = Template.bind({});
DefaultWithLabel.args = { label: 'Random checkbox' };

export const NoLabel = Template.bind({});
NoLabel.args = {};

export const Disabled = Template.bind({});
Disabled.args = { disabled: true, label: 'I am disabled' };

export const CheckedAndDisabled = Template.bind({});
CheckedAndDisabled.args = { checked: true, disabled: true, label: 'I am disabled' };

export const LabelOnLeft = Template.bind({});
LabelOnLeft.args = { labelSide: 'left', label: 'Checkbox with label on left' };
