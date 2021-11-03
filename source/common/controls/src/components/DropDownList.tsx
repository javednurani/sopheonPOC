import { Dropdown, IDropdownOption } from '@fluentui/react';
import React from 'react';

export type DropDownListProps = {
  options: IDropdownOption[];
  disabled?: boolean;
  multiple?: boolean;
  onChange?: () => void;
};

const DropDownList: React.FC<DropDownListProps> = ({ multiple, options, disabled, onChange }): JSX.Element => (
  <Dropdown options={options} disabled={disabled} multiSelect={multiple} onChange={onChange}></Dropdown>
);

export default DropDownList;
