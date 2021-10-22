import React from 'react';

export type ListBoxProps = {
  variant?: string;
  label: string;
};

const ListBox: React.FC<ListBoxProps> = ({ variant, label }): JSX.Element => {
  switch (variant) {
    case 'link':
      return <button style={{ border: 'none', textDecoration: 'underline', background: 'transparent' }}>{label}</button>;
    default:
      return <button type="button">{label}</button>;
  }
};

export default ListBox;
