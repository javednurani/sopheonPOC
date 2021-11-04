import { Action, createAction } from '@sopheon/shell-api';
import { Reducer } from 'redux';

//#region  Action Types

// REDUCER ACTION TYPES

// eslint-disable-next-line no-shadow
enum OnboardingActionTypes {
  NEXT_STEP = 'APP/NEXT_STEP'
}

export type NextStepAction = Action<OnboardingActionTypes.NEXT_STEP>;

export type OnboardingReducerActions = NextStepAction;
//#endregion

//#region  Action Creators

// REDUCER ACTIONS

export const nextStep = (): NextStepAction => createAction(OnboardingActionTypes.NEXT_STEP);

//#endregion

//#region Reducer

// INITIAL STATE & DEFAULTS

export type OnboardingStateShape = {
  currentStep: number;
};

export const initialState: OnboardingStateShape = {
  currentStep: 1,
};

// HANDLERS

const setValue = (state: OnboardingStateShape, valueToSet: number): OnboardingStateShape => ({
  ...state,
  currentStep: valueToSet,
});

// ACTION SWITCH

export const onboardingInfoReducer: Reducer<OnboardingStateShape, OnboardingReducerActions> = (state = initialState, action) => {
  switch (action.type) {
    case OnboardingActionTypes.NEXT_STEP:
      return setValue(state, state.currentStep + 1);
    default:
      return state;
  }
};

//#endregion