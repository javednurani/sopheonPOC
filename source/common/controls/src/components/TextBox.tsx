import { TextField } from '@fluentui/react';
import React from 'react';

export type TextBoxProps = {
  multiline?: boolean;
  value?: string;
  disabled?: boolean;
};

const TextBox: React.FC<TextBoxProps> = ({ multiline, value, disabled }): JSX.Element => (
  <TextField value={value} multiline={multiline} disabled={disabled} />
);

export default TextBox;
