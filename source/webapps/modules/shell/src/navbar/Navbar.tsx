//import { Theme, useTheme } from '@fluentui/react-theme-provider';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { CSSProperties } from 'react';
import { useIntl } from 'react-intl';
import { Link } from 'react-router-dom';

import { appModules } from '../settings/appModuleSettings';

const Navbar: React.FunctionComponent = () => {
  const { formatMessage } = useIntl();
  const theme = useTheme();

  const linkStyles: CSSProperties = {
    fontSize: '20px',
    textDecoration: 'none',
    color: theme.palette.themeDark,
    marginLeft: '20px',
  };

  return (
    <nav role="navigation">
      {appModules.map(appModule => (
        <Link to={appModule.routeName} key={`key${appModule.routeName}`} style={linkStyles}>
          {formatMessage({ id: appModule.displayNameResourceKey })}
        </Link>
      ))}
    </nav>
  );
};

export default Navbar;
