import {
  ContextualMenu,
  ContextualMenuItemType,
  FontIcon,
  IContextualMenuItem,
  IStackItemStyles,
  ITextFieldStyles,
  mergeStyles,
  mergeStyleSets,
  Modal,
  Stack,
} from '@fluentui/react';
import { Text } from '@fluentui/react/lib/Text';
import { useBoolean } from '@fluentui/react-hooks';
import { ShowAnnouncementAction, ShowAnnouncementModel } from '@sopheon/shell-api';
import React, { useRef, useState } from 'react';
import { useIntl } from 'react-intl';

import { Status } from './data/status';
import { CreateTaskAction, DeleteTaskAction, UpdateProductAction, UpdateProductItemAction, UpdateTaskAction } from './product/productReducer';
import TaskDetails from './TaskDetails';
import { DeleteTaskModel, PostPutTaskModel, Product, Task, TaskDto, UpdateProductItemModel, UpdateProductModel } from './types';

export interface IToDoListProps {
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  updateProductItem: (productItem: UpdateProductItemModel) => UpdateProductItemAction;
  environmentKey: string;
  accessToken: string;
  products: Product[];
  createTask: (task: PostPutTaskModel) => CreateTaskAction;
  updateTask: (task: PostPutTaskModel) => UpdateTaskAction;
  deleteTask: (task: DeleteTaskModel) => DeleteTaskAction;
  showAnnouncement: (announcement: ShowAnnouncementModel) => ShowAnnouncementAction;
}

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

// TaskDetails modal style
const taskDetailsModalStyles = mergeStyleSets({
  container: {
    height: '560px',
    width: '956px',
    display: 'flex',
  },
});

const pointerCursorStyle: React.CSSProperties = {
  cursor: 'pointer',
};

const sharedNameStyles: Partial<ITextFieldStyles> = {
  root: {
    'display': '-webkit-box',
    'overflow': 'hidden',
    '-webkit-line-clamp': '2',
    '-webkit-box-orient': 'vertical',
    'word-break': 'break-all',
    'marginBottom': '5px',
    'cursor': 'pointer',
  },
};

const completedNameStyles: Partial<ITextFieldStyles> = {
  root: {
    // eslint-disable-next-line @typescript-eslint/ban-types
    ...(sharedNameStyles.root as {}),
    textDecoration: 'line-through',
  },
};

const marginRightStackItemStyles: IStackItemStyles = {
  root: {
    marginRight: '7px',
  },
};

const todoContainterStyle: React.CSSProperties = {
  marginTop: 10,
};

// TODO: move to utility?
const isToday = (someDate: Date) => {
  const today = new Date();
  return someDate.getDate() === today.getDate() && someDate.getMonth() === today.getMonth() && someDate.getFullYear() === today.getFullYear();
};

