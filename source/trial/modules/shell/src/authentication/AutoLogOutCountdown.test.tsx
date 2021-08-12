import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';

import { render } from '../testUtils';
import AutoLogOutCountdown from './AutoLogOutCountDown';

expect.extend(toHaveNoViolations);

describe('AutoLogOutCountdown', () => {
  test('Has no a11y vialotions.', async () => {
    // Act
    const { container } = render(<AutoLogOutCountdown />);
    const axeResults = await axe(container);

    // Assert
    expect(axeResults).toHaveNoViolations();
  });
});
