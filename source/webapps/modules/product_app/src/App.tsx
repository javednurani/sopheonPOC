import { Spinner, Stack } from '@fluentui/react';
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
  hideHeaderFooter,
  showHeaderFooter,
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
      hideHeaderFooter();
    } else {
      showHeaderFooter();
    }
  }, [currentStep, products, environmentKey, hideHeaderFooter, showHeaderFooter]);

  if (!environmentKey || getProductsFetchStatus === FetchStatus.NotActive || getProductsFetchStatus === FetchStatus.InProgress) {
    return <Spinner label={formatMessage({ id: 'fallback.loading' })} />;
  }

  if (getProductsFetchStatus === FetchStatus.DoneFailure) {
    showHeaderFooter();
    throw new Error(formatMessage({ id: 'error.erroroccurred' }));
  }

  // TODO: once we fix the header toggle timing issue, this would probably get changed to getProductsFetchStatus === FetchStatus.DoneSuccess && products.length === 0
  const userNeedsOnboarding = (products.length === 0 && environmentKey) || (currentStep === 3 && products.length === 1);

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

  return <Dashboard products={products} />;
};
export default App;
