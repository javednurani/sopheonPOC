import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import { DefaultButton, IContextualMenuProps } from '@fluentui/react';
import React, { FunctionComponent } from 'react';

const SignupLoginButton: FunctionComponent = () => {
  const { instance, accounts } = useMsal(); // , accounts, inProgress
  const menuProps: IContextualMenuProps = {
    items: [
      {
        key: 'profile',
        text: 'My Profile',
        iconProps: { iconName: 'EditContact' },
      },
      {
        key: 'signout',
        text: 'Sign Out',
        iconProps: { iconName: 'SignOut' },
      },
    ],
  };

  return (
    <React.Fragment>
      <AuthenticatedTemplate>
        <DefaultButton text={accounts[0] ? accounts[0].name : 'My Profile'} split menuProps={menuProps} />
      </AuthenticatedTemplate>
      <UnauthenticatedTemplate>
        <DefaultButton
          text="Signup/Login"
          onClick={() => instance.loginRedirect()}
          // eslint-disable-next-line max-len
          // href="https://StratusB2CDev.b2clogin.com/StratusB2CDev.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1A_SIGNUP_SIGNIN&client_id=8bdfb9a7-913a-48a8-9fe0-5b2877fb844d&nonce=defaultNonce&redirect_uri=https%3A%2F%2Flocalhost%3A3000&scope=openid&response_type=code&prompt=login"
        />
      </UnauthenticatedTemplate>
    </React.Fragment>
  );
};

export default SignupLoginButton;
