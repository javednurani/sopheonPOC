import { mergeStyleSets, Modal } from '@fluentui/react';
import { Text } from '@fluentui/react/lib/Text';
import { useBoolean } from '@fluentui/react-hooks';
import { FontIcon, mergeStyles, Stack } from 'office-ui-fabric-react';
import React from 'react';
import { useIntl } from 'react-intl';

import AddTask from './AddTask';
import { UpdateProductAction } from './product/productReducer';
import { Product, UpdateProductModel } from './types';

export interface IToDoListProps {
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
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

const ToDoList: React.FunctionComponent<IToDoListProps> = ({ updateProduct, environmentKey, accessToken, products }: IToDoListProps) => {
  const { formatMessage } = useIntl();
  const [isTaskModalOpen, { setTrue: showTaskModal, setFalse: hideTaskModal }] = useBoolean(false);

  return (
    <div style={mainDivStyle}>
      <Stack horizontal>
        <Stack.Item grow style={headingLeftStyle}>
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
      <div style={contentDivStyle}>
        <Text variant="xLarge">
          {formatMessage({ id: 'toDo.empty1' })}
          <FontIcon iconName="CirclePlus" />
          {formatMessage({ id: 'toDo.empty2' })}
        </Text>
      </div>
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
