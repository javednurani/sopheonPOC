import {
  Dropdown,
  FontSizes,
  IDropdownOption,
  IDropdownStyles,
  IStackTokens,
  ITextFieldStyles,
  Label,
  PrimaryButton,
  ProgressIndicator,
  Stack,
  TextField,
} from '@fluentui/react';
import React, { useEffect, useState } from 'react';
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
    { key: 1, text: formatMessage({ id: 'industryoption.advertising' }) },
    { key: 2, text: formatMessage({ id: 'industryoption.agricuture' }) },
    { key: 3, text: formatMessage({ id: 'industryoption.construction' }) },
    { key: 4, text: formatMessage({ id: 'industryoption.eduhigher' }) },
    { key: 5, text: formatMessage({ id: 'industryoption.eduk12' }) },
    { key: 6, text: formatMessage({ id: 'industryoption.energy' }) },
    { key: 7, text: formatMessage({ id: 'industryoption.financialservices' }) },
    { key: 8, text: formatMessage({ id: 'industryoption.govfederal' }) },
    { key: 9, text: formatMessage({ id: 'industryoption.govlocal' }) },
    { key: 10, text: formatMessage({ id: 'industryoption.govmilitary' }) },
    { key: 11, text: formatMessage({ id: 'industryoption.govstate' }) },
    { key: 12, text: formatMessage({ id: 'industryoption.healthcare' }) },
    { key: 13, text: formatMessage({ id: 'industryoption.insurance' }) },
    { key: 14, text: formatMessage({ id: 'industryoption.manuaero' }) },
    { key: 15, text: formatMessage({ id: 'industryoption.manuauto' }) },
    { key: 16, text: formatMessage({ id: 'industryoption.manuconsumergoods' }) },
    { key: 17, text: formatMessage({ id: 'industryoption.manuindustrial' }) },
    { key: 18, text: formatMessage({ id: 'industryoption.entertainment' }) },
    { key: 19, text: formatMessage({ id: 'industryoption.membershiporg' }) },
    { key: 20, text: formatMessage({ id: 'industryoption.nonprofit' }) },
    { key: 21, text: formatMessage({ id: 'industryoption.pharma' }) },
    { key: 22, text: formatMessage({ id: 'industryoption.protechservices' }) },
    { key: 23, text: formatMessage({ id: 'industryoption.realestate' }) },
    { key: 24, text: formatMessage({ id: 'industryoption.retail' }) },
    { key: 25, text: formatMessage({ id: 'industryoption.techhardware' }) },
    { key: 26, text: formatMessage({ id: 'industryoption.techsoftware' }) },
    { key: 27, text: formatMessage({ id: 'industryoption.telecom' }) },
    { key: 28, text: formatMessage({ id: 'industryoption.transportation' }) },
    { key: 29, text: formatMessage({ id: 'industryoption.travel' }) },
    { key: 30, text: formatMessage({ id: 'industryoption.utilities' }) },
  ];

  const [productName, setProductName] = useState('');
  const [industryKeys, setIndustryKeys] = useState<number[]>([]);
  const [continueDisabled, setContinueDisabled] = useState(true);

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
        setIndustryKeys(industryKeys.filter(k => k != (option.key as number)));
      }
    }
  };

  useEffect(() => {
    console.log(productName, industryKeys);
    setContinueDisabled(productName.length == 0 || industryKeys.length == 0);
  }, [productName, industryKeys]);

  switch (currentStep) {
    case 2:
      return (
        <Stack className="step2" horizontalAlign="center" tokens={stackTokens}>
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
              onChange={handleIndustryDropdownChange}
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
