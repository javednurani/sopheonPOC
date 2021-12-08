import { Text } from '@fluentui/react/lib/Text';
import { FontIcon, mergeStyles, Stack } from 'office-ui-fabric-react';
import React from 'react';
import { useIntl } from 'react-intl';

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
  marginLeft: '100px',
  marginRight: '100px',
};

const iconClass = mergeStyles({
  marginLeft: '7px',
});

const filterSortIconClass = mergeStyles({
  marginLeft: '7px',
  verticalAlign: 'bottom',
});

const ToDoList: React.FunctionComponent<IToDoListProps> = ({}: IToDoListProps) => {
  const { formatMessage } = useIntl();

  return (
    <div style={mainDivStyle}>
      <Stack horizontal>
        <Stack.Item grow style={headingLeftStyle}>
          <Text variant="xxLarge">{formatMessage({ id: 'toDo.title' })}</Text>
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
          {formatMessage({ id: 'toDo.empty1' })}
          <FontIcon iconName="CirclePlus" />
          {formatMessage({ id: 'toDo.empty2' })}
        </Text>
      </div>
    </div>
  );
};

export default ToDoList;
