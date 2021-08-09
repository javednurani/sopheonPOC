import { AccountInfo, IPublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { messages } from '@sopheon/shared-ui';
import { screen, waitFor } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React, { ReactElement } from 'react';
import { IntlProvider } from 'react-intl';

import { autoLogOutTime, showAutoLogOutWarningThreshhold } from '../settings/appSettings';
import { randomMsalAccount, render, testMsalInstance } from '../testUtils';
import IdleMonitor from './IdleMonitor';

expect.extend(toHaveNoViolations);

describe('Test Unauthenticated IdleMonitor component', () => {
  test('IdleMonitor does not render when unauthenticated', async () => {
    // Arrange
    const sut: ReactElement = <IdleMonitor />;

    // Act
    const { container } = render(sut);
    const axeResults = await axe(container);

    // Assert
    // expect(IdleMonitor).not.toBeInTheDocument();
    expect(axeResults).toHaveNoViolations();
  });
});
describe('Test Authenticated IdleMonitor component', () => {
  let pca: IPublicClientApplication;

  beforeEach(() => {
    pca = testMsalInstance();
    jest.useFakeTimers();
  });

  afterEach(() => {
    // cleanup on exiting
    jest.clearAllMocks();
    jest.useRealTimers();
  });
  test('IdleMonitor renders when authenticated', async () => {
    // Arrange
    const testAccount: AccountInfo = randomMsalAccount();
    const handleRedirectSpy = jest.spyOn(pca, 'handleRedirectPromise');
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [testAccount]);
    const logoutRedirectSpy = jest.spyOn(pca, 'logoutRedirect').mockImplementation(request => {
      expect(request).toBe(undefined);

      return Promise.resolve();
    });

    // Act
    render(
      <MsalProvider instance={pca}>
        <IntlProvider locale="en" messages={messages.en}>
          <IdleMonitor />
        </IntlProvider>
      </MsalProvider>
    );

    // Assert
    await waitFor(() => expect(handleRedirectSpy).toHaveBeenCalledTimes(1));
    const countdown: HTMLElement = await screen.findByTestId('idleCountdown');
    expect(countdown).toBeInTheDocument();
    expect(logoutRedirectSpy).toHaveBeenCalledTimes(0);
  });
  test('IdleMonitor shows text after 5 second delay', async () => {
    // Arrange
    const testAccount: AccountInfo = randomMsalAccount();
    const handleRedirectSpy = jest.spyOn(pca, 'handleRedirectPromise');
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [testAccount]);
    const logoutRedirectSpy = jest.spyOn(pca, 'logoutRedirect').mockImplementation(request => {
      expect(request).toBe(undefined);

      return Promise.resolve();
    });

    // Act
    render(
      <MsalProvider instance={pca}>
        <IntlProvider locale="en" messages={messages.en}>
          <IdleMonitor />
        </IntlProvider>
      </MsalProvider>
    );

    // Assert
    await waitFor(() => expect(handleRedirectSpy).toHaveBeenCalledTimes(1));
    const countdown: HTMLElement = await screen.findByTestId('idleCountdown');
    expect(countdown).toBeInTheDocument();
    setTimeout(
      () => expect(countdown.textContent).toBe(`Auto Log Out in ${showAutoLogOutWarningThreshhold / 1000} seconds...`),
      autoLogOutTime - showAutoLogOutWarningThreshhold
    );
    expect(logoutRedirectSpy).toHaveBeenCalledTimes(0);
  });
  test('IdleMonitor logs out user after 15 second delay', async () => {
    // Arrange
    const testAccount: AccountInfo = randomMsalAccount();
    const handleRedirectSpy = jest.spyOn(pca, 'handleRedirectPromise');
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [testAccount]);
    const logoutRedirectSpy = jest.spyOn(pca, 'logoutRedirect').mockImplementation(request => {
      expect(request).toBe(undefined);

      return Promise.resolve();
    });

    // Act
    render(
      <MsalProvider instance={pca}>
        <IntlProvider locale="en" messages={messages.en}>
          <IdleMonitor />
        </IntlProvider>
      </MsalProvider>
    );

    // Assert
    await waitFor(() => expect(handleRedirectSpy).toHaveBeenCalledTimes(1));
    const countdown: HTMLElement = await screen.findByTestId('idleCountdown');
    expect(countdown).toBeInTheDocument();
    setTimeout(() => expect(logoutRedirectSpy).toBeCalledTimes(1), autoLogOutTime);
  });
});
