import { Spinner } from '@fluentui/react';
import React, { CSSProperties } from 'react';
import { useIntl } from 'react-intl';

import HistoryListItem from './HistoryListItem';
import { HistoryItem } from './types';

export type HistoryListProps = {
  events: HistoryItem[] | null;
};

const HistoryList: React.FC<HistoryListProps> = ({ events }: HistoryListProps) => {
  const { formatMessage } = useIntl();

  const itemStyles: CSSProperties = {
    marginBottom: 15,
  };

  if (!events) {
    return <Spinner />;
  }

  if (events.length === 0) {
    return <div>{formatMessage({ id: 'history.none' })}</div>;
  }

  const items: JSX.Element[] = events.map((evt, idx) => (
    <div key={idx} style={itemStyles}>
      <HistoryListItem {...evt} />
    </div>
  ));

  return <div>{items}</div>;
};

export default HistoryList;
