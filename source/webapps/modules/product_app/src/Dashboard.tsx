import { IStackItemStyles, IStackStyles, IStackTokens, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';

import KPIs from './KPIs';
import ProductHealth from './ProductHealth';
import ProductSection from './ProductSection';
import ResourcesAndLinks from './ResourcesAndLinks';
import Timeline from './Timeline';

export interface IDashboardProps {}

const stackTokens: IStackTokens = {
  childrenGap: 10,
  padding: 10,
};

const Dashboard: React.FunctionComponent<IDashboardProps> = ({}: IDashboardProps) => {
  const theme = useTheme();

  const stackItemStyles: IStackItemStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      //backgroundImage: `linear-gradient(to bottom right, ${theme.semanticColors.bodyBackground}, ${theme.semanticColors.bodyBackgroundHovered})`,
      display: 'flex',
      justifyContent: 'center',
      border: '1px solid',
      borderColor: theme.palette.neutralTertiary,
      borderRadius: '3px',
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
          <Stack.Item grow={2} styles={stackItemStyles}>
            <ProductSection />
          </Stack.Item>
          <Stack.Item grow={9} styles={stackItemStyles}>
            To Do List
          </Stack.Item>
          <Stack.Item grow={3} styles={stackItemStyles}>
            <ResourcesAndLinks />
          </Stack.Item>
        </Stack>
      </Stack.Item>
      <Stack.Item grow={7}>
        <Stack styles={stackStyles} tokens={stackTokens}>
          <Stack.Item grow={2} styles={stackItemStyles}>
            <KPIs />
          </Stack.Item>
          <Stack.Item grow={9} styles={stackItemStyles}>
            <Timeline />
          </Stack.Item>
          <Stack.Item grow={3} styles={stackItemStyles}>
            <ProductHealth />
          </Stack.Item>
        </Stack>
      </Stack.Item>
    </Stack>
  );
};

export default Dashboard;
