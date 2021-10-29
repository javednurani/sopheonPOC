import { ChoiceGroup, IChoiceGroupOption } from '@fluentui/react';
import React from 'react';

export type RadioButtonGroupProps = {
  options: IChoiceGroupOption[];
  disabled?: boolean;
  selectedKey?: string;
  onChange?: () => void;
};

const RadioButtonGroup: React.FC<RadioButtonGroupProps> = ({ options, selectedKey, disabled, onChange }): JSX.Element => (
  <ChoiceGroup defaultSelectedKey={selectedKey} options={options} disabled={disabled} onChange={onChange} />
);

export default RadioButtonGroup;
