import { initialState, nextStep, onboardingReducer, OnboardingStateShape } from './onboardingReducer';

describe.skip('onboardingReducer', () => {
  test('nextStep', () => {
    const nextState: OnboardingStateShape = onboardingReducer(initialState, nextStep());
    expect(nextState.currentStep).toEqual(2);
  });
});
