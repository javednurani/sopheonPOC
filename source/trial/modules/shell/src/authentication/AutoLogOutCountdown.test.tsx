import { messages } from '@sopheon/shared-ui';
import { screen } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';

import { showAutoLogOutWarningThreshholdSeconds } from '../settings/appSettings';
import { getInitState } from '../testUtils';
import { languageRender } from './../testUtils';
import AutoLogOutCountdown from './AutoLogOutCountdown';

expect.extend(toHaveNoViolations);

describe('AutoLogOutCountdown', () => {
  test('Has no a11y vialotions.', async () => {
    // Act
    const { container } = languageRender(<AutoLogOutCountdown />, getInitState({}));
    const axeResults = await axe(container);

    // Assert
    expect(axeResults).toHaveNoViolations();
  });
  test('To have Yes and No buttons', async () => {
    // Act
    const { getByText } = languageRender(<AutoLogOutCountdown />, getInitState({}));

    // Assert
    const yesButton: HTMLElement = getByText(messages.en.yes);
    const noButton: HTMLElement = getByText(messages.en.no);

    expect(yesButton).toBeInTheDocument();
    expect(noButton).toBeInTheDocument();
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
