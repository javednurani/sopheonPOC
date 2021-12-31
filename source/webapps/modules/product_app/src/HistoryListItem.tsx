import { Link } from '@fluentui/react';
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
        {ChangeEvent[changeEvent]} {fieldName}
      </div>
      <div>{date.toLocaleString()}</div>
      <div>
        <Link variant="xSmall" onClick={togglePrevValueShown} style={linkStyles}>
          {formatMessage({ id: isPrevValueShown ? 'history.hidePreviousValue' : 'history.showPreviousValue' })}
        </Link>
      </div>
      {isPrevValueShown && <div> {previousValue}</div>}
    </>
  );
};

export default HistoryListItem;
