import { Spinner } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';
import { useIntl } from 'react-intl';
import { Link } from 'react-router-dom';

import { ChangeEvent } from './data/changeEvents';
import { HistoryItem } from './types';

export type HistoryListProps = {
  events: HistoryItem[] | null;
};

const handleTogglePreviousValueClick = () => {
  const previousValueElement = event.target.nextElementSibling;
  if (previousValueElement.style.display === 'none') {
    previousValueElement.style.display = 'block';
    event.target.innerHTML = 'history.hidePreviousValue';
  } else {
    previousValueElement.style.display = 'none';
    event.target.innerHTML = 'history.showPreviousValue';
  }
};

const HistoryList: React.FC<HistoryListProps> = ({ events }: HistoryListProps) => {
  const { formatMessage } = useIntl();
  const theme = useTheme();

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
          <div style={{ color: theme.palette.themePrimary, fontSize: `${theme.fonts.xSmall}px` }} onClick={handleTogglePreviousValueClick}>
            {formatMessage({ id: 'history.showPreviousValue' })}
          </div>
          <div style={{ display: 'none' }}>{evt.previousValue}</div>
        </div>
      ))}
    </>
  );
};

export default HistoryList;
