import { SpinButton } from '@fluentui/react';
import React from 'react';

export type NumberBoxProps = {
  disabled?: boolean;
  value?: number;
  onChange?: () => void;
};

const NumberBox: React.FC<NumberBoxProps> = ({ disabled, value, onChange }): JSX.Element => (
  <SpinButton disabled={disabled} value={value?.toString()} onChange={onChange} />
);

export default NumberBox;
