import { AuthenticatedTemplate } from '@azure/msal-react';
import { IconButton, IIconProps, IIconStyles, ITooltipHostStyles, TooltipHost } from '@fluentui/react';
import { FontSizes } from '@fluentui/theme';
import React from 'react';
import { useIntl } from 'react-intl';

const NotificationsButton: React.FunctionComponent = () => {
  const { formatMessage } = useIntl();
  //const theme: Theme = useTheme();

  const notificationIconStyles: Partial<IIconStyles> = {
    root: {
      fontSize: FontSizes.size18,
      transform: 'rotate(20deg)',
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
