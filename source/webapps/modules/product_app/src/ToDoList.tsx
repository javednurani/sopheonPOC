import { FontIcon, mergeStyles, Stack, StackItem } from 'office-ui-fabric-react';
import React from 'react';

export interface IToDoListProps {}

const mainDivStyle: React.CSSProperties = {
  width: '100%',
  marginLeft: '48px',
  marginRight: '48px',
  marginTop: '35px',
};

const headingLeftStyle: React.CSSProperties = {
  textAlign: 'left',
};

const contentDivStyle: React.CSSProperties = {
  marginTop: '24px',
};

const iconClass = mergeStyles({
  marginLeft: '7px',
});

const ToDoList: React.FunctionComponent<IToDoListProps> = ({}: IToDoListProps) => (
  <div style={mainDivStyle}>
    <Stack horizontal>
      <Stack.Item grow style={headingLeftStyle}>
        To Do <FontIcon iconName="CirclePlus" className={iconClass} />
      </Stack.Item>
      <Stack.Item>
        <FontIcon iconName="Filter" className={iconClass} />
        <FontIcon iconName="Sort" className={iconClass} />
      </Stack.Item>
    </Stack>
    <hr />
    <div style={contentDivStyle}>
      You don't have any tasks yet. Click <FontIcon iconName="CirclePlus" /> above to add one.
    </div>
  </div>
);

export default ToDoList;
