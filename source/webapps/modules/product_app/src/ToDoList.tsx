import React from 'react';

export interface IToDoListProps {}

const mainDivStyle: React.CSSProperties = {
  width: '100%',
};

const ToDoList: React.FunctionComponent<IToDoListProps> = ({}: IToDoListProps) => (
  <div style={mainDivStyle}>
    <div>
      <span>To Do +</span>
      <span>XYZ</span>
    </div>
    <hr />
    <div>You don't have any tasks yet. Click + above to add one.</div>
  </div>
);

export default ToDoList;
