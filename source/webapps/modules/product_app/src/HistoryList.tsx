import { Spinner } from '@fluentui/react';
import React from 'react';

import { HistoryItem } from './types';

export type HistoryListProps = {
  events: HistoryItem[] | null;
};

const HistoryList: React.FC<HistoryListProps> = ({ events }: HistoryListProps) => {
  if (!events) {
    return <Spinner />;
  }

  if (events.length === 0) {
    return <div>There is no history available.</div>;
  }
  return (
    <>
      {events.map(evt => (
        <div key={evt.id}>
          {evt.event} on {evt.eventDate.toDateString()}
        </div>
      ))}
    </>
  );
};

export default HistoryList;
