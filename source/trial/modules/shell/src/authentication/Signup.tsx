import React, { FunctionComponent } from 'react';

import azureSettings from '../azureSettings';
import AuthLanding from './AuthLanding';

const Signup: FunctionComponent = () => {
  const adB2cPolicyName: string = azureSettings.AD_B2C_SignUp_Policy;
  const spinnerMessageResourceKey = 'authlanding.signupspinner';
  return <AuthLanding adB2cPolicyName={adB2cPolicyName} spinnerMessageResourceKey={spinnerMessageResourceKey} />;
};

export default Signup;
