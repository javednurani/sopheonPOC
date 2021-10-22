import { ActionButton, DefaultButton, IconButton, IIconProps, initializeIcons, PrimaryButton } from '@fluentui/react';
import React from 'react';

initializeIcons();

export type ButtonProps = {
  variant?: string;
  label?: string;
  disabled?: boolean;
  primary?: boolean;
  icon?: string;
  onClick?: () => void;
};

const Button: React.FC<ButtonProps> = ({ variant, label, primary, icon, disabled, onClick }): JSX.Element => {
  const iconProps: IIconProps | undefined = icon ? { iconName: icon } : undefined;
  switch (variant) {
    case 'link':
      return (
        <ActionButton iconProps={iconProps} disabled={disabled} onClick={onClick}>
          {label}
        </ActionButton>
      );
    case 'icon':
      return <IconButton iconProps={iconProps} disabled={disabled} onClick={onClick} />;
    default:
      return primary ? (
        <PrimaryButton iconProps={iconProps} disabled={disabled} onClick={onClick}>
          {label}
        </PrimaryButton>
      ) : (
        <DefaultButton disabled={disabled} iconProps={iconProps} onClick={onClick}>
          {label}
        </DefaultButton>
      );
  }
};

export default Button;
