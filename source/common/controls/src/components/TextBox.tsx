import { TextField } from '@fluentui/react';
import React from 'react';

export type TextBoxProps = {
  multiline?: boolean;
  value?: string;
  disabled?: boolean;
  onChange?: () => void;
};

const TextBox: React.FC<TextBoxProps> = ({ multiline, value, disabled, onChange }): JSX.Element => (
  <TextField value={value} multiline={multiline} disabled={disabled} onChange={onChange} />
);

export default TextBox;
