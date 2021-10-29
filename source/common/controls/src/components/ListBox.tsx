import { Dropdown, IDropdownOption } from '@fluentui/react';
import React from 'react';

export type ListBoxProps = {
  options: IDropdownOption[];
  disabled?: boolean;
  multiple?: boolean;
  onChange?: () => void;
};

const ListBox: React.FC<ListBoxProps> = ({ options, disabled, multiple, onChange }): JSX.Element => (
  <Dropdown options={options} disabled={disabled} multiSelect={multiple} onChange={onChange}></Dropdown>
);

export default ListBox;
