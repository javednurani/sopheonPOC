import { gantt } from '@sopheon/controls/ext/Gantt';
import Gantt from '@sopheon/controls/src/components/Gantt';
import { Stack } from 'office-ui-fabric-react';
import React from 'react';

import { Product } from '../types';

export interface ITimelineProps {
  product: Product;
}

const Timeline: React.FunctionComponent<ITimelineProps> = ({ product }: ITimelineProps) => {
  const todoItems = product.todos.map(todo => ({ id: `${todo.name}_${todo.dueDate}`, text: todo.name, type: gantt.config.types.milestone, start_date: todo.dueDate }));

  const mainStackStyle: React.CSSProperties = {
    width: '100%',
    height: '100%',
    marginLeft: '10px',
    marginRight: '10px',
    marginTop: '10px',
    paddingBottom: '20px',
  };

  return (
    <Stack style={mainStackStyle}>
      <Gantt todoItems={todoItems}></Gantt>
    </Stack>
  );
};

export default Timeline;
