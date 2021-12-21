import { IStackItemStyles, IStackStyles, IStackTokens, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';

import KPIs from './KPIs';
import { UpdateProductAction } from './product/productReducer';
import ProductHealth from './ProductHealth';
import ProductSection from './ProductSection';
import ResourcesAndLinks from './ResourcesAndLinks';
import Timeline from './Timeline';
import ToDoList from './ToDoList';
import { Product, UpdateProductModel } from './types';

export interface IDashboardProps {
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  environmentKey: string;
  accessToken: string;
  products: Product[];
}

const stackTokens: IStackTokens = {
  childrenGap: 5,
  padding: 5,
};

const Dashboard: React.FunctionComponent<IDashboardProps> = ({ updateProduct, environmentKey, accessToken, products }: IDashboardProps) => {
  const theme = useTheme();

  const stackItemStyles: IStackItemStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      display: 'flex',
      justifyContent: 'center',
    },
  };

  const stackStyles: IStackStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      height: '100%',
      width: '620px',
    },
  };

  return (
    <Stack styles={stackStyles}>
      <Stack.Item shrink>
        <Stack horizontal styles={stackStyles} tokens={stackTokens}>
          <Stack.Item styles={stackItemStyles}>
            <ProductSection product={products[0]} />
          </Stack.Item>
          <Stack.Item grow styles={stackItemStyles}>
            <KPIs />
          </Stack.Item>
        </Stack>
      </Stack.Item>
      <Stack.Item grow={5}>
        <Stack horizontal styles={stackStyles} tokens={stackTokens}>
          <Stack.Item grow styles={stackItemStyles}>
            <ToDoList updateProduct={updateProduct} environmentKey={environmentKey} accessToken={accessToken} products={products} />
          </Stack.Item>
          <Stack.Item grow styles={stackItemStyles}>
            <Timeline />
          </Stack.Item>
        </Stack>
      </Stack.Item>
      <Stack.Item grow={2}>
        <Stack horizontal styles={stackStyles} tokens={stackTokens}>
          <Stack.Item grow styles={stackItemStyles}>
            <ResourcesAndLinks />
          </Stack.Item>
          <Stack.Item grow styles={stackItemStyles}>
            <ProductHealth />
          </Stack.Item>
        </Stack>
      </Stack.Item>
    </Stack>
  );
};

export default Dashboard;
