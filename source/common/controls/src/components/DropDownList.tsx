import { Dropdown, IDropdownOption } from '@fluentui/react';
import React from 'react';

export type DropDownListProps = {
  options: IDropdownOption[];
  disabled?: boolean;
  multiple?: boolean;
};

const DropDownList: React.FC<DropDownListProps> = ({ multiple, options, disabled }): JSX.Element => (
  <Dropdown options={options} disabled={disabled} multiSelect={multiple}></Dropdown>
);

export default DropDownList;
