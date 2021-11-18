import { Icon, IIconStyles, Stack, Sticky, StickyPositionType } from '@fluentui/react';
import React from 'react';
import { FormattedMessage } from 'react-intl';

const Footer: React.FunctionComponent = () => {
  const footerStyle: React.CSSProperties = {
    borderTop: '1px solid',
    padding: '10px',
    textAlign: 'start',
  };

  const iconStyles: Partial<IIconStyles> = {
    root: {
      marginRight: '8px',
      marginLeft: '8px',
    },
  };

  return (
    <Sticky stickyPosition={StickyPositionType.Footer}>
      <footer style={footerStyle} role="contentinfo">
        <Stack horizontal verticalAlign="center">
          <Stack.Item grow align="start">
            <FormattedMessage id="footer.copyright" />
          </Stack.Item>
          <Stack.Item shrink align="end">
            <span>
              <Icon styles={iconStyles} iconName={'LikeSolid'} aria-hidden="true" />
              <Icon styles={iconStyles} iconName={'CommentSolid'} aria-hidden="true" />
              <Icon styles={iconStyles} iconName={'LinkedInLogo'} aria-hidden="true" />
              <Icon styles={iconStyles} iconName={'TeamsLogo'} aria-hidden="true" />
            </span>
          </Stack.Item>
        </Stack>
      </footer>
    </Sticky>
  );
};

export default Footer;
