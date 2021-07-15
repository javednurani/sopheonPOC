import { useMsal } from '@azure/msal-react';
import { Spinner, SpinnerSize, Stack } from '@fluentui/react';
import React, { FunctionComponent, useEffect } from 'react';
import { useIntl } from 'react-intl';

import azureSettings from '../azureSettings';
import AzureBlueBackground from '../images/azure-blue-background.png';

const sectionStyle = {
  width: '100%',
  height: '100vh',
  backgroundImage: `url(${AzureBlueBackground})`,
};

const isDev = process.env.NODE_ENV === 'development';
const redirectUri: string = isDev ? azureSettings.SPA_Root_URL_Dev : azureSettings.SPA_Root_URL;

const NewUserLanding: FunctionComponent = () => {
  const { instance } = useMsal();
  const { formatMessage } = useIntl();
  useEffect(() => {
    document.body.style.margin = '0 0';
    instance
      .handleRedirectPromise()
      .then(tokenResponse => {
        if (!tokenResponse) {
          const accountList = instance.getAllAccounts();
          if (accountList.length === 0) {
            instance.loginRedirect({
              scopes: ['openid', 'offline_access'],
              redirectUri: redirectUri,
              redirectStartPage: redirectUri,
            });
          }
        }
      })
      .catch(err => {
        // Handle error
        // TODO, haven't observed errors here yet...monitor, and replace console.error as appropriate?
        // eslint-disable-next-line no-console
        console.error('REDIRECT_PROMISE_ERROR', err);
      });
  }, []);

  const stackStyles = {
    root: {
      height: '100vh',
    },
  };
  const spinnerStyles = {
    label: {
      color: 'white',
      fontSize: '18px',
    },
    circle: {
      borderColor: 'white',
      height: '40px',
      width: '40px',
    },
  };
  return (
    <section style={sectionStyle}>
      <Stack>
        <Stack.Item grow>
          <Stack horizontal verticalAlign="center" styles={stackStyles}>
            <Stack.Item grow>
              <Spinner styles={spinnerStyles} size={SpinnerSize.large} label={formatMessage({ id: 'newuserlanding.enteringflow' })} />
            </Stack.Item>
          </Stack>
        </Stack.Item>
      </Stack>
    </section>
  );
};

export default NewUserLanding;
