import { FontIcon, ITextFieldStyles, mergeStyles, mergeStyleSets, Modal, Stack } from '@fluentui/react';
import { Text } from '@fluentui/react/lib/Text';
import { useBoolean } from '@fluentui/react-hooks';
import React from 'react';
import { useIntl } from 'react-intl';

import AddTask from './AddTask';
import { UpdateProductAction, UpdateProductItemAction } from './product/productReducer';
import { Attributes, Product, Status, ToDoItem, UpdateProductItemModel, UpdateProductModel } from './types';

export interface IToDoListProps {
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  updateProductItem: (productItem: UpdateProductItemModel) => UpdateProductItemAction;
  environmentKey: string;
  accessToken: string;
  products: Product[];
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

// AddTask modal style
const addTaskModalStyles = mergeStyleSets({
  container: {
    height: '560px',
    width: '956px',
  },
});

const pointerCursorStyle: React.CSSProperties = {
  cursor: 'pointer',
};

const emptyDueDateStyles: Partial<ITextFieldStyles> = {
  root: {
    color: 'red',
  },
};

const completedNameStyles: Partial<ITextFieldStyles> = {
  root: {
    textDecoration: 'line-through',
  },
};

const ToDoList: React.FunctionComponent<IToDoListProps> = ({
  updateProduct,
  updateProductItem,
  environmentKey,
  accessToken,
  products,
}: IToDoListProps) => {
  const { formatMessage } = useIntl();
  const [isTaskModalOpen, { setTrue: showTaskModal, setFalse: hideTaskModal }] = useBoolean(false);
  const { todos } = products[0];
  const [isFilteredToComplete, { toggle: toggleFiltered }] = useBoolean(false);

  const handleStatusIconClick = (todo: ToDoItem) => {
    // if it's not complete, mark it as complete, otherwise in progress
    todo.status = todo.status !== Status.Complete ? Status.Complete : Status.InProgress;
    callUpdateProductItemSaga(todo);
  };

  const callUpdateProductItemSaga = (todo: ToDoItem) => {
    const updateProductItemDto: UpdateProductItemModel = {
      ProductKey: products[0].key || 'BAD_PRODUCT_KEY',
      EnvironmentKey: environmentKey,
      AccessToken: accessToken,
      ProductItem: {
        Id: todo.id,
        EnumCollectionAttributeValues: [
          {
            AttributeId: Attributes.STATUS,
            Value: [
              {
                EnumAttributeOptionId: todo.status,
              },
            ],
          },
        ],
        // TODO: might need to send other values in future
      },
    };
    updateProductItem(updateProductItemDto);
  };

  const handleFilterIconClick = () => {
    toggleFiltered();
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
      {todos
        .filter(todo => (isFilteredToComplete ? todo.status === Status.Complete : true))
        .map((todo, index) => {
          const emptyNamePlaceholder = 'xxxx';
          const statusIcon: JSX.Element = (
            <FontIcon
              iconName={todo.status === Status.Complete ? 'CheckMark' : 'CircleRing'}
              style={pointerCursorStyle}
              onClick={() => handleStatusIconClick(todo)}
            />
          );
          const name: JSX.Element =
            todo.status === Status.Complete ? <Text styles={completedNameStyles}>{todo.name}</Text> : <Text>{todo.name}</Text>;
          const dueDate: JSX.Element = todo.dueDate ? (
            <Text>{todo.dueDate.toLocaleDateString()}</Text>
          ) : (
            <Text styles={emptyDueDateStyles}>{emptyNamePlaceholder}</Text>
          );

          return (
            <div key={index}>
              <Stack horizontal>
                <Stack.Item>{statusIcon}</Stack.Item>
                <Stack.Item align="stretch">
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

  const toDoListContent: JSX.Element = todos.length === 0 ? emptyListContent : populatedListContent;

  return (
    <div style={mainDivStyle}>
      <Stack horizontal>
        <Stack.Item grow>
          <Text variant="xxLarge">{formatMessage({ id: 'toDo.title' })}</Text>
          <Text variant="xLarge">
            <FontIcon style={pointerCursorStyle} onClick={showTaskModal} iconName="CirclePlus" className={iconClass} />
          </Text>
        </Stack.Item>
        <Stack.Item>
          <Text variant="xLarge">
            {/* TODO: combine class and styles? */}
            <FontIcon
              iconName={isFilteredToComplete ? 'FilterSolid' : 'Filter'}
              className={filterSortIconClass}
              style={pointerCursorStyle}
              onClick={handleFilterIconClick}
            />
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
        {/* <div>Display only Modal :) does it work nice with the IdleMonitor?</div> */}
        <AddTask
          hideModal={hideTaskModal}
          updateProduct={updateProduct}
          environmentKey={environmentKey}
          accessToken={accessToken}
          products={products}
        />
      </Modal>
    </div>
  );
};

export default ToDoList;
