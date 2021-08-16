import { StringDict } from '@azure/msal-common';
import { useMsal } from '@azure/msal-react';
import React, { FunctionComponent } from 'react';

import { azureSettings } from '../settings/azureSettings';
import AuthLanding from './AuthLanding';

const Signup: FunctionComponent = () => {
  const { instance, accounts } = useMsal();

  // if user is already logged in, they are trying to create a new account
  // log them out and clear cookies
  if (accounts.length > 0) {
    instance.logout();
  }

  // TODO: when multiple queryparameters are used, may be worth extracting this object builder to an authHelper
  const queryParams: StringDict = {
    mode: azureSettings.AD_B2C_Sopheon_Mode_Signup,
  };

  return <AuthLanding queryParams={queryParams} spinnerMessageResourceKey={'authlanding.signupspinner'} />;
};

export default Signup;
