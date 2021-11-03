import { initialState, nextStep, onboardingInfoReducer, OnboardingStateShape } from './onboardingInfoReducer';

describe('onboardingInfoReducer', () => {
  test('nextStep', () => {
    const nextState: OnboardingStateShape = onboardingInfoReducer(initialState, nextStep());
    expect(nextState.currentStep).toEqual(2);
  });
});
