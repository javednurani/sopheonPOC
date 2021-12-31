import { Link, Spinner } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { CSSProperties } from 'react';
import { useIntl } from 'react-intl';

import { ChangeEvent } from './data/changeEvents';
import { HistoryItem } from './types';

export type HistoryListProps = {
  events: HistoryItem[] | null;
};

const HistoryList: React.FC<HistoryListProps> = ({ events }: HistoryListProps) => {
  const { formatMessage } = useIntl();
  const theme = useTheme();
  const linkStyles: CSSProperties = {
    color: theme.palette.themePrimary,
  };
  const handleTogglePreviousValueClick = () => {
    const previousValueElement = event.target.parentElement.nextElementSibling;
    if (previousValueElement.style.display === 'none') {
      previousValueElement.style.display = 'block';
      event.target.innerHTML = formatMessage({ id: 'history.hidePreviousValue' });
    } else {
      previousValueElement.style.display = 'none';
      event.target.innerHTML = formatMessage({ id: 'history.showPreviousValue' });
    }
  };

  if (!events) {
    return <Spinner />;
  }

  if (events.length === 0) {
    return <div>{formatMessage({ id: 'history.none' })}</div>;
  }

  return (
    <>
      {events.map((evt, idx) => (
        <div key={idx} style={{ marginBottom: 15 }}>
          <div>
            {ChangeEvent[evt.event]} {evt.item}
          </div>
          <div>{evt.eventDate.toLocaleString()}</div>
          <div>
            <Link variant="xSmall" onClick={handleTogglePreviousValueClick} style={linkStyles}>
              {formatMessage({ id: 'history.showPreviousValue' })}
            </Link>
          </div>
          <div style={{ display: 'none' }}>{evt.previousValue}</div>
        </div>
      ))}
    </>
  );
};

export default HistoryList;
