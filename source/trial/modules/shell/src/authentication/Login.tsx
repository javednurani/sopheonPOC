import React, { FunctionComponent } from 'react';

import azureSettings from '../azureSettings';
import AuthLanding from './AuthLanding';

const Login: FunctionComponent = () => {
  const adB2cPolicyName: string = azureSettings.AD_B2C_SignUpSignIn_Policy;
  const spinnerMessageResourceKey = 'authlanding.loginspinner';
  return <AuthLanding adB2cPolicyName={adB2cPolicyName} spinnerMessageResourceKey={spinnerMessageResourceKey} />;
};

export default Login;
