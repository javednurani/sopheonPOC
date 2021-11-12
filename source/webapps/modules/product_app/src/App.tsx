import { Label, registerIcons } from '@fluentui/react';
import { AppProps } from '@sopheon/shell-api';
import React, { useEffect } from 'react';

import { AppDispatchProps, AppStateProps } from './AppContainer';
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
import OnboardingInfo from './onboardingInfo';

export type Props = AppProps<AppStateProps, AppDispatchProps>;

const svgIconStyle: React.CSSProperties = {
  width: '20px',
  height: '20px',
  overflow: 'visible',
};
registerIcons({
  icons: {
    AgIndustryIcon: <AgIndustry style={svgIconStyle} />,
    AeroIndustryIcon: <AeroIndustry style={svgIconStyle} />,
    AutoIndustryIcon: <AutoIndustry style={svgIconStyle} />,
    ConstREIndustryIcon: <ConstREIndustry style={svgIconStyle} />,
    ConsumerIndustryIcon: <ConsumerIndustry style={svgIconStyle} />,
    EduIndustryIcon: <EdIndustry style={svgIconStyle} />,
    EnergyIndustryIcon: <EnergyIndustry style={svgIconStyle} />,
    FinIndustryIcon: <FinIndustry style={svgIconStyle} />,
    GovtIndustryIcon: <GovtIndustry style={svgIconStyle} />,
    HealthIndustryIcon: <HealthIndustry style={svgIconStyle} />,
    HospIndustryIcon: <HospIndustry style={svgIconStyle} />,
    IndusIndustryIcon: <IndusIndustry style={svgIconStyle} />,
    MediaIndustryIcon: <MediaIndustry style={svgIconStyle} />,
    MemberIndustryIcon: <MemberIndustry style={svgIconStyle} />,
    ServicesIndustryIcon: <ServicesIndustry style={svgIconStyle} />,
    TechIndustryIcon: <TechIndustry style={svgIconStyle} />,
    TeleIndustryIcon: <TeleIndustry style={svgIconStyle} />,
    TransIndustryIcon: <TransIndustry style={svgIconStyle} />,
  },
});

const App: React.FunctionComponent<Props> = ({ currentStep, createProduct, updateProduct, environmentKey, getAccessToken, accessToken }: Props) => {
  useEffect(() => {
    // trigger Shell action to store access token, freshly acquired from MSAL, in Redux state
    getAccessToken();
  }, []);

  return (
    <div>
      <Label>
        <OnboardingInfo
          currentStep={currentStep}
          createProduct={createProduct}
          updateProduct={updateProduct}
          environmentKey={environmentKey}
          accessToken={accessToken}
        />
      </Label>
    </div>
  );
};
export default App;
