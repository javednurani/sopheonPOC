import { combineReducers } from 'redux';

import { onboardingInfoReducer, OnboardingStateShape } from './onboardingInfoReducer';

export const rootReducer = combineReducers({
  onboardingInfo: onboardingInfoReducer
});

type AppState = {
  onboardingInfo: OnboardingStateShape
};

// The below NAMESPACE string, and RootState key, interact to provide Redux store nested namespacing.
//They should be identical, and reflective of module/microFrontEnd name, eg, 'app1', 'app3', environments'

export type RootState = {
  app: AppState;
};

export const NAMESPACE = 'app';
