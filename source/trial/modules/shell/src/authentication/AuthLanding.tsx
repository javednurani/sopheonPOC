import { StringDict } from '@azure/msal-common';
import { useMsal } from '@azure/msal-react';
import { ISpinnerStyles, IStackStyles, Spinner, SpinnerSize, Stack } from '@fluentui/react';
import React, { CSSProperties, FunctionComponent, useEffect } from 'react';
import { useIntl } from 'react-intl';

import AzureBlueBackground from '../images/azure-blue-background.png';
import { getAuthLandingRedirectRequest } from './authHelpers';

const sectionStyle: CSSProperties = {
  width: '100vw',
  height: '100vh',
  backgroundImage: `url(${AzureBlueBackground})`,
};

export interface AuthLandingProps {
  queryParams: StringDict | undefined;
  spinnerMessageResourceKey: string;
}

const AuthLanding: FunctionComponent<AuthLandingProps> = ({ queryParams, spinnerMessageResourceKey }: AuthLandingProps) => {
  const { instance } = useMsal();
  const { formatMessage } = useIntl();
  useEffect(() => {
    document.body.style.margin = '0 0';

    instance
      .handleRedirectPromise()
      .then(tokenResponse => {
        if (!tokenResponse) {
          instance.loginRedirect(getAuthLandingRedirectRequest(queryParams));
        }
      })
      .catch(err => {
        // Handle error
        // TODO, haven't observed errors here yet...monitor, and replace console.error as appropriate?
        // eslint-disable-next-line no-console
        console.error('REDIRECT_PROMISE_ERROR', err);
      });
  }, []);

  const stackStyles: IStackStyles = {
    root: {
      height: '100vh',
    },
  };
  const spinnerStyles: ISpinnerStyles = {
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
              <Spinner styles={spinnerStyles} size={SpinnerSize.large} label={formatMessage({ id: spinnerMessageResourceKey })} />
            </Stack.Item>
          </Stack>
        </Stack.Item>
      </Stack>
    </section>
  );
};

export default AuthLanding;
