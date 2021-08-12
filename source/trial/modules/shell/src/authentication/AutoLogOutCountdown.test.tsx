import { messages } from '@sopheon/shared-ui';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';

import { getInitState, languageRender } from '../testUtils';
import AutoLogOutCountdown from './AutoLogOutCountDown';

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
});
