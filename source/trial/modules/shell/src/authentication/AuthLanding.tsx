import { useMsal } from '@azure/msal-react';
import { ISpinnerStyles, IStackStyles, Spinner, SpinnerSize, Stack } from '@fluentui/react';
import React, { CSSProperties, FunctionComponent, useEffect } from 'react';
import { useIntl } from 'react-intl';

import azureSettings from '../azureSettings';
import AzureBlueBackground from '../images/azure-blue-background.png';

const sectionStyle: CSSProperties = {
  width: '100vw',
  height: '100vh',
  backgroundImage: `url(${AzureBlueBackground})`,
};

const isDev = process.env.NODE_ENV === 'development';
const redirectUri: string = isDev ? azureSettings.SPA_Root_URL_Dev : azureSettings.SPA_Root_URL;

export interface AuthLandingProps {
  adB2cPolicyName: string;
  spinnerMessageResourceKey: string;
}

const AuthLanding: FunctionComponent<AuthLandingProps> = ({ adB2cPolicyName, spinnerMessageResourceKey }: AuthLandingProps) => {
  const { instance } = useMsal();
  const { formatMessage } = useIntl();
  useEffect(() => {
    document.body.style.margin = '0 0';

    const adB2cTenantName: string = isDev ? azureSettings.AD_B2C_TenantName_Dev : azureSettings.AD_B2C_TenantName;

    const authorityUrl = `https://${adB2cTenantName}.b2clogin.com/${adB2cTenantName}.onmicrosoft.com/${adB2cPolicyName}`;
    instance
      .handleRedirectPromise()
      .then(tokenResponse => {
        if (!tokenResponse) {
          instance.loginRedirect({
            authority: authorityUrl,
            scopes: ['openid', 'offline_access'],
            redirectUri: redirectUri,
            redirectStartPage: redirectUri,
            prompt: 'login',
          });
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
