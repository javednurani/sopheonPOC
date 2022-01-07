import { IStackItemStyles, IStackStyles, IStackTokens, IStyle, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';

import KPIs from './KPIs';
import { CreateTaskAction, DeleteTaskAction, UpdateProductAction, UpdateProductItemAction, UpdateTaskAction } from './product/productReducer';
import ProductHealth from './ProductHealth';
import ProductSection from './ProductSection';
import ResourcesAndLinks from './ResourcesAndLinks';
import Timeline from './timeline/Timeline';
import ToDoList from './ToDoList';
import { DeleteTaskModel, PostPutTaskModel, Product, UpdateProductItemModel, UpdateProductModel } from './types';

export interface IDashboardProps {
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  updateProductItem: (productItem: UpdateProductItemModel) => UpdateProductItemAction;
  environmentKey: string;
  accessToken: string;
  products: Product[];
  createTask: (task: PostPutTaskModel) => CreateTaskAction;
  updateTask: (task: PostPutTaskModel) => UpdateTaskAction;
  deleteTask: (task: DeleteTaskModel) => DeleteTaskAction;
}

const stackTokens: IStackTokens = {
  childrenGap: 10,
  padding: 10,
};

const Dashboard: React.FunctionComponent<IDashboardProps> = ({
  updateProduct,
  updateProductItem,
  environmentKey,
  accessToken,
  products,
  createTask,
  updateTask,
  deleteTask,
}: IDashboardProps) => {
  const theme = useTheme();

  const sharedStackItemStyles: Partial<IStyle> = {
    display: 'flex',
    justifyContent: 'center',
    border: '1px solid',
    borderColor: theme.palette.neutralTertiary,
    borderRadius: '3px',
    overflow: 'auto',
  };

  const topRowStackItemStyles: IStackItemStyles = {
    root: {
      ...sharedStackItemStyles,
      height: '14vh',
    },
  };

  const middleRowStackItemStyles: IStackItemStyles = {
    root: {
      ...sharedStackItemStyles,
      height: '52vh',
    },
  };

  const bottomRowStackItemStyles: IStackItemStyles = {
    root: {
      ...sharedStackItemStyles,
      height: '18vh',
    },
  };

  const stackStyles: IStackStyles = {
    root: {
      height: '100%',
      width: '620px',
    },
  };

  return (
    <Stack horizontal styles={stackStyles}>
      <Stack.Item grow={4}>
        <Stack styles={stackStyles} tokens={stackTokens}>
          <Stack.Item styles={topRowStackItemStyles}>
            <ProductSection product={products[0]} />
          </Stack.Item>
          <Stack.Item styles={middleRowStackItemStyles}>
            <ToDoList
              updateProduct={updateProduct}
              updateProductItem={updateProductItem}
              environmentKey={environmentKey}
              accessToken={accessToken}
              products={products}
              createTask={createTask}
              updateTask={updateTask}
              deleteTask={deleteTask}
            />
          </Stack.Item>
          <Stack.Item styles={bottomRowStackItemStyles}>
            <ResourcesAndLinks />
          </Stack.Item>
        </Stack>
      </Stack.Item>
      <Stack.Item grow={7}>
        <Stack styles={stackStyles} tokens={stackTokens}>
          <Stack.Item styles={topRowStackItemStyles}>
            <KPIs />
          </Stack.Item>
          <Stack.Item styles={middleRowStackItemStyles}>
            <Timeline product={products[0]} />
          </Stack.Item>
          <Stack.Item styles={bottomRowStackItemStyles}>
            <ProductHealth />
          </Stack.Item>
        </Stack>
      </Stack.Item>
    </Stack>
  );
};

export default Dashboard;
