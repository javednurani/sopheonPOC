import { AccountInfo, IPublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { messages } from '@sopheon/shared-ui';
import { screen, waitFor } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React, { ReactElement } from 'react';
import { IntlProvider } from 'react-intl';

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
  });

  afterEach(() => {
    // cleanup on exiting
    jest.clearAllMocks();
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
    jest.setTimeout(10000);
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
    await waitFor(() => expect(countdown.textContent).toBe('Auto Log Out in 10 seconds...'), { timeout: 5100 });
    expect(logoutRedirectSpy).toHaveBeenCalledTimes(0);
  });
  test('IdleMonitor logs out user after 15 second delay', async () => {
    // Arrange
    jest.setTimeout(30000);
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
    await waitFor(() => expect(countdown.textContent).toBe('Auto Log Out in 0 seconds...'), { timeout: 15100 });
    expect(logoutRedirectSpy).toHaveBeenCalledTimes(1);
  });
});
