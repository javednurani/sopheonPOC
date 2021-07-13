import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from '@azure/msal-react';
import { DefaultButton, IContextualMenuProps } from '@fluentui/react';
import React, { FunctionComponent, useEffect } from 'react';
import { useIntl } from 'react-intl';

const SignupLoginButton: FunctionComponent = () => {
  const { formatMessage } = useIntl();
  const { instance, accounts } = useMsal();

  // !!!!!!
  // Use this section with Implicit Grant Flow settings on the app registration
  // !!!!!!

  useEffect(() => {
    if (window.location.hash.startsWith('#id_token')) {
      instance
        .handleRedirectPromise()
        .then(tokenResponse => {
          if (!tokenResponse) {
            const accountList = instance.getAllAccounts();
            if (accountList.length === 0) {
              // No user signed in
              // instance.loginRedirect();
              instance.acquireTokenRedirect({ scopes: ['openid', 'offline_access'] });
            }
          } else {
            // Do something with the tokenResponse
          }
        })
        .catch(err => {
          // Handle error
          console.error(err);
        });
    }
  }, []);

  // !!!!!!
  // Use this section with Authorization Code Flow settings on the app registration
  // !!!!!!

  // useEffect(() => {
  //   if (window.location.search.startsWith('?code')) {
  //     instance
  //       .handleRedirectPromise()
  //       .then(tokenResponse => {
  //         if (!tokenResponse) {
  //           const accountList = instance.getAllAccounts();
  //           if (accountList.length === 0) {
  //             // No user signed in
  //             // instance.loginRedirect();
  //             instance.acquireTokenRedirect({ scopes: ['openid', 'offline_access'] });
  //           }
  //         } else {
  //           // Do something with the tokenResponse
  //         }
  //       })
  //       .catch(err => {
  //         // Handle error
  //         console.error(err);
  //       });
  //   }
  // }, []);

  const menuProps: IContextualMenuProps = {
    items: [
      {
        key: 'profile',
        text: formatMessage({ id: 'auth.myprofile' }),
        iconProps: { iconName: 'EditContact' },
      },
      {
        key: 'signout',
        text: formatMessage({ id: 'auth.signout' }),
        iconProps: { iconName: 'SignOut' },
        //@ts-ignore
        onClick: () => instance.logout(),
      },
    ],
  };

  return (
    <React.Fragment>
      <AuthenticatedTemplate>
        <DefaultButton text={accounts[0] ? accounts[0].name : formatMessage({ id: 'auth.myprofile' })} split menuProps={menuProps} />
      </AuthenticatedTemplate>
      <UnauthenticatedTemplate>
        <DefaultButton text={formatMessage({ id: 'auth.signuplogin' })} onClick={() => instance.loginRedirect()} />
      </UnauthenticatedTemplate>
    </React.Fragment>
  );
};

export default SignupLoginButton;
