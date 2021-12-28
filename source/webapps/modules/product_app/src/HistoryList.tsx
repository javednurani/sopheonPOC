import { Spinner } from '@fluentui/react';
import React from 'react';
import { useIntl } from 'react-intl';

import { HistoryItem } from './types';

export type HistoryListProps = {
  events: HistoryItem[] | null;
};

const HistoryList: React.FC<HistoryListProps> = ({ events }: HistoryListProps) => {
  const { formatMessage } = useIntl();

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
            {evt.event} {evt.item}
          </div>
          <div>{evt.eventDate.toLocaleString()}</div>
          <div>{evt.previousValue}</div>
        </div>
      ))}
    </>
  );
};

export default HistoryList;
