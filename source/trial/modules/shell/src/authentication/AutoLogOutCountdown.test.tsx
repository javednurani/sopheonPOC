import { IPublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { messages } from '@sopheon/shared-ui';
import { screen, waitFor } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';

import { showAutoLogOutWarningThreshholdSeconds } from '../settings/appSettings';
import { getInitState, testMsalInstance } from '../testUtils';
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
  test('Logout called when timer is 0', async () => {
    // Arrange
    const pca: IPublicClientApplication = testMsalInstance();
    const logoutRedirectSpy = jest.spyOn(pca, 'logoutRedirect').mockImplementation(request => {
      expect(request).toBe(undefined);

      return Promise.resolve();
    });
    const setState = jest.fn();
    const useStateSpy = jest.spyOn(React, 'useState');
    useStateSpy.mockImplementation(() => [0, setState]);

    // Act
    languageRender(
      <MsalProvider instance={pca}>
        <AutoLogOutCountdown />
      </MsalProvider>,
      getInitState({})
    );

    // Assert
    await waitFor(() => expect(logoutRedirectSpy).toBeCalledTimes(1));
  });
});
