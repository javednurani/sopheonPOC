import { Meta, Story } from '@storybook/react';
import React from 'react';

import Button, { ButtonProps } from '../components/Button';

export default {
  title: 'Components/Button',
  component: Button,
  argTypes: { onClick: { action: 'clicked' } },
} as Meta;

const Template: Story<ButtonProps> = args => <Button {...args} />;

export const Primary = Template.bind({});
Primary.args = { primary: true, label: 'I am Primary' };

export const PrimaryWithIcon = Template.bind({});
PrimaryWithIcon.args = { primary: true, label: 'I am Primary', icon: 'Add' };

export const Disabled = Template.bind({});
Disabled.args = { primary: true, label: 'I am Disabled', disabled: true };

export const Secondary = Template.bind({});
Secondary.args = { primary: false, label: 'I am Secondary' };

export const SecondaryWithIcon = Template.bind({});
SecondaryWithIcon.args = { primary: false, label: 'I am Secondary', icon: 'Cancel' };

export const IconOnly = Template.bind({});
IconOnly.args = { variant: 'icon', icon: 'Emoji' };

export const Link = Template.bind({});
Link.args = { variant: 'link', label: 'I am a link' };

export const LinkWithIcon = Template.bind({});
LinkWithIcon.args = { variant: 'link', label: 'I am a link', icon: 'Calendar' };
