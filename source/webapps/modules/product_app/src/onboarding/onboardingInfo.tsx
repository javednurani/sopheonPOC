import {
  Dropdown,
  FontSizes,
  Icon,
  IDropdownOption,
  IDropdownStyles,
  IStackTokens,
  ITextFieldStyles,
  Label,
  PrimaryButton,
  ProgressIndicator,
  registerIcons,
  Stack,
  TextField,
} from '@fluentui/react';
import React, { useEffect, useState } from 'react';
import { FormattedMessage, useIntl } from 'react-intl';

import { industries } from '../data/industries';
import { CreateProductAction, UpdateProductAction } from '../product/productReducer';
import SopheonLogo from '../SopheonLogo';
import { Attributes, CreateProductModel, KeyPerformanceIndicatorDto, PatchOperation, Product, ProductPostDto, UpdateProductModel } from '../types';
import { ReactComponent as AeroIndustry } from './../images/industryico_Aero.svg';
import { ReactComponent as AgIndustry } from './../images/industryico_Ag.svg';
import { ReactComponent as AutoIndustry } from './../images/industryico_Auto.svg';
import { ReactComponent as ConstREIndustry } from './../images/industryico_ConstRE.svg';
import { ReactComponent as ConsumerIndustry } from './../images/industryico_Consumer.svg';
import { ReactComponent as EdIndustry } from './../images/industryico_Ed.svg';
import { ReactComponent as EnergyIndustry } from './../images/industryico_Energy.svg';
import { ReactComponent as FinIndustry } from './../images/industryico_Fin.svg';
import { ReactComponent as GovtIndustry } from './../images/industryico_Govt.svg';
import { ReactComponent as HealthIndustry } from './../images/industryico_Health.svg';
import { ReactComponent as HospIndustry } from './../images/industryico_Hosp.svg';
import { ReactComponent as IndusIndustry } from './../images/industryico_Indus.svg';
import { ReactComponent as MediaIndustry } from './../images/industryico_Media.svg';
import { ReactComponent as MemberIndustry } from './../images/industryico_Member.svg';
import { ReactComponent as ServicesIndustry } from './../images/industryico_Services.svg';
import { ReactComponent as TechIndustry } from './../images/industryico_Tech.svg';
import { ReactComponent as TeleIndustry } from './../images/industryico_Tele.svg';
import { ReactComponent as TransIndustry } from './../images/industryico_Trans.svg';
import { NextStepAction } from './onboardingReducer';

export interface IOnboardingInfoProps {
  currentStep: number;
  nextStep: () => NextStepAction;
  createProduct: (product: CreateProductModel) => CreateProductAction;
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
  environmentKey: string;
  accessToken: string;
  products: Product[];
}

const svgIndustryIconStyleSmall: React.CSSProperties = {
  width: '20px',
  height: '20px',
  overflow: 'visible',
};

// Industry Icons are currently used to stub out Product Section Product Icon. Appending 'Small' supports 2x registerIcons with 2x styles
registerIcons({
  icons: {
    AgIndustryIconSmall: <AgIndustry style={svgIndustryIconStyleSmall} />,
    AeroIndustryIconSmall: <AeroIndustry style={svgIndustryIconStyleSmall} />,
    AutoIndustryIconSmall: <AutoIndustry style={svgIndustryIconStyleSmall} />,
    ConstREIndustryIconSmall: <ConstREIndustry style={svgIndustryIconStyleSmall} />,
    ConsumerIndustryIconSmall: <ConsumerIndustry style={svgIndustryIconStyleSmall} />,
    EduIndustryIconSmall: <EdIndustry style={svgIndustryIconStyleSmall} />,
    EnergyIndustryIconSmall: <EnergyIndustry style={svgIndustryIconStyleSmall} />,
    FinIndustryIconSmall: <FinIndustry style={svgIndustryIconStyleSmall} />,
    GovtIndustryIconSmall: <GovtIndustry style={svgIndustryIconStyleSmall} />,
    HealthIndustryIconSmall: <HealthIndustry style={svgIndustryIconStyleSmall} />,
    HospIndustryIconSmall: <HospIndustry style={svgIndustryIconStyleSmall} />,
    IndusIndustryIconSmall: <IndusIndustry style={svgIndustryIconStyleSmall} />,
    MediaIndustryIconSmall: <MediaIndustry style={svgIndustryIconStyleSmall} />,
    MemberIndustryIconSmall: <MemberIndustry style={svgIndustryIconStyleSmall} />,
    ServicesIndustryIconSmall: <ServicesIndustry style={svgIndustryIconStyleSmall} />,
    TechIndustryIconSmall: <TechIndustry style={svgIndustryIconStyleSmall} />,
    TeleIndustryIconSmall: <TeleIndustry style={svgIndustryIconStyleSmall} />,
    TransIndustryIconSmall: <TransIndustry style={svgIndustryIconStyleSmall} />,
  },
});

