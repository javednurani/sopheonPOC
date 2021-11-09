import {
  Dropdown,
  FontSizes,
  Icon,
  IDropdownOption,
  IDropdownStyles,
  IStackStyles,
  IStackTokens,
  ITextFieldStyles,
  Label,
  PrimaryButton,
  ProgressIndicator,
  Stack,
  TextField,
} from '@fluentui/react';
import React from 'react';
import { FormattedMessage, useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';

export type OnboardingInfoProps = AppStateProps & AppDispatchProps;

const OnboardingInfo: React.FunctionComponent<OnboardingInfoProps> = ({ currentStep, nextStep }: OnboardingInfoProps) => {
  const { formatMessage } = useIntl();
  const headerStyle: React.CSSProperties = {
    fontSize: FontSizes.size42,
    marginBottom: '2vh',
  };
  const fieldWidth: number = 300;

  const stackTokens: IStackTokens = { childrenGap: 15 };
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
  const onRenderOption = (option: IDropdownOption): JSX.Element => {
    const svgIconStyle: React.CSSProperties = {
      marginRight: 8,
    };
    return (
      <div>
        {option.data && option.data.icon && <Icon style={svgIconStyle} iconName={option.data.icon} aria-hidden="true" title={option.data.icon} />}
        <span>{option.text}</span>
      </div>
    );
  };
  switch (currentStep) {
    case 2:
      return (
        <>
          <Stack className="step2" horizontalAlign="center" tokens={stackTokens}>
            <Stack.Item>
              <Label style={headerStyle}>{formatMessage({ id: 'onboarding.setupproduct' })}</Label>
            </Stack.Item>
            <Stack.Item>
              <TextField
                label={formatMessage({ id: 'onboarding.yourproductname' })}
                aria-label={formatMessage({ id: 'onboarding.yourproductname' })}
                styles={textFieldStyles}
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
                multiSelect
                required
              />
            </Stack.Item>
            <Stack.Item>
              <PrimaryButton
                text={currentStep === 2 ? formatMessage({ id: 'continue' }) : formatMessage({ id: 'getStarted' })}
                aria-label={currentStep === 2 ? formatMessage({ id: 'continue' }) : formatMessage({ id: 'getStarted' })}
                onClick={() => nextStep()}
                style={buttonStyles}
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
        </>
      );
    case 3:
      return (
        <Stack className="step3" horizontalAlign="center">
          <Stack.Item>
            <FormattedMessage id={'step3'} />
          </Stack.Item>
        </Stack>
      );
    case 4:
      return (
        <Stack horizontalAlign="center">
          <h1>To Do: send to home</h1>
        </Stack>
      );
    default:
      return (
        <Stack className="badStep" horizontalAlign="center">
          <FormattedMessage id="error.erroroccurred" />
        </Stack>
      );
  }
};

export default OnboardingInfo;
