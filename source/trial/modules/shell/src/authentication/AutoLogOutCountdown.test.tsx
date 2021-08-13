import { IPublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { messages } from '@sopheon/shared-ui';
import { screen, waitFor } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';

import { IdleTimeoutSettings } from '../settings/appSettings';
import { getInitState, testMsalInstance } from '../testUtils';
import { languageRender } from './../testUtils';
import AutoLogOutCountdown from './AutoLogOutCountdown';

expect.extend(toHaveNoViolations);

describe('AutoLogOutCountdown', () => {
  let pca: IPublicClientApplication;
  let mockToggleHidden: jest.Mock;

  // Reset the tests
  beforeEach(() => {
    pca = testMsalInstance();
    mockToggleHidden = jest.fn();
  });
  afterEach(() => {
    // cleanup on exiting
    jest.clearAllMocks();
    jest.useRealTimers();
  });
  test('Has no a11y vialotions.', async () => {
    // Act
    const { container } = languageRender(<AutoLogOutCountdown hidden={false} toggleHidden={mockToggleHidden} />, getInitState({}));
    const axeResults = await axe(container);

    // Assert
    expect(axeResults).toHaveNoViolations();
  });
  test('To have Yes and No buttons', async () => {
    // Act
    const { getByText } = languageRender(<AutoLogOutCountdown hidden={false} toggleHidden={mockToggleHidden} />, getInitState({}));

    // Assert
    const yesButton: HTMLElement = getByText(messages.en.yes);
    const noButton: HTMLElement = getByText(messages.en.no);

    expect(yesButton).toBeInTheDocument();
    expect(noButton).toBeInTheDocument();
  });
  test('Logout called when No button clicked', async () => {
    // Arrange
    const logoutRedirectSpy = jest.spyOn(pca, 'logoutRedirect').mockImplementation(request => {
      expect(request).toBe(undefined);

      return Promise.resolve();
    });

    // Act
    languageRender(
      <MsalProvider instance={pca}>
        <AutoLogOutCountdown hidden={false} toggleHidden={mockToggleHidden} />
      </MsalProvider>,
      getInitState({})
    );
    const noButton: HTMLElement = await screen.findByText(messages.en.no);
    noButton.click();

    // Assert
    expect(logoutRedirectSpy).toBeCalledTimes(1);
  });
  test('Countdown timer starts at warning threshold', async () => {
    // Arrange
    const logoutRedirectSpy = jest.spyOn(pca, 'logoutRedirect').mockImplementation(request => {
      expect(request).toBe(undefined);

      return Promise.resolve();
    });
    const sut = <AutoLogOutCountdown hidden={false} toggleHidden={mockToggleHidden} />;
    const initialState = getInitState({});

    // Act
    languageRender(sut, initialState);
    const warningText: HTMLElement = await screen.findByText('Are you still working?', { exact: false });

    // Assert
    expect(warningText.textContent).toContain(IdleTimeoutSettings.IdleLogOutWarningSeconds);
    await waitFor(() => expect(logoutRedirectSpy).toBeCalledTimes(0));
  });
  test('Countdown timer advances properly', async () => {
    // Arrange
    jest.useFakeTimers();
    const logoutRedirectSpy = jest.spyOn(pca, 'logoutRedirect').mockImplementation(request => {
      expect(request).toBe(undefined);

      return Promise.resolve();
    });

    const sut = (
      <MsalProvider instance={pca}>
        <AutoLogOutCountdown hidden={false} toggleHidden={mockToggleHidden} />
      </MsalProvider>
    );
    const initialState = getInitState({});
    const secondsToAdvance = 5;

    // Act
    languageRender(sut, initialState);
    const warningText: HTMLElement = await screen.findByText('Are you still working?', { exact: false });
    jest.advanceTimersByTime(secondsToAdvance * 1000);

    // Assert
    expect(warningText.textContent).not.toContain(IdleTimeoutSettings.IdleLogOutWarningSeconds);
    expect(warningText.textContent).toContain(IdleTimeoutSettings.IdleLogOutWarningSeconds - secondsToAdvance);
    await waitFor(() => expect(logoutRedirectSpy).toBeCalledTimes(0));
  });
  test('Logout called when timer is 0', async () => {
    // Arrange
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
        <AutoLogOutCountdown hidden={false} toggleHidden={mockToggleHidden} />
      </MsalProvider>,
      getInitState({})
    );

    // Assert
    await waitFor(() => expect(logoutRedirectSpy).toBeCalledTimes(1));
  });
});
