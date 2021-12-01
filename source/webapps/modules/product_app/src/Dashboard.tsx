import { DefaultPalette, IStackItemStyles, IStackStyles, IStackTokens, Stack } from '@fluentui/react';
import React from 'react';

export interface IDashboardProps {}

const stackStyles: IStackStyles = {
  root: {
    //background: DefaultPalette.themeTertiary,
    height: '100%',
  },
};
const stackItemStyles: IStackItemStyles = {
  root: {
    alignItems: 'center',
    background: 'white',
    //color: DefaultPalette.white,
    display: 'flex',
    justifyContent: 'center',
    border: '1px solid',
    borderColor: 'black',
  },
};

const stackTokens: IStackTokens = {
  childrenGap: 5,
  padding: 10,
};

const Dashboard: React.FunctionComponent<IDashboardProps> = ({}: IDashboardProps) => (
  <Stack horizontal styles={stackStyles} tokens={stackTokens}>
    <Stack.Item grow={4} styles={stackItemStyles}>
      Main Left
    </Stack.Item>
    <Stack.Item grow={7} styles={stackItemStyles}>
      Main Right
    </Stack.Item>
  </Stack>
);

export default Dashboard;
