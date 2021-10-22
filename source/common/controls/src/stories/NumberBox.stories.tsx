import { Meta, Story } from '@storybook/react';
import React from 'react';

import NumberBox, { NumberBoxProps } from '../components/NumberBox';

export default {
  title: 'Components/NumberBox',
  component: NumberBox,
} as Meta;

const Template: Story<NumberBoxProps> = args => <NumberBox {...args} />;

export const Primary = Template.bind({});
Primary.args = { value: 123.45 };

export const Disabled = Template.bind({});
Disabled.args = { value: 666, disabled: true };
