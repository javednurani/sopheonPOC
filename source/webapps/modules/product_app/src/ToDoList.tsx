import { Stack, StackItem } from 'office-ui-fabric-react';
import React from 'react';

export interface IToDoListProps {}

const mainDivStyle: React.CSSProperties = {
  width: '100%',
};

const headingLeftStyle: React.CSSProperties = {
  textAlign: 'left',
};

const ToDoList: React.FunctionComponent<IToDoListProps> = ({}: IToDoListProps) => (
  <div style={mainDivStyle}>
    <Stack horizontal>
      <Stack.Item grow style={headingLeftStyle}>
        To Do +
      </Stack.Item>
      <Stack.Item>XYZ</Stack.Item>
    </Stack>
    <hr />
    <div>You don't have any tasks yet. Click + above to add one.</div>
  </div>
);

export default ToDoList;
