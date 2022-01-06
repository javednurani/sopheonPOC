import { AuthenticatedTemplate } from '@azure/msal-react';
import { IconButton, IIconProps, IIconStyles, ITooltipHostStyles, TooltipHost } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import { FontSizes } from '@fluentui/theme';
import React from 'react';
import { useIntl } from 'react-intl';

const NotificationsButton: React.FunctionComponent = () => {
  const { formatMessage } = useIntl();
  const theme = useTheme();

  const notificationIconStyles: Partial<IIconStyles> = {
    root: {
      fontSize: FontSizes.size18,
      transform: 'rotate(20deg)',
      color: theme.palette.neutralSecondary,
    },
  };

  const notificationIcon: IIconProps = { iconName: 'Ringer', styles: notificationIconStyles };
  const hostStyles: Partial<ITooltipHostStyles> = { root: { display: 'inline-block' } };

  return (
    <React.Fragment>
      <AuthenticatedTemplate>
        <TooltipHost content={formatMessage({ id: 'auth.notifications' })} id="NotificationsTooltip" styles={hostStyles}>
          <IconButton iconProps={notificationIcon} />
        </TooltipHost>
      </AuthenticatedTemplate>
    </React.Fragment>
  );
};

export default NotificationsButton;