const ToDoList: React.FunctionComponent<IToDoListProps> = ({
  updateProduct,
  updateProductItem, // INFO: createTask/updateTask pipes have replace use of updateProduct/updateProductItem.  can verify and remove dead code
  environmentKey,
  accessToken,
  products,
  createTask,
  updateTask,
  deleteTask,
  showAnnouncement,
}: IToDoListProps) => {
  const { tasks } = products[0];
  const { formatMessage } = useIntl();
  const [isTaskModalOpen, { setTrue: showTaskModal, setFalse: hideTaskModal }] = useBoolean(false);
  const [isFilteredToShowComplete, { toggle: toggleFiltered }] = useBoolean(false);
  const [isFilterContextMenuShown, { setFalse: hideFilterContextMenu, toggle: toggleFilterContextMenu }] = useBoolean(false);
  const filterContextMenuRef = useRef(null); // used to link context menu to element
  const [selectedTask, setSelectedTask] = useState<Task | null>(null);

  const filterMenuItems: IContextualMenuItem[] = [
    {
      key: 'filterSection',
      itemType: ContextualMenuItemType.Section,
      sectionProps: {
        title: formatMessage({ id: 'toDo.filter' }),
        items: [
          {
            key: 'showCompleted',
            text: formatMessage({ id: 'toDo.showCompleted' }),
            canCheck: true,
            checked: isFilteredToShowComplete,
            onClick: toggleFiltered,
          },
        ],
      },
    },
  ];

  const filterContextMenu: JSX.Element = (
    <ContextualMenu
      items={filterMenuItems}
      hidden={!isFilterContextMenuShown}
      target={filterContextMenuRef}
      onItemClick={hideFilterContextMenu}
      onDismiss={hideFilterContextMenu}
    />
  );

  const handleStatusIconClick = (todo: Task) => {
    // if it's not complete, mark it as complete, otherwise in progress
    todo.status = todo.status !== Status.Complete ? Status.Complete : Status.InProgress;
    makeUpdateTaskCall(todo);
  };

  const handleToDoListItemClick = (todo: Task) => {
    setSelectedTask(todo);
    showTaskModal();
  };

  const handleCreateToDoIconClick = () => {
    setSelectedTask(null);
    showTaskModal();
  };

  const makeUpdateTaskCall = (todo: Task) => {
    // use PUT /Tasks to make full update, even though we're only changing Status
    // Cloud-2157 captures an optional rework to use PATCH /Tasks for a partial update
    const task: TaskDto = {
      id: todo.id,
      name: todo.name,
      notes: todo.notes,
      status: todo.status,
      dueDate: todo.dueDate ? todo.dueDate.toDateString() : null,
    };
    const updateTaskModel: PostPutTaskModel = {
      ProductKey: products[0].key || 'BAD_PRODUCT_KEY',
      EnvironmentKey: environmentKey,
      AccessToken: accessToken,
      Task: task,
    };
    updateTask(updateTaskModel);
  };

  const emptyListContent: JSX.Element = (
    <div style={contentDivStyle}>
      <Text variant="xLarge">
        {formatMessage({ id: 'toDo.empty1' })}
        <FontIcon iconName="CirclePlus" />
        {formatMessage({ id: 'toDo.empty2' })}
      </Text>
    </div>
  );

  const populatedListContent: JSX.Element = (
    <div>
      {tasks
        .filter(todo => (isFilteredToShowComplete ? true : todo.status !== Status.Complete))
        .map((todo, index) => {
          const statusIcon: JSX.Element = (
            <Text variant="xLarge">
              <FontIcon
                iconName={todo.status === Status.Complete ? 'CheckMark' : 'CircleRing'}
                style={pointerCursorStyle}
                onClick={() => handleStatusIconClick(todo)}
              />
            </Text>
          );
          const name: JSX.Element = (
            <Text onClick={() => handleToDoListItemClick(todo)} styles={todo.status === Status.Complete ? completedNameStyles : sharedNameStyles}>
              {todo.name}
            </Text>
          );

          let dueDate: JSX.Element;
          if (todo.dueDate) {
            if (isToday(todo.dueDate)) {
              dueDate = <Text>Due Today</Text>;
            } else {
              const isPastDue: boolean = todo.dueDate < new Date();
              dueDate = (
                <Text style={isPastDue ? { color: 'red' } : {}}>
                  Due {todo.dueDate.toLocaleDateString(undefined, { year: '2-digit', month: 'numeric', day: 'numeric' })}
                </Text>
              );
            }
          } else {
            dueDate = <Text />; // display nothing
          }

          return (
            <div key={index} style={todoContainterStyle}>
              <Stack horizontal>
                <Stack.Item styles={marginRightStackItemStyles}>{statusIcon}</Stack.Item>
                <Stack.Item>
                  <div>{name}</div>
                  <div>{dueDate}</div>
                </Stack.Item>
              </Stack>
              <hr />
            </div>
          );
        })}
    </div>
  );

  const toDoListContent: JSX.Element = tasks.length === 0 ? emptyListContent : populatedListContent;

  return (
    <div style={mainDivStyle}>
      <Stack horizontal>
        <Stack.Item grow>
          <Text variant="xxLarge">{formatMessage({ id: 'toDo.title' })}</Text>
          <Text variant="xLarge">
            <FontIcon style={pointerCursorStyle} onClick={handleCreateToDoIconClick} iconName="CirclePlus" className={iconClass} />
          </Text>
        </Stack.Item>
        <Stack.Item>
          <Text variant="xLarge">
            <span ref={filterContextMenuRef}>
              {/* TODO: combine class and styles? */}
              <FontIcon
                iconName={isFilteredToShowComplete ? 'FilterSolid' : 'Filter'}
                className={filterSortIconClass}
                style={pointerCursorStyle}
                onClick={toggleFilterContextMenu}
              />
            </span>
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
        containerClassName={taskDetailsModalStyles.container}
      >
        {/* <div>Display only Modal :) does it work nice with the IdleMonitor?</div> */}
        <TaskDetails
          hideModal={hideTaskModal}
          updateProduct={updateProduct}
          environmentKey={environmentKey}
          accessToken={accessToken}
          products={products}
          selectedTask={selectedTask}
          updateProductItem={updateProductItem}
          createTask={createTask}
          updateTask={updateTask}
          deleteTask={deleteTask}
          showAnnouncement={showAnnouncement}
        />
      </Modal>
      {filterContextMenu}
    </div>
  );
};

export default ToDoList;
