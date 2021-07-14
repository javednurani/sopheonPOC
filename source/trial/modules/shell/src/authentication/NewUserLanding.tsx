import { useMsal } from '@azure/msal-react';
import { IStackProps, Spinner, SpinnerSize, Stack } from '@fluentui/react';
import React, { FunctionComponent, useEffect } from 'react';

import AzureBlueBackground from '../images/azure-blue-background.png';

const sectionStyle = {
  width: '100%',
  height: '100%',
  backgroundImage: `url(${AzureBlueBackground})`,
};

const NewUserLanding: FunctionComponent = () => {
  const { instance } = useMsal();
  useEffect(() => {
    document.body.style.margin = '0 0';
    // TODO: tokenize / isDev redirectUris
    // TODO, stack style (center spinner on page?)
    // TODO, resource spinner label
    // TODO, spinner style white
    instance
      .handleRedirectPromise()
      .then(tokenResponse => {
        if (!tokenResponse) {
          const accountList = instance.getAllAccounts();
          if (accountList.length === 0) {
            instance.loginRedirect({
              scopes: ['openid', 'offline_access'],
              redirectUri: 'https://localhost:3000/', // https://stratusapp-dev.azureedge.net/ https://localhost:3000/
              redirectStartPage: 'https://localhost:3000/', // https://stratusapp-dev.azureedge.net/ https://localhost:3000/
            });
          }
        }
      })
      .catch(err => {
        // Handle error
        // eslint-disable-next-line no-console
        console.error('REDIRECT_PROMISE_ERROR', err);
      });
  }, []);

  const stackProps: IStackProps = { horizontalAlign: 'center', verticalAlign: 'center' };
  return (
    <section style={sectionStyle}>
      <Stack {...stackProps}>
        <Stack.Item>
          <Spinner size={SpinnerSize.large} label="Entering signup/signin flow..." />
        </Stack.Item>
      </Stack>
    </section>
  );
};

export default NewUserLanding;
