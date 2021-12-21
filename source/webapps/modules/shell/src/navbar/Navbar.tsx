//import { Theme, useTheme } from '@fluentui/react-theme-provider';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { CSSProperties } from 'react';
import { useIntl } from 'react-intl';
import { Link, useLocation } from 'react-router-dom';

import { appModules } from '../settings/appModuleSettings';

const Navbar: React.FunctionComponent = () => {
  const { formatMessage } = useIntl();
  const theme = useTheme();
  const location = useLocation();

  const linkStyles: CSSProperties = {
    fontSize: theme.fonts.medium.fontSize,
    lineHeight: '40px',
    display: 'inline-block',
    textDecoration: 'none',
    color: theme.palette.themeDark,
    marginLeft: '20px',
    borderBottomStyle: 'none',
  };

  const activeLinkStyles: CSSProperties = {
    ...linkStyles,
    borderBottomWidth: 3,
    borderBottomStyle: 'solid',
    borderBottomColor: theme.palette.themePrimary,
    fontWeight: 'bold',
  };

  return (
    <nav role="navigation">
      {appModules.map(appModule => {
        const isCurrentPath: boolean = location.pathname.toLowerCase().startsWith(appModule.routeName.toLowerCase());
        return (
          <Link to={appModule.routeName} key={`key${appModule.routeName}`} style={isCurrentPath ? activeLinkStyles : linkStyles}>
            {formatMessage({ id: appModule.displayNameResourceKey })}
          </Link>
        );
      })}
    </nav>
  );
};

export default Navbar;
