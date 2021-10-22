import React from 'react';

export type RadioButtonGroupProps = {
  variant?: string;
  label: string;
};

const RadioButtonGroup: React.FC<RadioButtonGroupProps> = ({ variant, label }): JSX.Element => {
  switch (variant) {
    case 'link':
      return <button style={{ border: 'none', textDecoration: 'underline', background: 'transparent' }}>{label}</button>;
    default:
      return <button type="button">{label}</button>;
  }
};

export default RadioButtonGroup;
