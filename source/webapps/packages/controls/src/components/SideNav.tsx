import { AccountInfo } from '@azure/msal-browser';
import { useMsal } from '@azure/msal-react';
import { INavLinkGroup, INavStyleProps, INavStyles, IStyleFunctionOrObject, Link, Nav, Stack, Text } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { useEffect, useState } from 'react';
import { useIntl } from 'react-intl';

import { getMsalAccount } from '../authHelpers';

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

  const { instance, accounts } = useMsal();

  const [account, setAccount] = useState<AccountInfo>();

  useEffect(() => {
    const msalAccount: AccountInfo | undefined = getMsalAccount(instance);
    if (msalAccount !== undefined) {
      setAccount(msalAccount);
    }
  }, [instance, accounts]);

  const sideBarStyles: IStyleFunctionOrObject<INavStyleProps, INavStyles> = {
    root: {
      width: '150px',
      backgroundColor: theme.palette.neutralLight,
    },
    navItem: {
      selectors: {
        '.is-selected': { backgroundColor: theme.semanticColors.bodyBackground, color: theme.semanticColors.bodyTextChecked },
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
      color: theme.palette.neutralPrimary,
      marginBottom: '10px',
      marginRight: '12px',
      borderBottomStyle: 'none',
    },
  };

  const mailToUrl = `mailto:support@sopheon.com?subject=${account?.name} has a question or comment`;

  return (
    <Stack verticalAlign="space-between" styles={fullHeight}>
      <Stack.Item>
        <Nav selectedKey={selectedMenuKey} ariaLabel="SideBar" styles={sideBarStyles} groups={menuItems} />
      </Stack.Item>
      <Stack.Item styles={linkContainerStyles}>
        <Stack>
          <Stack.Item>
            <Link styles={linkStyles} href="">
              <Text variant="medium">{formatMessage({ id: 'sidebar.resourcesHelp' })}</Text>
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
