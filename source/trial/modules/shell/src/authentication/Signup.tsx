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

  return <AuthLanding adB2cPolicyName={azureSettings.AD_B2C_SignUp_Policy} spinnerMessageResourceKey={'authlanding.signupspinner'} />;
};

export default Signup;
