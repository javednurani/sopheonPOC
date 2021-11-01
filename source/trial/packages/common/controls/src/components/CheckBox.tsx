import { Checkbox } from '@fluentui/react';
import React from 'react';

export type CheckBoxProps = {
  labelSide?: 'left' | 'right';
  checked?: boolean;
  label: string;
  triState?: boolean;
  disabled?: boolean;
  onChange?: () => void;
};

const CheckBox: React.FC<CheckBoxProps> = ({ checked, label, labelSide, disabled, onChange }): JSX.Element => {
  const boxSide = labelSide === 'left' ? 'end' : 'start';
  return <Checkbox label={label} checked={checked} disabled={disabled} onChange={onChange} boxSide={boxSide} />;
};

export default CheckBox;
