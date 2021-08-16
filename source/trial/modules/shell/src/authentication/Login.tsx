import { StringDict } from '@azure/msal-common';
import React, { FunctionComponent } from 'react';

import { azureSettings } from '../settings/azureSettings';
import AuthLanding from './AuthLanding';

const Login: FunctionComponent = () => {
  // TODO: when multiple queryparameters are used, may be worth extracting this object builder to an authHelper
  const queryParams: StringDict = {
    mode: azureSettings.AD_B2C_Sopheon_Mode_Login,
  };

  return <AuthLanding queryParams={queryParams} spinnerMessageResourceKey={'authlanding.loginspinner'} />;
};

export default Login;
