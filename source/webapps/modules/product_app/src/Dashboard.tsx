import { IStackItemStyles, IStackStyles, IStackTokens, IStyle, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';

import KPIs from './KPIs';
import ProductHealth from './ProductHealth';
import ProductSection from './ProductSection';
import ResourcesAndLinks from './ResourcesAndLinks';
import Timeline from './Timeline';
import { Product } from './types';

export interface IDashboardProps {
  products: Product[];
}

const stackTokens: IStackTokens = {
  childrenGap: 10,
  padding: 10,
};

const Dashboard: React.FunctionComponent<IDashboardProps> = ({ products }: IDashboardProps) => {
  const theme = useTheme();

  const sharedStackItemStyles: Partial<IStyle> = {
    background: theme.semanticColors.bodyBackground, // TODO: why needed?
    //backgroundImage: `linear-gradient(to bottom right, ${theme.semanticColors.bodyBackground}, ${theme.semanticColors.bodyBackgroundHovered})`,
    display: 'flex',
    justifyContent: 'center',
    border: '1px solid',
    borderColor: theme.palette.neutralTertiary,
    borderRadius: '3px',
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
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      height: '100%',
      width: '100%',
    },
  };

  return (
    <Stack horizontal styles={stackStyles}>
      <Stack.Item grow={4}>
        <Stack styles={stackStyles} tokens={stackTokens}>
          <Stack.Item styles={topRowStackItemStyles}>
            <ProductSection product={products[0]} />
          </Stack.Item>
          <Stack.Item styles={middleRowStackItemStyles}>To Do List</Stack.Item>
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
            <Timeline />
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
