import { Meta, Story } from '@storybook/react';
import React from 'react';

import TextBox, { TextBoxProps } from '../components/TextBox';

export default {
  title: 'Components/TextBox',
  component: TextBox,
  argTypes: {
    value: {
      control: { type: 'text' },
    },
    multiline: {
      control: { type: 'boolean' },
      defaultValue: false,
    },
    disabled: {
      control: { type: 'boolean' },
      defaultValue: false,
    },
    onChange: { action: 'changed' },
  },
} as Meta;

const Template: Story<TextBoxProps> = args => <TextBox {...args} />;

export const Primary = Template.bind({});
Primary.args = { value: 'This is text' };

export const TextArea = Template.bind({});
TextArea.args = { multiline: true, value: 'this is a description' };

export const Disabled = Template.bind({});
Disabled.args = { disabled: true, value: 'this is a description' };
