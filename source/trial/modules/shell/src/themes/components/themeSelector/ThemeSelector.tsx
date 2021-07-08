import { Stack, Toggle } from '@fluentui/react';
import React, { FunctionComponent } from 'react';
import { FormattedMessage, useIntl } from 'react-intl';

import { ChangeThemeAction } from '../../themeReducer/themeReducer';

export interface ThemeSelectorProps {
  changeTheme: (useDarkTheme: boolean) => ChangeThemeAction;
}

const ThemeSelector: FunctionComponent<ThemeSelectorProps> = ({ changeTheme }: ThemeSelectorProps) => {
  const { formatMessage } = useIntl();

  const toggleStyle = {
    root: {
      marginLeft: '4px',
      marginRight: '4px',
    },
  };

  const onChange = (event: React.MouseEvent<HTMLElement, MouseEvent>, checked?: boolean | undefined): void => {
    if (checked !== undefined) {
      changeTheme(checked);
    }
  };

  return (
    <Stack horizontal>
      <FormattedMessage id="header.useDarkTheme" />
      <Toggle styles={toggleStyle} onChange={onChange} ariaLabel={formatMessage({ id: 'header.useDarkTheme' })} />
    </Stack>
  );
};

export default ThemeSelector;
