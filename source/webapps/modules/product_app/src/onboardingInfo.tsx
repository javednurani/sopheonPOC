import { Dropdown, IDropdownOption, IDropdownStyles, Label, PrimaryButton, ProgressIndicator, Stack, TextField } from '@fluentui/react';
import React from 'react';
import { FormattedMessage, useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';

export type OnboardingInfoProps = AppStateProps & AppDispatchProps;

const OnboardingInfo: React.FunctionComponent<OnboardingInfoProps> = ({ currentStep, nextStep }: OnboardingInfoProps) => {
  const { formatMessage } = useIntl();
  const headerStyle: React.CSSProperties = {
    // TODO: Use header styles from cloud-1583 at merge
  };
  const progressBarStyles: React.CSSProperties = {
    padding: '0vh 25vw 0vh 25vw',
  };
  const dropdownStyles: Partial<IDropdownStyles> = {
    dropdown: { width: 250 },
  };

  const industryOptions: IDropdownOption[] = [
    { key: 'industryoption.advertising', text: 'Advertising' },
    { key: 'industryoption.agricuture', text: 'Agriculture & Forestry' },
    { key: 'industryoption.construction', text: 'Construction' },
    { key: 'industryoption.eduhigher', text: 'Education - Higher Ed' },
    { key: 'industryoption.eduk12', text: 'Education - K12' },
    { key: 'industryoption.energy', text: 'Energy, Mining, Oil & Gas' },
    { key: 'industryoption.financialservices', text: 'Financial Services' },
    { key: 'industryoption.govfederal', text: 'Government - Federal' },
    { key: 'industryoption.govlocal', text: 'Government - Local' },
    { key: 'industryoption.govmilitary', text: 'Government - Military' },
    { key: 'industryoption.govstate', text: 'Government - State' },
    { key: 'industryoption.healthcare', text: 'Health Care' },
    { key: 'industryoption.insurance', text: 'Insurance' },
    { key: 'industryoption.manuaero', text: 'Manufacturing - Aerospace' },
    { key: 'industryoption.manuauto', text: 'Manufacturing - Automotive' },
    { key: 'industryoption.manuconsumergoods', text: 'Manufacturing - Consumer Goods' },
    { key: 'industryoption.manuindustrial', text: 'Manufacturing - Industrial' },
    { key: 'industryoption.entertainment', text: 'Media & Entertainment' },
    { key: 'industryoption.membershiporg', text: 'Membership Organizations' },
    { key: 'industryoption.nonprofit', text: 'Non-Profit' },
    { key: 'industryoption.pharma', text: 'Pharmaceuticals & Biotech' },
    { key: 'industryoption.protechservices', text: 'Professional & Technical Services' },
    { key: 'industryoption.realestate', text: 'Real Estate, Rental & Leasing' },
    { key: 'industryoption.retail', text: 'Retail' },
    { key: 'industryoption.techhardware', text: 'Technology Hardware' },
    { key: 'industryoption.techsoftware', text: 'Technology Software & Services' },
    { key: 'industryoption.telecom', text: 'Telecommunications' },
    { key: 'industryoption.transportation', text: 'Transportation & Warehousing' },
    { key: 'industryoption.travel', text: 'Travel, Leisure & Hospitality' },
    { key: 'industryoption.utilities', text: 'Utilities' },
  ];

  switch (currentStep) {
    case 2:
      return (
        <Stack className="step2" horizontalAlign="center">
          <Stack.Item>
            <Label style={headerStyle}>{formatMessage({ id: 'onboarding.setupproduct' })}</Label>
          </Stack.Item>
          <Stack.Item>
            <TextField
              label={formatMessage({ id: 'onboarding.yourproductname' })}
              aria-label={formatMessage({ id: 'onboarding.yourproductname' })}
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
              multiSelect
              required
            />
          </Stack.Item>
          <Stack.Item align={'auto'} style={progressBarStyles}>
            <ProgressIndicator
              label={formatMessage({ id: 'onboarding.step2of3' })}
              description={formatMessage({ id: 'onboarding.nextGoals' })}
              ariaValueText={formatMessage({ id: 'onboarding.step2of3' })}
              percentComplete={0.67}
              barHeight={12}
            />
          </Stack.Item>
        </Stack>
      );
    case 3:
      return (
        <Stack className="step3" horizontalAlign="center">
          <FormattedMessage id={'step3'} />
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
