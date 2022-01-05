import { Stack } from '@fluentui/react';
import { Gantt } from '@sopheon/controls';
import React from 'react';

import { Product } from '../types';

export interface ITimelineProps {
  product: Product;
}

const Timeline: React.FunctionComponent<ITimelineProps> = ({ product }: ITimelineProps) => {
  const todoItems = product.tasks.map(task => ({
    id: `${task.name}_${task.dueDate}`,
    text: task.name,
    type: 'milestone',
    start_date: task.dueDate,
  }));

  const mainStackStyle: React.CSSProperties = {
    width: '100%',
    height: '98%',
    marginLeft: '10px',
    marginRight: '10px',
    marginTop: '10px',
    paddingBottom: '10px',
  };

  return (
    <Stack style={mainStackStyle}>
      <Gantt todoItems={todoItems}></Gantt>
    </Stack>
  );
};

export default Timeline;
