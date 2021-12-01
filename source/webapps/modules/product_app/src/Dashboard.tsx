import { IStackItemStyles, IStackStyles, IStackTokens, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';

export interface IDashboardProps {}

const stackTokens: IStackTokens = {
  childrenGap: 5,
  padding: 5,
};

const Dashboard: React.FunctionComponent<IDashboardProps> = ({}: IDashboardProps) => {
  const theme = useTheme();

  const stackItemStyles: IStackItemStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      display: 'flex',
      justifyContent: 'center',
      border: '1px solid',
      borderColor: theme.semanticColors.bodyText,
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
    <Stack horizontal styles={stackStyles} tokens={stackTokens}>
      <Stack.Item grow={4}>
        <Stack styles={stackStyles} tokens={stackTokens}>
          <Stack.Item grow={2} styles={stackItemStyles}>
            Product Section
          </Stack.Item>
          <Stack.Item grow={9} styles={stackItemStyles}>
            To Do List
          </Stack.Item>
          <Stack.Item grow={3} styles={stackItemStyles}>
            Resources & Links
          </Stack.Item>
        </Stack>
      </Stack.Item>
      <Stack.Item grow={7}>
        <Stack styles={stackStyles} tokens={stackTokens}>
          <Stack.Item grow={2} styles={stackItemStyles}>
            KPI's
          </Stack.Item>
          <Stack.Item grow={9} styles={stackItemStyles}>
            Timeline
          </Stack.Item>
          <Stack.Item grow={3} styles={stackItemStyles}>
            Product Health & extra KPIs
          </Stack.Item>
        </Stack>
      </Stack.Item>
    </Stack>
  );
};

export default Dashboard;
