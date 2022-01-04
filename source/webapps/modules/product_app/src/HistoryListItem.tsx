import { Link, Text } from '@fluentui/react';
import { useBoolean } from '@fluentui/react-hooks';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { CSSProperties } from 'react';
import { useIntl } from 'react-intl';

import { ChangeEvent } from './data/changeEvents';
import { HistoryItem } from './types';

// extending HistoryItem since thats all this component needs
export type HistoryListItemProps = {} & HistoryItem;

const HistoryListItem: React.FC<HistoryListItemProps> = ({
  event: changeEvent,
  item: fieldName,
  eventDate: date,
  previousValue,
}: HistoryListItemProps) => {
  const { formatMessage } = useIntl();
  const [isPrevValueShown, { toggle: togglePrevValueShown }] = useBoolean(false);
  const theme = useTheme();

  const linkStyles: CSSProperties = {
    color: theme.palette.themePrimary,
  };

  return (
    <>
      <div>
        <Text variant="small">
          {ChangeEvent[changeEvent]} {fieldName}
        </Text>
      </div>
      <div>
        <Text variant="xSmall">{date.toLocaleString()}</Text>
      </div>
      {changeEvent === ChangeEvent.Updated && (
        <div>
          <Link onClick={togglePrevValueShown} style={linkStyles}>
            <Text variant="xSmall">{formatMessage({ id: isPrevValueShown ? 'history.hidePreviousValue' : 'history.showPreviousValue' })}</Text>
          </Link>
        </div>
      )}
      {isPrevValueShown && (
        <div>
          <Text variant="xSmall">{previousValue}</Text>
        </div>
      )}
    </>
  );
};

export default HistoryListItem;
