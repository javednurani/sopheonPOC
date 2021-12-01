import {
  FontIcon,
  IFontStyles,
  IStackItemStyles,
  IStackStyles,
  ITextProps,
  ITextStyles,
  mergeStyles,
  registerIcons,
  Stack,
  Text,
} from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React from 'react';

import { ReactComponent as FinIndustry } from './images/industryico_Fin.svg';

export interface IProductSectionProps {
  productName: string;
}

const ProductSection: React.FunctionComponent<IProductSectionProps> = ({ productName }: IProductSectionProps) => {
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
    <Stack horizontal styles={stackStyles}>
      <Stack.Item styles={stackItemStyles}>
        <FontIcon iconName="HardcodedProductIcon" className={productIconClass} />
      </Stack.Item>
      <Stack.Item>
        <Stack styles={stackStyles}>
          <Stack.Item>
            <Text variant={'xLarge'}>{productName}</Text>
          </Stack.Item>
          <Stack.Item>
            <Stack horizontal styles={stackStyles}>
              <Stack.Item>
                <FontIcon iconName="Share" className={controlButtonIconClass} />
                <Text>Share</Text>
              </Stack.Item>
              <Stack.Item>
                <FontIcon iconName="Edit" className={controlButtonIconClass} />
                <Text>Edit</Text>
              </Stack.Item>
            </Stack>
          </Stack.Item>
        </Stack>
      </Stack.Item>
    </Stack>
  );
};

export default ProductSection;