const OnboardingInfo: React.FunctionComponent<IOnboardingInfoProps> = ({
  currentStep,
  nextStep,
  createProduct,
  updateProduct,
  environmentKey,
  accessToken,
  products,
}: IOnboardingInfoProps) => {
  const { formatMessage } = useIntl();

  // Onboarding Page 2
  const [productName, setProductName] = useState('');
  const [industryKeys, setIndustryKeys] = useState<number[]>([]);
  const [continueDisabled, setContinueDisabled] = useState(true);

  // Onboarding Page 3
  const [goal, setGoal] = useState('');
  const [kpi, setKpi] = useState('');

  useEffect(() => {
    setContinueDisabled(productName.length === 0 || industryKeys.length === 0);
  }, [productName, industryKeys]);

  const headerStyle: React.CSSProperties = {
    fontSize: FontSizes.size42,
    marginBottom: '2vh',
  };
  const fieldWidth = 300;

  const stackTokens: IStackTokens = { childrenGap: 15 };
  const sopheonLogoStyle: React.CSSProperties = {
    marginTop: '10vh',
    width: 200,
  };
  const buttonStyles: React.CSSProperties = {
    marginTop: '6vh',
  };
  const progressBarStyles: React.CSSProperties = {
    marginTop: '10vh',
    width: fieldWidth,
  };
  const textFieldStyles: Partial<ITextFieldStyles> = {
    root: {
      width: fieldWidth,
    },
  };
  const dropdownStyles: Partial<IDropdownStyles> = {
    root: {
      width: fieldWidth,
    },
  };

  const industryOptions: IDropdownOption[] = industries.map(ind => ({
    key: ind.key,
    text: formatMessage({ id: ind.resourceKey }),
    data: {
      // Industry Icons are currently used to stub out Product Section Product Icon. Appending 'Small' supports 2x registerIcons with 2x styles
      icon: `${ind.iconName}Small`,
    },
  }));

  const onRenderOption = (option: IDropdownOption | undefined): JSX.Element => {
    const svgIconStyle: React.CSSProperties = {
      marginRight: 8,
    };
    if (option) {
      return (
        <div>
          {option.data && option.data.icon && <Icon style={svgIconStyle} iconName={option.data.icon} aria-hidden="true" title={option.data.icon} />}
          <span>{option.text}</span>
        </div>
      );
    }
    return <></>;
  };

  const handleProductNameChange = (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string | undefined): void => {
    setProductName(newValue || '');
  };

  const handleIndustryDropdownChange = (
    event: React.FormEvent<HTMLDivElement>,
    option?: IDropdownOption | undefined,
    index?: number | undefined
  ): void => {
    if (option) {
      if (industryKeys.indexOf(option.key as number) < 0) {
        setIndustryKeys([...industryKeys, option.key as number]);
      } else {
        setIndustryKeys(industryKeys.filter(k => k !== (option.key as number)));
      }
    }
  };

  const handleOnboardingContinueClick = () => {
    const productData: ProductPostDto = {
      Name: productName,
      Int32AttributeValues: industryKeys.map(ik => ({
        AttributeId: Attributes.INDUSTRIES,
        Value: ik,
      })),
    };

    const createProductDto: CreateProductModel = {
      Product: productData,
      EnvironmentKey: environmentKey,
      AccessToken: accessToken,
    };

    createProduct(createProductDto);
    nextStep();
  };

  const handleGoalChange = (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string | undefined): void => {
    setGoal(newValue || '');
  };

  const handleKpiChange = (event: React.FormEvent<HTMLInputElement | HTMLTextAreaElement>, newValue?: string | undefined): void => {
    setKpi(newValue || '');
  };

  const handleOnboardingGetStartedClick = () => {
    const kpiAttributes: KeyPerformanceIndicatorDto[] = kpi.split(',').map(k => ({
      attribute: {
        attributeValueTypeId: 3,
        name: k.trim(),
      },
    }));

    const productPatchData: PatchOperation[] = [
      {
        op: 'replace',
        path: '/Goals',
        value: [
          {
            name: goal,
          },
        ],
      },
      {
        op: 'replace',
        path: '/KeyPerformanceIndicators',
        value: kpiAttributes,
      },
    ];

    const updateProductDto: UpdateProductModel = {
      ProductPatchData: productPatchData,
      ProductKey: products[0].key || 'BAD_PRODUCT_KEY',
      EnvironmentKey: environmentKey,
      AccessToken: accessToken,
    };
    updateProduct(updateProductDto);
    nextStep();
  };

  if (currentStep === 2 && products.length === 0) {
    // onboarding 'step 2' (first onboarding page in SPA: Product Name & Industries)
    return (
      <Stack className="step2" horizontalAlign="center" tokens={stackTokens}>
        <Stack.Item>
          <SopheonLogo style={sopheonLogoStyle} />
        </Stack.Item>
        <Stack.Item>
          <Label style={headerStyle}>{formatMessage({ id: 'onboarding.setupproduct' })}</Label>
        </Stack.Item>
        <Stack.Item>
          <TextField
            label={formatMessage({ id: 'onboarding.yourproductname' })}
            aria-label={formatMessage({ id: 'onboarding.yourproductname' })}
            styles={textFieldStyles}
            onChange={handleProductNameChange}
            required
            maxLength={300}
          />
        </Stack.Item>
        <Stack.Item>
          <Dropdown
            label={formatMessage({ id: 'onboarding.industryselection' })}
            placeholder={formatMessage({ id: 'industryoption.default' })}
            options={industryOptions}
            styles={dropdownStyles}
            onRenderOption={onRenderOption}
            onChange={handleIndustryDropdownChange}
            multiSelect
            required
          />
        </Stack.Item>
        <Stack.Item>
          <PrimaryButton
            text={formatMessage({ id: 'continue' })}
            aria-label={formatMessage({ id: 'continue' })}
            onClick={handleOnboardingContinueClick}
            style={buttonStyles}
            disabled={continueDisabled}
          />
        </Stack.Item>
        <Stack.Item style={progressBarStyles}>
          <ProgressIndicator
            label={formatMessage({ id: 'onboarding.step2of3' })}
            description={formatMessage({ id: 'onboarding.nextGoals' })}
            ariaValueText={formatMessage({ id: 'onboarding.step2of3' })}
            percentComplete={0.67}
            barHeight={8}
          />
        </Stack.Item>
      </Stack>
    );
  } else if (currentStep === 3) {
    // onboarding 'step 3' (second onboarding page in SPA: Goal & KPIs)
    return (
      <Stack className="step3" horizontalAlign="center" tokens={stackTokens}>
        <Stack.Item>
          <SopheonLogo style={sopheonLogoStyle} />
        </Stack.Item>
        <Stack.Item>
          <Label style={headerStyle}>{formatMessage({ id: 'onboarding.setupYourGoals' })}</Label>
        </Stack.Item>
        <Stack.Item>
          {/* Wrapped in a div to alter the DOM structure between steps, preventing text carry over bug */}
          <div>
            <TextField
              label={formatMessage({ id: 'onboarding.productgoal' })}
              maxLength={300}
              multiline
              rows={4}
              styles={textFieldStyles}
              resizable={false}
              onChange={handleGoalChange}
            />
          </div>
        </Stack.Item>
        <Stack.Item>
          <TextField label={formatMessage({ id: 'onboarding.productKpi' })} styles={textFieldStyles} onChange={handleKpiChange} maxLength={60} />
        </Stack.Item>
        <Stack.Item>
          <PrimaryButton
            text={formatMessage({ id: 'onboarding.getstarted' })}
            aria-label={formatMessage({ id: 'onboarding.getstarted' })}
            onClick={handleOnboardingGetStartedClick}
            style={buttonStyles}
          />
        </Stack.Item>
        <Stack.Item align={'auto'} style={progressBarStyles}>
          <ProgressIndicator
            label={formatMessage({ id: 'onboarding.step3of3' })}
            description={formatMessage({ id: 'onboarding.done' })}
            ariaValueText={formatMessage({ id: 'onboarding.step3of3' })}
            percentComplete={1}
            barHeight={8}
          />
        </Stack.Item>
      </Stack>
    );
  }

  return (
    <Stack className="badStep" horizontalAlign="center">
      <FormattedMessage id="error.erroroccurred" />
    </Stack>
  );
};

export default OnboardingInfo;
