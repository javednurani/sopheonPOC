import { Meta, Story } from '@storybook/react';
import React from 'react';

import CheckBox, { CheckBoxProps } from '../components/CheckBox';

export default {
  title: 'Components/CheckBox',
  component: CheckBox,
  argTypes: { onChange: { action: 'change' } },
} as Meta;

const Template: Story<CheckBoxProps> = args => <CheckBox {...args} />;

export const Primary = Template.bind({});
Primary.args = { label: 'Random checkbox' };

export const NoLabel = Template.bind({});
NoLabel.args = {};

export const Disabled = Template.bind({});
Disabled.args = { disabled: true, label: 'I am disabled' };

export const CheckedAndDisabled = Template.bind({});
CheckedAndDisabled.args = { checked: true, disabled: true, label: 'I am disabled' };

export const LabelOnLeft = Template.bind({});
LabelOnLeft.args = { labelSide: 'left', label: 'Checkbox with label on left' };
