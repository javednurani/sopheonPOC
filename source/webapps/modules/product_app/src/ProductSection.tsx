import { FontIcon, IStackItemStyles, IStackStyles, IStackTokens, mergeStyles, registerIcons, Stack, Text } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import { darkTheme } from '@sopheon/shared-ui';
import React from 'react';
import { useIntl } from 'react-intl';

import { industries } from './data/industries';
import { ReactComponent as AeroIndustry } from './images/industryico_Aero.svg';
import { ReactComponent as AgIndustry } from './images/industryico_Ag.svg';
import { ReactComponent as AutoIndustry } from './images/industryico_Auto.svg';
import { ReactComponent as ConstREIndustry } from './images/industryico_ConstRE.svg';
import { ReactComponent as ConsumerIndustry } from './images/industryico_Consumer.svg';
import { ReactComponent as EdIndustry } from './images/industryico_Ed.svg';
import { ReactComponent as EnergyIndustry } from './images/industryico_Energy.svg';
import { ReactComponent as FinIndustry } from './images/industryico_Fin.svg';
import { ReactComponent as GovtIndustry } from './images/industryico_Govt.svg';
import { ReactComponent as HealthIndustry } from './images/industryico_Health.svg';
import { ReactComponent as HospIndustry } from './images/industryico_Hosp.svg';
import { ReactComponent as IndusIndustry } from './images/industryico_Indus.svg';
import { ReactComponent as MediaIndustry } from './images/industryico_Media.svg';
import { ReactComponent as MemberIndustry } from './images/industryico_Member.svg';
import { ReactComponent as ServicesIndustry } from './images/industryico_Services.svg';
import { ReactComponent as TechIndustry } from './images/industryico_Tech.svg';
import { ReactComponent as TeleIndustry } from './images/industryico_Tele.svg';
import { ReactComponent as TransIndustry } from './images/industryico_Trans.svg';
import { Product } from './types';

export interface IProductSectionProps {
  product: Product;
}

const ProductSection: React.FunctionComponent<IProductSectionProps> = ({ product }: IProductSectionProps) => {
  const theme = useTheme();
  const { formatMessage } = useIntl();

  const svgIndustryIconStyleLarge: React.CSSProperties = {
    width: '48px',
    height: '48px',
    overflow: 'visible',
    fill: theme.id?.includes(darkTheme.id ?? 'darkTheme') ? '#f0f0f0' : '#898989',
    stroke: theme.id?.includes(darkTheme.id ?? 'darkTheme') ? '#898989' : '#ffffff',
  };

  // Industry Icons are currently used to stub out Product Section Product Icon. Appending 'Large' supports 2x registerIcons with 2x styles
  registerIcons({
    icons: {
      AgIndustryIconLarge: <AgIndustry style={svgIndustryIconStyleLarge} />,
      AeroIndustryIconLarge: <AeroIndustry style={svgIndustryIconStyleLarge} />,
      AutoIndustryIconLarge: <AutoIndustry style={svgIndustryIconStyleLarge} />,
      ConstREIndustryIconLarge: <ConstREIndustry style={svgIndustryIconStyleLarge} />,
      ConsumerIndustryIconLarge: <ConsumerIndustry style={svgIndustryIconStyleLarge} />,
      EduIndustryIconLarge: <EdIndustry style={svgIndustryIconStyleLarge} />,
      EnergyIndustryIconLarge: <EnergyIndustry style={svgIndustryIconStyleLarge} />,
      FinIndustryIconLarge: <FinIndustry style={svgIndustryIconStyleLarge} />,
      GovtIndustryIconLarge: <GovtIndustry style={svgIndustryIconStyleLarge} />,
      HealthIndustryIconLarge: <HealthIndustry style={svgIndustryIconStyleLarge} />,
      HospIndustryIconLarge: <HospIndustry style={svgIndustryIconStyleLarge} />,
      IndusIndustryIconLarge: <IndusIndustry style={svgIndustryIconStyleLarge} />,
      MediaIndustryIconLarge: <MediaIndustry style={svgIndustryIconStyleLarge} />,
      MemberIndustryIconLarge: <MemberIndustry style={svgIndustryIconStyleLarge} />,
      ServicesIndustryIconLarge: <ServicesIndustry style={svgIndustryIconStyleLarge} />,
      TechIndustryIconLarge: <TechIndustry style={svgIndustryIconStyleLarge} />,
      TeleIndustryIconLarge: <TeleIndustry style={svgIndustryIconStyleLarge} />,
      TransIndustryIconLarge: <TransIndustry style={svgIndustryIconStyleLarge} />,
    },
  });

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

  const industryIconName = industries.find(ind => ind.key === product.industries[0])?.iconName;

  return (
    <Stack horizontal styles={stackStyles} tokens={mainStackTokens}>
      <Stack.Item styles={productIconStackItemStyles}>
        <FontIcon iconName={`${industryIconName}Large`} className={productIconClass} />
      </Stack.Item>
      <Stack.Item>
        <Stack styles={stackStyles} tokens={nestedStackTokens}>
          <Stack.Item styles={productNameStackItemStyles}>
            <Text variant={'xLarge'}>{product.name}</Text>
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
