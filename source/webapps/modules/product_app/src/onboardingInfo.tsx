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
