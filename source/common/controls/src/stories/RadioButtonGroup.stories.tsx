import { Meta, Story } from '@storybook/react';
import React from 'react';

import RadioButtonGroup, { RadioButtonGroupProps } from '../components/RadioButtonGroup';

export default {
  title: 'Components/RadioButtonGroup',
  component: RadioButtonGroup,
} as Meta;

const Template: Story<RadioButtonGroupProps> = args => <RadioButtonGroup {...args} />;

export const Primary = Template.bind({});
Primary.args = { label: 'I am Primary' };

export const Link = Template.bind({});
Link.args = { variant: 'link', label: 'I am a link' };
