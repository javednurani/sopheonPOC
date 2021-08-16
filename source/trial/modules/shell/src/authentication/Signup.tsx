import { StringDict } from '@azure/msal-common';
import { useMsal } from '@azure/msal-react';
import React, { FunctionComponent } from 'react';

import AuthLanding from './AuthLanding';

const Signup: FunctionComponent = () => {
  const { instance, accounts } = useMsal();

  // if user is already logged in, they are trying to create a new account
  // log them out and clear cookies
  if (accounts.length > 0) {
    instance.logout();
  }

  const queryParams: StringDict = {
    mode: 'signup',
  };

  return <AuthLanding queryParams={queryParams} spinnerMessageResourceKey={'authlanding.signupspinner'} />;
};

export default Signup;
