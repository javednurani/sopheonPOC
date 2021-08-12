import { screen } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';

import { showAutoLogOutWarningThreshholdSeconds } from '../settings/appSettings';
import { getInitState, render } from '../testUtils';
import { languageRender } from './../testUtils';
import AutoLogOutCountdown from './AutoLogOutCountdown';

expect.extend(toHaveNoViolations);

describe('AutoLogOutCountdown', () => {
  test('Has no a11y vialotions.', async () => {
    // Act
    const { container } = render(<AutoLogOutCountdown />);
    const axeResults = await axe(container);

    // Assert
    expect(axeResults).toHaveNoViolations();
  });
  test('Countdown timer starts at warning threshold', async () => {
    // Arrange
    const sut = <AutoLogOutCountdown />;
    const initialState = getInitState({});

    // Act
    languageRender(sut, initialState);
    const warningText: HTMLElement = await screen.findByText('Are you still working?', { exact: false });
    // Assert
    expect(warningText.textContent).toContain(showAutoLogOutWarningThreshholdSeconds);
  });
});
