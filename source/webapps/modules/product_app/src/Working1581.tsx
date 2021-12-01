import { FontIcon, IStackItemStyles, IStackStyles, mergeStyles, Stack } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';

export interface IWorking1581Props {
  productName: string;
}

const Working1581: React.FunctionComponent<IWorking1581Props> = ({ productName }: IWorking1581Props) => {
  const theme = useTheme();

  const stackStyles: IStackStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      height: '100%',
      width: '100%',
    },
  };

  const stackItemStyles: IStackItemStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      display: 'flex',
      justifyContent: 'center',
    },
  };

  const iconClass = mergeStyles({
    fontSize: 48,
    height: 48,
    width: 48,
  });

  return (
    <Stack horizontal styles={stackStyles}>
      <Stack.Item styles={stackItemStyles}>
        <FontIcon iconName="MediaIndustryIcon" className={iconClass} />
      </Stack.Item>
      <Stack.Item>
        <Stack styles={stackStyles}>
          <Stack.Item>
            <p>{productName}</p>
          </Stack.Item>
          <Stack.Item>
            <p>control buttons</p>
          </Stack.Item>
        </Stack>
      </Stack.Item>
    </Stack>
  );
};

export default Working1581;
