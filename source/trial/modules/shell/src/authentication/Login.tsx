import { StringDict } from '@azure/msal-common';
import React, { FunctionComponent } from 'react';

import AuthLanding from './AuthLanding';

const Login: FunctionComponent = () => {
  const queryParams: StringDict = {
    mode: 'login',
  };

  return <AuthLanding queryParams={queryParams} spinnerMessageResourceKey={'authlanding.loginspinner'} />;
};

export default Login;
