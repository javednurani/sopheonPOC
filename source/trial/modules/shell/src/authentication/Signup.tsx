import { useMsal } from '@azure/msal-react';
import React, { FunctionComponent } from 'react';

import azureSettings from '../azureSettings';
import AuthLanding from './AuthLanding';

const Signup: FunctionComponent = () => {
  const { instance, accounts } = useMsal();
  const adB2cPolicyName: string = azureSettings.AD_B2C_SignUp_Policy;
  const spinnerMessageResourceKey = 'authlanding.signupspinner';

  // if user is already logged in, they are trying to create a new account
  // log them out and clear cookies
  if (accounts.length > 0) {
    instance.logout();
  }

  return <AuthLanding adB2cPolicyName={adB2cPolicyName} spinnerMessageResourceKey={spinnerMessageResourceKey} />;
};

export default Signup;
