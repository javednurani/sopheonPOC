import React, { FunctionComponent } from 'react';

import { azureSettings } from '../azureSettings';
import AuthLanding from './AuthLanding';

const Signup: FunctionComponent = () => (
  <AuthLanding adB2cPolicyName={azureSettings.AD_B2C_SignUp_Policy} spinnerMessageResourceKey={'authlanding.signupspinner'} />
);

export default Signup;
