import { AccountInfo, IPublicClientApplication } from '@azure/msal-browser';
import { messages } from '@sopheon/shared-ui';
import { waitFor } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React, { ReactElement } from 'react';
import { IntlProvider } from 'react-intl';

import { randomMsalAccount, render, testMsalInstance } from '../testUtils';
import IdleMonitor, { handleOnIdle } from './IdleMonitor';

expect.extend(toHaveNoViolations);

describe('Test Unauthenticated IdleMonitor component', () => {
  let pca: IPublicClientApplication;

  // Reset the tests
  beforeEach(() => {
    pca = testMsalInstance();
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => []); // No accounts logged in
  });

  afterEach(() => {
    // cleanup on exiting
    jest.clearAllMocks();
  });

  test('IdleMonitor renders correctly', async () => {
    // Arrange
    const sut: ReactElement = (
      <IntlProvider locale="en" messages={messages.en}>
        <IdleMonitor />
      </IntlProvider>
    );

    // Act
    const { container } = render(sut);
    const axeResults = await axe(container);

    // Assert
    expect(axeResults).toHaveNoViolations();
  });
  test('Test handleOnIdle does not call toggle function if no active accounts', async () => {
    // Arrange
    const toggleMock = jest.fn();

    // Act
    handleOnIdle(pca, toggleMock);

    // Assert
    await waitFor(() => expect(toggleMock).toBeCalledTimes(0));
  });
});
describe('Test Authenticated IdleMonitor component', () => {
  let pca: IPublicClientApplication;

  // Reset the tests
  beforeEach(() => {
    pca = testMsalInstance();
    const testAccount: AccountInfo = randomMsalAccount();
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [testAccount]); // A user is logged in
  });

  afterEach(() => {
    // cleanup on exiting
    jest.clearAllMocks();
  });

  test('Test handleOnIdle calls toggle function if an account is active', async () => {
    // Arrange
    const toggleMock = jest.fn();

    // Act
    handleOnIdle(pca, toggleMock);

    // Assert
    await waitFor(() => expect(toggleMock).toBeCalledTimes(1));
  });
});
