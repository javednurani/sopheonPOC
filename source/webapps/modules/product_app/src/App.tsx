import { Spinner, Stack } from '@fluentui/react';
import { SideNav } from '@sopheon/controls';
import { SideBarProps } from '@sopheon/controls/dist/components/SideNav';
import { AppProps, FetchStatus } from '@sopheon/shell-api';
import React, { useEffect } from 'react';
import { useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';
import Dashboard from './Dashboard';
import OnboardingInfo from './onboarding/onboardingInfo';
import { EnvironmentScopedApiRequestModel } from './types';

export type Props = AppProps<AppStateProps, AppDispatchProps>;

const App: React.FunctionComponent<Props> = ({
  currentStep,
  nextStep,
  createProduct,
  updateProduct,
  products,
  getProducts,
  getProductsFetchStatus,
  environmentKey,
  accessToken,
  hideHeader,
  showHeader,
}: Props) => {
  const { formatMessage } = useIntl();

  useEffect(() => {
    if (accessToken && getProductsFetchStatus === FetchStatus.NotActive) {
      const requestDto: EnvironmentScopedApiRequestModel = {
        EnvironmentKey: environmentKey || '',
        AccessToken: accessToken,
      };

      getProducts(requestDto);
    }
  }, [accessToken, environmentKey, getProducts, getProductsFetchStatus]);

  // TODO: move this into onboarding component, but right now this is helping work around our injected saga delay issue
  useEffect(() => {
    if ((products.length === 0 && environmentKey) || (currentStep === 3 && products.length === 1)) {
      hideHeader();
    } else {
      showHeader();
    }
  }, [currentStep, products, environmentKey, hideHeader, showHeader]);

  if (!environmentKey || getProductsFetchStatus === FetchStatus.NotActive || getProductsFetchStatus === FetchStatus.InProgress) {
    return <Spinner label={formatMessage({ id: 'fallback.loading' })} />;
  }

  if (getProductsFetchStatus === FetchStatus.DoneFailure) {
    showHeader();
    throw new Error(formatMessage({ id: 'error.erroroccurred' }));
  }

  // TODO: once we fix the header toggle timing issue, this would probably get changed to getProductsFetchStatus === FetchStatus.DoneSuccess && products.length === 0
  const userNeedsOnboarding = (products.length === 0 && environmentKey) || (currentStep === 3 && products.length === 1);

  const sideProps: SideBarProps = {
    menuItems: [
      {
        links: [
          {
            name: 'Home',
            url: 'http://example.com',
            expandAriaLabel: 'Expand Home section',
            collapseAriaLabel: 'Collapse Home section',
            isExpanded: true,
            key: 'Home',
          },
          {
            name: 'Documents',
            url: 'http://example.com',
            key: 'key3',
            isExpanded: true,
            target: '_blank',
          },
        ],
      },
    ],
    selectedMenuKey: 'Home',
  };

  if (userNeedsOnboarding) {
    return (
      <div>
        <Stack>
          <OnboardingInfo
            currentStep={currentStep}
            nextStep={nextStep}
            createProduct={createProduct}
            updateProduct={updateProduct}
            environmentKey={environmentKey}
            accessToken={accessToken}
            products={products}
          />
        </Stack>
      </div>
    );
  }

  return (
    <Stack horizontal disableShrink>
      <Stack.Item>
        <SideNav {...sideProps} />
      </Stack.Item>
      <Stack.Item grow>
        <Dashboard updateProduct={updateProduct} environmentKey={environmentKey} accessToken={accessToken} products={products} />
      </Stack.Item>
    </Stack>
  );
};
export default App;
