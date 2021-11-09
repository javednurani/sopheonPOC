import { Dropdown, IDropdownOption, IDropdownStyles, Label, PrimaryButton, ProgressIndicator, Stack, TextField } from '@fluentui/react';
import React from 'react';
import { FormattedMessage, useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';

export type OnboardingInfoProps = AppStateProps & AppDispatchProps;

const headerStyle: React.CSSProperties = {
  marginTop: '20px',
  marginBottom: '20px',
  fontSize: '40px',
};

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
    { key: 'industryoption.advertising', text: formatMessage({ id: 'industryoption.advertising' }) },
    { key: 'industryoption.agricuture', text: formatMessage({ id: 'industryoption.agricuture' }) },
    { key: 'industryoption.construction', text: formatMessage({ id: 'industryoption.construction' }) },
    { key: 'industryoption.eduhigher', text: formatMessage({ id: 'industryoption.eduhigher' }) },
    { key: 'industryoption.eduk12', text: formatMessage({ id: 'industryoption.eduk12' }) },
    { key: 'industryoption.energy', text: formatMessage({ id: 'industryoption.energy' }) },
    { key: 'industryoption.financialservices', text: formatMessage({ id: 'industryoption.financialservices' }) },
    { key: 'industryoption.govfederal', text: formatMessage({ id: 'industryoption.govfederal' }) },
    { key: 'industryoption.govlocal', text: formatMessage({ id: 'industryoption.govlocal' }) },
    { key: 'industryoption.govmilitary', text: formatMessage({ id: 'industryoption.govmilitary' }) },
    { key: 'industryoption.govstate', text: formatMessage({ id: 'industryoption.govstate' }) },
    { key: 'industryoption.healthcare', text: formatMessage({ id: 'industryoption.healthcare' }) },
    { key: 'industryoption.insurance', text: formatMessage({ id: 'industryoption.insurance' }) },
    { key: 'industryoption.manuaero', text: formatMessage({ id: 'industryoption.manuaero' }) },
    { key: 'industryoption.manuauto', text: formatMessage({ id: 'industryoption.manuauto' }) },
    { key: 'industryoption.manuconsumergoods', text: formatMessage({ id: 'industryoption.manuconsumergoods' }) },
    { key: 'industryoption.manuindustrial', text: formatMessage({ id: 'industryoption.manuindustrial' }) },
    { key: 'industryoption.entertainment', text: formatMessage({ id: 'industryoption.entertainment' }) },
    { key: 'industryoption.membershiporg', text: formatMessage({ id: 'industryoption.membershiporg' }) },
    { key: 'industryoption.nonprofit', text: formatMessage({ id: 'industryoption.nonprofit' }) },
    { key: 'industryoption.pharma', text: formatMessage({ id: 'industryoption.pharma' }) },
    { key: 'industryoption.protechservices', text: formatMessage({ id: 'industryoption.protechservices' }) },
    { key: 'industryoption.realestate', text: formatMessage({ id: 'industryoption.realestate' }) },
    { key: 'industryoption.retail', text: formatMessage({ id: 'industryoption.retail' }) },
    { key: 'industryoption.techhardware', text: formatMessage({ id: 'industryoption.techhardware' }) },
    { key: 'industryoption.techsoftware', text: formatMessage({ id: 'industryoption.techsoftware' }) },
    { key: 'industryoption.telecom', text: formatMessage({ id: 'industryoption.telecom' }) },
    { key: 'industryoption.transportation', text: formatMessage({ id: 'industryoption.transportation' }) },
    { key: 'industryoption.travel', text: formatMessage({ id: 'industryoption.travel' }) },
    { key: 'industryoption.utilities', text: formatMessage({ id: 'industryoption.utilities' }) },
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
          <Stack.Item>
            <Label style={headerStyle}>{formatMessage({ id: 'onboarding.setupYourGoals' })}</Label>
          </Stack.Item>
          <Stack.Item>
            <TextField label={formatMessage({ id: 'onboarding.productgoal' })} maxLength={300} multiline rows={4} />
          </Stack.Item>
          <Stack.Item>
            <TextField label={formatMessage({ id: 'onboarding.productKpi' })} maxLength={60} />
          </Stack.Item>
          <Stack.Item>
            <PrimaryButton
              text={formatMessage({ id: 'onboarding.getstarted' })}
              aria-label={formatMessage({ id: 'onboarding.getstarted' })}
              onClick={() => nextStep()}
            />
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
