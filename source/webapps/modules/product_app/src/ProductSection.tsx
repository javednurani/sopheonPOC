import { FontIcon, IStackItemStyles, IStackStyles, IStackTokens, mergeStyles, registerIcons, Stack, Text } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';
import { useIntl } from 'react-intl';

import { ReactComponent as FinIndustry } from './images/industryico_Fin.svg';

export interface IProductSectionProps {
  productName: string;
}

const ProductSection: React.FunctionComponent<IProductSectionProps> = ({ productName }: IProductSectionProps) => {
  const theme = useTheme();
  const { formatMessage } = useIntl();

  const stackStyles: IStackStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      height: '100%',
      width: '100%',
    },
  };

  const mainStackTokens: IStackTokens = {
    childrenGap: 20,
    padding: 10,
  };

  const nestedStackTokens: IStackTokens = {
    childrenGap: 10,
  };

  const productIconStackItemStyles: IStackItemStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      display: 'flex',
      justifyContent: 'center',
    },
  };

  const productNameStackItemStyles: IStackItemStyles = {
    root: {
      background: theme.semanticColors.bodyBackground, // TODO: why needed?
      display: 'flex',
      justifyContent: 'left',
    },
  };

  const controlButtonIconStackItemStyles: IStackItemStyles = {
    root: {
      marginRight: '8px',
    },
  };

  const shareControlLabelStackItemStyles: IStackItemStyles = {
    root: {
      marginRight: '16px',
    },
  };

  const productIconClass = mergeStyles({
    fontSize: 48,
    height: 48,
    width: 48,
  });

  const controlButtonIconClass = mergeStyles({
    fontSize: 16,
    height: 16,
    width: 16,
  });

  // BEGIN Cloud-2035 stub (hardcoded product Icon)
  const hardcodedProductIconStyles: React.CSSProperties = {
    width: '48px',
    height: '48px',
    overflow: 'visible',
  };

  registerIcons({
    icons: {
      HardcodedProductIcon: <FinIndustry style={hardcodedProductIconStyles} />,
    },
  });
  // END Cloud-2035 stub (hardcoded product Icon)

  return (
    <Stack horizontal styles={stackStyles} tokens={mainStackTokens}>
      <Stack.Item styles={productIconStackItemStyles}>
        <FontIcon iconName="HardcodedProductIcon" className={productIconClass} />
      </Stack.Item>
      <Stack.Item>
        <Stack styles={stackStyles} tokens={nestedStackTokens}>
          <Stack.Item styles={productNameStackItemStyles}>
            <Text variant={'xLarge'}>{productName}</Text>
          </Stack.Item>
          <Stack.Item>
            <Stack horizontal styles={stackStyles}>
              <Stack.Item styles={controlButtonIconStackItemStyles}>
                <FontIcon iconName="Share" className={controlButtonIconClass} />
              </Stack.Item>
              <Stack.Item styles={shareControlLabelStackItemStyles}>
                <Text>{formatMessage({ id: 'share' })}</Text>
              </Stack.Item>
              <Stack.Item styles={controlButtonIconStackItemStyles}>
                <FontIcon iconName="Edit" className={controlButtonIconClass} />
              </Stack.Item>
              <Stack.Item>
                <Text>{formatMessage({ id: 'edit' })}</Text>
              </Stack.Item>
            </Stack>
          </Stack.Item>
        </Stack>
      </Stack.Item>
    </Stack>
  );
};

export default ProductSection;
