import { IStackTokens, Stack, Sticky, StickyPositionType } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { FunctionComponent } from 'react';
import { useIntl } from 'react-intl';
import { useLocation } from 'react-router-dom';

import { AppModule, appModules } from '../appModuleSettings';
import SignupLoginButton from '../authentication/SignupLoginButton';
import { ReactComponent as LucyLogo } from '../images/Lucy_logo.svg';
import Navbar from '../navbar/Navbar';
import ThemeSelector from '../themes/components/themeSelector/ThemeSelector';
import { ChangeThemeAction } from '../themes/themeReducer/themeReducer';

interface HeaderProps {
  changeTheme: (useDarkTheme: boolean) => ChangeThemeAction;
}

const Header: FunctionComponent<HeaderProps> = ({ changeTheme }: HeaderProps) => {
  const { formatMessage } = useIntl();
  const location = useLocation();
  const theme = useTheme();

  const headerStyle: React.CSSProperties = {
    marginTop: '8px',
    borderBottom: '1px solid',
    borderBottomColor: theme.palette.neutralSecondary,
  };

  const logoStyle: React.CSSProperties = {
    height: '100%',
    width: '100%',
    maxWidth: '100px',
    maxHeight: '100px',
    minWidth: '25px',
    minHeight: '25px',
    fill: theme.palette.themePrimary,
    stroke: theme.palette.themePrimary,
    overflow: 'visible',
  };

  const logoContainerStyle: React.CSSProperties = {
    margin: '5px',
  };

  const stackTokensWithGap: IStackTokens = {
    childrenGap: 2,
  };

  const getTitle = (path: string): string => {
    const currentApp: AppModule | undefined = appModules.find(appModule => appModule.routeName === path);
    return currentApp ? formatMessage({ id: currentApp.displayNameResourceKey }) : formatMessage({ id: 'defaultTitle' });
  };

  const pageTitle: string = getTitle(location.pathname);

  return (
    <Sticky stickyPosition={StickyPositionType.Header} isScrollSynced>
      <header style={headerStyle} role="banner">
        <Stack horizontal verticalAlign="center">
          <Stack.Item shrink>
            <div style={logoContainerStyle} title={formatMessage({ id: 'lucy' })}>
              <LucyLogo style={logoStyle} />
            </div>
          </Stack.Item>
          <Stack.Item>
            <Navbar />
          </Stack.Item>
          <Stack.Item grow align="center">
            <h1 id="page-title" className="page-title">
              {pageTitle}
            </h1>
          </Stack.Item>
          <Stack.Item>
            <Stack tokens={stackTokensWithGap}>
              <Stack.Item>
                <SignupLoginButton />
              </Stack.Item>
              <Stack.Item>
                <ThemeSelector changeTheme={changeTheme} />
              </Stack.Item>
            </Stack>
          </Stack.Item>
        </Stack>
      </header>
    </Sticky>
  );
};

export default Header;
