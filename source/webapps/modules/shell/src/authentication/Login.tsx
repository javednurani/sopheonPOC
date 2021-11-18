import React, { FunctionComponent } from 'react';

import { azureSettings } from '../settings/azureSettings';
import AuthLanding from './AuthLanding';

const Login: FunctionComponent = () => (
  <AuthLanding adB2cPolicyName={azureSettings.AD_B2C_SignUpSignIn_Policy} spinnerMessageResourceKey={'authlanding.loginspinner'} />
);

export default Login;
