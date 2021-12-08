import { ITextProps, Text } from '@fluentui/react/lib/Text';
import { FontIcon, FontSizes, mergeStyles, Stack, StackItem } from 'office-ui-fabric-react';
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
  margin: '24px',
  marginLeft: '72px',
  marginRight: '72px',
};

const iconClass = mergeStyles({
  marginLeft: '7px',
});

const filterSortIconClass = mergeStyles({
  marginLeft: '7px',
  verticalAlign: 'bottom',
});

const ToDoList: React.FunctionComponent<IToDoListProps> = ({}: IToDoListProps) => (
  <div style={mainDivStyle}>
    <Stack horizontal>
      <Stack.Item grow style={headingLeftStyle}>
        <Text variant="xxLarge">To Do</Text>
        <Text variant="xLarge">
          <FontIcon iconName="CirclePlus" className={iconClass} />
        </Text>
      </Stack.Item>
      <Stack.Item>
        <Text variant="xLarge">
          <FontIcon iconName="Filter" className={filterSortIconClass} />
          <FontIcon iconName="Sort" className={filterSortIconClass} />
        </Text>
      </Stack.Item>
    </Stack>
    <hr />
    <div style={contentDivStyle}>
      <Text variant="xLarge">
        You don't have any tasks yet. Click <FontIcon iconName="CirclePlus" /> above to add one.
      </Text>
    </div>
  </div>
);

export default ToDoList;
