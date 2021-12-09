import { mergeStyleSets, Modal } from '@fluentui/react';
import { Text } from '@fluentui/react/lib/Text';
import { useBoolean } from '@fluentui/react-hooks';
import { FontIcon, mergeStyles, Stack } from 'office-ui-fabric-react';
import React from 'react';
import { useIntl } from 'react-intl';

import AddTask from './AddTask';

export interface IToDoListProps {}

const mainDivStyle: React.CSSProperties = {
  width: '100%',
  marginLeft: '48px',
  marginRight: '48px',
  marginTop: '35px',
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

// AddTask modal style
const addTaskModalStyles = mergeStyleSets({
  container: {
    height: '560px',
    width: '956px',
  },
});

const addIconStyle: React.CSSProperties = {
  cursor: 'pointer',
};

// TODO: remove this for 1693 version
interface ToDoItem {
  name?: string;
  notes?: string;
  dueDate?: Date;
  status?: Status;
}

// TODO: remove this for 1693 version
// eslint-disable-next-line no-shadow
enum Status {
  NotStarted = -1,
  InProgress = -2,
  Assigned = -3,
  Complete = -4,
}

const ToDoList: React.FunctionComponent<IToDoListProps> = ({}: IToDoListProps) => {
  const { formatMessage } = useIntl();
  const [isTaskModalOpen, { setTrue: showTaskModal, setFalse: hideTaskModal }] = useBoolean(false);

  const dummyData: ToDoItem[] = [
    {
      name: 'ZachName',
      dueDate: new Date('01/01/2022'),
      status: Status.InProgress,
    },
    {
      name: 'ZachName2',
      status: Status.Complete,
    },
  ];

  const emptyListContent: JSX.Element = (
    <div style={contentDivStyle}>
      <Text variant="xLarge">
        {formatMessage({ id: 'toDo.empty1' })}
        <FontIcon iconName="CirclePlus" />
        {formatMessage({ id: 'toDo.empty2' })}
      </Text>
    </div>
  );

  // TODO: convert date UTC->local
  const populatedListContent: JSX.Element = (
    <div>
      {dummyData.map((item, index) => {
        const dueDateDisplay: string = item.dueDate ? item.dueDate.toLocaleDateString() : 'xxxx';
        const statusIcon = item.status === Status.Complete ? <FontIcon iconName="CircleRing" /> : <FontIcon iconName="CircleRing" />;

        return (
          <div key={index}>
            <Stack horizontal>
              <Stack.Item>{statusIcon}</Stack.Item>
              <Stack.Item align="stretch">
                <div>{item.name}</div>
                <div>Due {dueDateDisplay}</div>
              </Stack.Item>
            </Stack>
            <hr />
          </div>
        );
      })}
    </div>
  );

  const toDoListContent: JSX.Element = dummyData.length === 0 ? emptyListContent : populatedListContent;

  return (
    <div style={mainDivStyle}>
      <Stack horizontal>
        <Stack.Item grow>
          <Text variant="xxLarge">{formatMessage({ id: 'toDo.title' })}</Text>
          <Text variant="xLarge">
            <FontIcon style={addIconStyle} onClick={showTaskModal} iconName="CirclePlus" className={iconClass} />
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
      {toDoListContent}
      <Modal
        titleAriaId="TaskModal"
        isOpen={isTaskModalOpen}
        onDismiss={hideTaskModal}
        isBlocking={true}
        containerClassName={addTaskModalStyles.container}
      >
        <AddTask hideModal={hideTaskModal} />
      </Modal>
    </div>
  );
};

export default ToDoList;
