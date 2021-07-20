import React, { FunctionComponent } from 'react';

import azureSettings from '../azureSettings';
import NewUserLanding from './NewUserLanding';

const Login: FunctionComponent = () => {
  const adB2cPolicyName: string = azureSettings.AD_B2C_SignUpSignIn_Policy;
  const spinnerMessageResourceKey = 'newuserlanding.loginspinner';
  return <NewUserLanding adB2cPolicyName={adB2cPolicyName} spinnerMessageResourceKey={spinnerMessageResourceKey} />;
};

export default Login;
