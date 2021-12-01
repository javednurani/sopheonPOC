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

const svgIndustryIconStyle: React.CSSProperties = {
  width: '20px',
  height: '20px',
  overflow: 'visible',
};

// TODO: is this specific to Onboarding? (As of Cloud-2035 - will need access to registered Icons with a different style 48px width/height)
registerIcons({
  icons: {
    AgIndustryIcon: <AgIndustry style={svgIndustryIconStyle} />,
    AeroIndustryIcon: <AeroIndustry style={svgIndustryIconStyle} />,
    AutoIndustryIcon: <AutoIndustry style={svgIndustryIconStyle} />,
    ConstREIndustryIcon: <ConstREIndustry style={svgIndustryIconStyle} />,
    ConsumerIndustryIcon: <ConsumerIndustry style={svgIndustryIconStyle} />,
    EduIndustryIcon: <EdIndustry style={svgIndustryIconStyle} />,
    EnergyIndustryIcon: <EnergyIndustry style={svgIndustryIconStyle} />,
    FinIndustryIcon: <FinIndustry style={svgIndustryIconStyle} />,
    GovtIndustryIcon: <GovtIndustry style={svgIndustryIconStyle} />,
    HealthIndustryIcon: <HealthIndustry style={svgIndustryIconStyle} />,
    HospIndustryIcon: <HospIndustry style={svgIndustryIconStyle} />,
    IndusIndustryIcon: <IndusIndustry style={svgIndustryIconStyle} />,
    MediaIndustryIcon: <MediaIndustry style={svgIndustryIconStyle} />,
    MemberIndustryIcon: <MemberIndustry style={svgIndustryIconStyle} />,
    ServicesIndustryIcon: <ServicesIndustry style={svgIndustryIconStyle} />,
    TechIndustryIcon: <TechIndustry style={svgIndustryIconStyle} />,
    TeleIndustryIcon: <TeleIndustry style={svgIndustryIconStyle} />,
    TransIndustryIcon: <TransIndustry style={svgIndustryIconStyle} />,
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
      textAlign: 'left',
    },
  };
  const dropdownStyles: Partial<IDropdownStyles> = {
    root: {
      width: fieldWidth,
      textAlign: 'left',
    },
  };

  // TODO Cloud-2035, extract key/resourceId/iconName to an Industries data file
  // build this industryOptions [] with a .map()
  // access Industries data file as needed for Product Image display
  const industryOptions: IDropdownOption[] = [
    { key: 1, text: formatMessage({ id: 'industryoption.advertising' }), data: { icon: 'MediaIndustryIcon' } },
    { key: 2, text: formatMessage({ id: 'industryoption.agricuture' }), data: { icon: 'AgIndustryIcon' } },
    { key: 3, text: formatMessage({ id: 'industryoption.construction' }), data: { icon: 'ConstREIndustryIcon' } },
    { key: 4, text: formatMessage({ id: 'industryoption.eduhigher' }), data: { icon: 'EduIndustryIcon' } },
    { key: 5, text: formatMessage({ id: 'industryoption.eduk12' }), data: { icon: 'EduIndustryIcon' } },
    { key: 6, text: formatMessage({ id: 'industryoption.energy' }), data: { icon: 'EnergyIndustryIcon' } },
    { key: 7, text: formatMessage({ id: 'industryoption.financialservices' }), data: { icon: 'FinIndustryIcon' } },
    { key: 8, text: formatMessage({ id: 'industryoption.govfederal' }), data: { icon: 'GovtIndustryIcon' } },
    { key: 9, text: formatMessage({ id: 'industryoption.govlocal' }), data: { icon: 'GovtIndustryIcon' } },
    { key: 10, text: formatMessage({ id: 'industryoption.govmilitary' }), data: { icon: 'GovtIndustryIcon' } },
    { key: 11, text: formatMessage({ id: 'industryoption.govstate' }), data: { icon: 'GovtIndustryIcon' } },
    { key: 12, text: formatMessage({ id: 'industryoption.healthcare' }), data: { icon: 'HealthIndustryIcon' } },
    { key: 13, text: formatMessage({ id: 'industryoption.insurance' }), data: { icon: 'FinIndustryIcon' } },
    { key: 14, text: formatMessage({ id: 'industryoption.manuaero' }), data: { icon: 'AeroIndustryIcon' } },
    { key: 15, text: formatMessage({ id: 'industryoption.manuauto' }), data: { icon: 'AutoIndustryIcon' } },
    { key: 16, text: formatMessage({ id: 'industryoption.manuconsumergoods' }), data: { icon: 'ConsumerIndustryIcon' } },
    { key: 17, text: formatMessage({ id: 'industryoption.manuindustrial' }), data: { icon: 'IndusIndustryIcon' } },
    { key: 18, text: formatMessage({ id: 'industryoption.entertainment' }), data: { icon: 'MediaIndustryIcon' } },
    { key: 19, text: formatMessage({ id: 'industryoption.membershiporg' }), data: { icon: 'MemberIndustryIcon' } },
    { key: 20, text: formatMessage({ id: 'industryoption.nonprofit' }), data: { icon: 'MemberIndustryIcon' } },
    { key: 21, text: formatMessage({ id: 'industryoption.pharma' }), data: { icon: 'HealthIndustryIcon' } },
    { key: 22, text: formatMessage({ id: 'industryoption.protechservices' }), data: { icon: 'ServicesIndustryIcon' } },
    { key: 23, text: formatMessage({ id: 'industryoption.realestate' }), data: { icon: 'ConstREIndustryIcon' } },
    { key: 24, text: formatMessage({ id: 'industryoption.retail' }), data: { icon: 'ConsumerIndustryIcon' } },
    { key: 25, text: formatMessage({ id: 'industryoption.techhardware' }), data: { icon: 'TechIndustryIcon' } },
    { key: 26, text: formatMessage({ id: 'industryoption.techsoftware' }), data: { icon: 'TechIndustryIcon' } },
    { key: 27, text: formatMessage({ id: 'industryoption.telecom' }), data: { icon: 'TeleIndustryIcon' } },
    { key: 28, text: formatMessage({ id: 'industryoption.transportation' }), data: { icon: 'TransIndustryIcon' } },
    { key: 29, text: formatMessage({ id: 'industryoption.travel' }), data: { icon: 'HospIndustryIcon' } },
    { key: 30, text: formatMessage({ id: 'industryoption.utilities' }), data: { icon: 'TechIndustryIcon' } },
  ];

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
      IntAttributeValues: industryKeys.map(ik => ({
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

  if (!environmentKey) {
    return (
      <Stack horizontalAlign="center">
        <h1>Log In to use the Product App.</h1>
      </Stack>
    );
  }

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
