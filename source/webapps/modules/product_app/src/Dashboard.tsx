import { IStackItemStyles, IStackStyles, IStackTokens, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';

export interface IDashboardProps {}

const stackTokens: IStackTokens = {
  childrenGap: 5,
  padding: 10,
};

const Dashboard: React.FunctionComponent<IDashboardProps> = ({}: IDashboardProps) => {
  const theme = useTheme();

  const stackItemStyles: IStackItemStyles = {
    root: {
      alignItems: 'center',
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
    },
  };

  return (
    <Stack horizontal styles={stackStyles} tokens={stackTokens}>
      <Stack.Item grow={4} styles={stackItemStyles}>
        Main Left
      </Stack.Item>
      <Stack.Item grow={7} styles={stackItemStyles}>
        Main Right
      </Stack.Item>
    </Stack>
  );
};

export default Dashboard;
