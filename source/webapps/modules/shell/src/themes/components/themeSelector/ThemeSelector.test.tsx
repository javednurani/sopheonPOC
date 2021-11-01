import { messages } from '@sopheon/shared-ui';
import { createPayloadAction } from '@sopheon/shell-api';
import userEvent from '@testing-library/user-event';
import React, { ReactElement } from 'react';

import { RootState } from '../../../store';
import { getInitState, languageRender } from '../../../testUtils';
import { ChangeThemeAction, ThemeActionTypes } from '../../themeReducer/themeReducer';
import ThemeSelector from './ThemeSelector';

describe('Test ThemeSelector component', () => {
  test('Test component renders correctly', () => {
    // Arrange
    const changeThemeMock = jest.fn((useDarkTheme: boolean): ChangeThemeAction => createPayloadAction(ThemeActionTypes.CHANGE_THEME, useDarkTheme));

    const sut: ReactElement = <ThemeSelector changeTheme={changeThemeMock} />;
    const initialState: RootState = getInitState({});

    // Act
    const { getByText, getByRole } = languageRender(sut, initialState);
    const label: HTMLElement = getByText(messages.en['header.useDarkTheme']);
    const toggle: HTMLElement = getByRole('switch');

    expect(label).toBeInTheDocument();
    expect(toggle).toBeInTheDocument();
  });

  test('Test onChange event', () => {
    // Arrange
    const changeThemeMock = jest.fn((useDarkTheme: boolean): ChangeThemeAction => createPayloadAction(ThemeActionTypes.CHANGE_THEME, useDarkTheme));

    const sut: ReactElement = <ThemeSelector changeTheme={changeThemeMock} />;
    const initialState: RootState = getInitState({});

    // Act
    const { getByRole } = languageRender(sut, initialState);
    const toggle: HTMLElement = getByRole('switch');
    userEvent.click(toggle);

    // Assert
    expect(changeThemeMock).toHaveBeenCalled();
  });

  //TODO: add test that calls on change with checked = undefined if possible
});
