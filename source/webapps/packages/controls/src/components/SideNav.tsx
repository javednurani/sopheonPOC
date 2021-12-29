import { INavLinkGroup, INavStyleProps, INavStyles, IStyleFunctionOrObject, Link, Nav, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';
import { useIntl } from 'react-intl';

export type SideBarProps = {
  menuItems: INavLinkGroup[];
  selectedMenuKey: string;
};

const fullHeight: IStyleFunctionOrObject<INavStyleProps, INavStyles> = {
  root: {
    height: '100%',
  },
};

const SideNav: React.FC<SideBarProps> = ({ menuItems, selectedMenuKey }): JSX.Element => {
  const { formatMessage } = useIntl();
  const theme = useTheme();

  const sideBarStyles: IStyleFunctionOrObject<INavStyleProps, INavStyles> = {
    root: {
      width: '150px',
      backgroundColor: '#eAeef0',
    },
    navItem: {
      selectors: {
        '.is-selected': { backgroundColor: theme.semanticColors.bodyBackground },
      },
    },

    link: {
      background: 'transparent',
    },
  };

  const linkContainerStyles: IStyleFunctionOrObject<INavStyleProps, INavStyles> = {
    root: {
      marginLeft: '25px',
    },
  };

  const linkStyles: IStyleFunctionOrObject<INavStyleProps, INavStyles> = {
    root: {
      fontSize: theme.fonts.medium.fontSize,
      display: 'inline-block',
      textDecoration: 'none !important',
      color: theme.palette.themeDark,
      marginBottom: '10px',
      borderBottomStyle: 'none',
    },
  };

  const mailToUrl = `mailto:support@sopheon.com?subject= App User has a question or comment`;

  return (
    <Stack verticalAlign="space-between" styles={fullHeight}>
      <Stack.Item>
        <Nav selectedKey={selectedMenuKey} ariaLabel="SideBar" styles={sideBarStyles} groups={menuItems} />
      </Stack.Item>
      <Stack.Item styles={linkContainerStyles}>
        <Stack>
          <Stack.Item>
            <Link styles={linkStyles} href="">
              {formatMessage({ id: 'sidebar.resourcesHelp' })}
            </Link>
          </Stack.Item>
          <Stack.Item>
            <Link styles={linkStyles} href={mailToUrl}>
              {formatMessage({ id: 'sidebar.contactus' })}
            </Link>
          </Stack.Item>
        </Stack>
      </Stack.Item>
    </Stack>
  );
};

export default SideNav;
