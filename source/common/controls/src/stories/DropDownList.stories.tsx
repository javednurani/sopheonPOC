import { IDropdownOption } from '@fluentui/react';
import { Meta, Story } from '@storybook/react';
import React from 'react';

import DropDownList, { DropDownListProps } from '../components/DropDownList';

export default {
  title: 'Components/DropDownList',
  component: DropDownList,
} as Meta;

const Template: Story<DropDownListProps> = args => <DropDownList {...args} />;

const listOptions: IDropdownOption[] = [
  { key: 'A', text: 'Option A' },
  { key: 'B', text: 'Option B', selected: true },
  { key: 'C', text: 'Option C', disabled: true },
  { key: 'D', text: 'Option D' },
];

export const SingleSelect = Template.bind({});
SingleSelect.args = { options: listOptions };

export const MultiSelect = Template.bind({});
MultiSelect.args = { options: listOptions, multiple: true };

export const Disabled = Template.bind({});
Disabled.args = { options: listOptions, disabled: true };
