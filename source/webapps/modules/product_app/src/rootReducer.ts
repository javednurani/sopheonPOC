import { combineReducers } from 'redux';

import { onboardingReducer, OnboardingStateShape } from './onboarding/onboardingReducer';
import { productReducer, ProductStateShape } from './product/productReducer';

export const rootReducer = combineReducers({
  onboarding: onboardingReducer,
  product: productReducer,
});

type AppState = {
  onboarding: OnboardingStateShape;
  product: ProductStateShape;
};

// The below NAMESPACE string, and RootState key, interact to provide Redux store nested namespacing.
//They should be identical, and reflective of module/microFrontEnd name, eg, 'app1', 'app3', environments'

export type RootState = {
  app: AppState;
};

export const NAMESPACE = 'product_app';
