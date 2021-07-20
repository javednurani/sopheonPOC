import React, { FunctionComponent } from 'react';

import azureSettings from '../azureSettings';
import NewUserLanding from './NewUserLanding';

const Signup: FunctionComponent = () => {
  const adB2cPolicyName: string = azureSettings.AD_B2C_SignUp_Policy;
  const spinnerMessageResourceKey = 'newuserlanding.signupspinner';
  return <NewUserLanding adB2cPolicyName={adB2cPolicyName} spinnerMessageResourceKey={spinnerMessageResourceKey} />;
};

export default Signup;
