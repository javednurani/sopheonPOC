import { Meta, Story } from '@storybook/react';
import React from 'react';

import ListBox, { ListBoxProps } from '../components/ListBox';

export default {
  title: 'Components/ListBox',
  component: ListBox,
} as Meta;

const Template: Story<ListBoxProps> = args => <ListBox {...args} />;

export const Primary = Template.bind({});
Primary.args = { label: 'I am Primary' };

export const Link = Template.bind({});
Link.args = { variant: 'link', label: 'I am a link' };
