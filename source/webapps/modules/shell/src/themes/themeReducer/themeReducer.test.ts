import { darkTheme, lightTheme } from '@sopheon/shared-ui';

import { ThemeShape } from '../../types';
import { changeTheme, initialState as initialThemeState, themeReducer as reducer } from './themeReducer';

describe('ThemeReducer', () => {
  const lightThemeShape: ThemeShape = {
    theme: lightTheme,
  };
  const darkThemeShape: ThemeShape = {
    theme: darkTheme,
  };

  test('Change theme sets from init light to dark', () => {
    // Act
    const result: ThemeShape = reducer(initialThemeState, changeTheme(true));

    // Assert
    expect(result).toEqual(darkThemeShape);
  });

  test('Change theme sets from dark to light', () => {
    // Act
    const result: ThemeShape = reducer(darkThemeShape, changeTheme(false));

    // Assert
    expect(result).toEqual(lightThemeShape);
  });
});
