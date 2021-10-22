import { SpinButton } from '@fluentui/react';
import React from 'react';

export type NumberBoxProps = {
  disabled?: boolean;
  value?: number;
};

const NumberBox: React.FC<NumberBoxProps> = ({ disabled, value }): JSX.Element => <SpinButton disabled={disabled} value={value?.toString()} />;

export default NumberBox;
