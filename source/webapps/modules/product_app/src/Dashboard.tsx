import { IStackItemStyles, IStackStyles, IStackTokens, IStyle, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';

import KPIs from './KPIs';
import { UpdateProductAction, UpdateProductItemAction } from './product/productReducer';
import ProductHealth from './ProductHealth';
import ProductSection from './ProductSection';
import ResourcesAndLinks from './ResourcesAndLinks';
import Timeline from './timeline/Timeline';
import ToDoList from './ToDoList';
import { Product, UpdateProductItemModel, UpdateProductModel } from './types';

export interface IDashboardProps {
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  updateProductItem: (productItem: UpdateProductItemModel) => UpdateProductItemAction;
  environmentKey: string;
  accessToken: string;
  products: Product[];
}

const stackTokens: IStackTokens = {
  childrenGap: 5,
  padding: 5,
};

const Dashboard: React.FunctionComponent<IDashboardProps> = ({
  updateProduct,
  updateProductItem,
  environmentKey,
  accessToken,
  products,
}: IDashboardProps) => {
  const theme = useTheme();

  const stackItemStyles: IStackItemStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      display: 'flex',
      justifyContent: 'center',
    },
  };

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

  const stackStyles: IStackStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      height: '100%',
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
            <ToDoList
              updateProduct={updateProduct}
              updateProductItem={updateProductItem}
              environmentKey={environmentKey}
              accessToken={accessToken}
              products={products}
            />
          </Stack.Item>
          <Stack.Item grow styles={stackItemStyles}>
            <Timeline product={products[0]} />
          </Stack.Item>
        </Stack>
      </Stack.Item>
      <Stack.Item grow={2}>
        <Stack horizontal styles={stackStyles} tokens={stackTokens}>
          <Stack.Item grow styles={stackItemStyles}>
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
          <Stack.Item grow styles={stackItemStyles}>
            <ProductHealth />
          </Stack.Item>
        </Stack>
      </Stack.Item>
    </Stack>
  );
};

export default Dashboard;
