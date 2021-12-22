import { Stack } from '@fluentui/react';
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
  updateProductItem,
  products,
  getProducts,
  getProductsFetchStatus,
  environmentKey,
  accessToken,
  hideHeaderFooter,
  showHeaderFooter,
  createTask,
  updateTask,
}: Props) => {
  useEffect(() => {
    if (accessToken && getProductsFetchStatus === FetchStatus.NotActive) {
      const requestDto: EnvironmentScopedApiRequestModel = {
        EnvironmentKey: environmentKey || '',
        AccessToken: accessToken,
      };

      getProducts(requestDto);
    }
  }, [accessToken, getProductsFetchStatus]);

  // TODO: maybe this should be the respnsibility of the onbaording component to control?
  useEffect(() => {
    if ((products.length === 0 && environmentKey) || (currentStep === 3 && products.length === 1)) {
      hideHeaderFooter();
    } else {
      showHeaderFooter();
    }
  }, [products, environmentKey]);

  const { formatMessage } = useIntl();

  // TODO: condition copied from above, can be simplified?
  const userNeedsOnboarding = (products.length === 0 && environmentKey) || (currentStep === 3 && products.length === 1);

  if (!environmentKey) {
    return (
      <Stack horizontalAlign="center">
        <h2>{formatMessage({ id: 'onboarding.pleaseLogin' })}</h2>
      </Stack>
    );
  }

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
    <Dashboard
      updateProduct={updateProduct}
      updateProductItem={updateProductItem}
      environmentKey={environmentKey}
      accessToken={accessToken}
      products={products}
      createTask={createTask}
      updateTask={updateTask}
    />
  );
};
export default App;
