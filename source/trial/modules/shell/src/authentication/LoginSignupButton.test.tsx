import { AccountInfo, Configuration, PublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { messages } from '@sopheon/shared-ui';
import { screen, waitFor } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React, { ReactElement } from 'react';
import { IntlProvider } from 'react-intl';

import { RootState } from '../store';
import { getInitState, languageRender, randomString, render } from '../testUtils';
import LoginSignupButton from './LoginSignupButton';

expect.extend(toHaveNoViolations);

afterEach(() => {
  // cleanup on exiting
  jest.clearAllMocks();
});

describe('Test Unauthenticated LoginSignupButton component', () => {
  test('button renders correctly and a11y compliant', async () => {
    // Arrange
    const sut: ReactElement = <LoginSignupButton />;
    const initialState: RootState = getInitState({});

    // Act
    const { getByText, container, getAllByRole } = languageRender(sut, initialState);
    const signup: HTMLElement = getByText(messages.en['auth.loginbutton']);
    const axeResults = await axe(container);

    // Assert
    expect(getAllByRole('button')).toHaveLength(1);
    expect(signup).toBeInTheDocument();
    expect(axeResults).toHaveNoViolations();
  });
  test('button onClick event fires loginRedirect', async () => {
    // Arrange
    const msalConfig: Configuration = {
      auth: {
        clientId: '8bdfb9a7-913a-48a8-9fe0-5b2877fb844d',
      },
    };
    const pca = new PublicClientApplication(msalConfig);
    const loginRedirectSpy = jest.spyOn(pca, 'loginRedirect').mockImplementation(request => {
      expect(request).toBe(undefined);

      return Promise.resolve();
    });

    // Act
    render(
      <MsalProvider instance={pca}>
        <IntlProvider locale="en" messages={messages.en}>
          <LoginSignupButton />
        </IntlProvider>
      </MsalProvider>
    );
    const button: HTMLElement = await screen.findByText(messages.en['auth.loginbutton']);
    button.click();
    // Assert
    await waitFor(() => expect(loginRedirectSpy).toHaveBeenCalledTimes(1));
  });
});
describe('Test Authenticated LoginSignupButton component', () => {
  test('button renders correctly and a11y compliant', async () => {
    // Arrange
    const msalConfig: Configuration = {
      auth: {
        clientId: '8bdfb9a7-913a-48a8-9fe0-5b2877fb844d',
      },
    };

    const testAccount: AccountInfo = {
      homeAccountId: randomString(),
      localAccountId: randomString(),
      environment: 'login.windows.net',
      tenantId: randomString(),
      username: 'test@test.com',
      name: randomString(), // This value will appear on button
    };

    const pca = new PublicClientApplication(msalConfig);
    const handleRedirectSpy = jest.spyOn(pca, 'handleRedirectPromise');
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [testAccount]);

    // Act
    const { container } = render(
      <MsalProvider instance={pca}>
        <IntlProvider locale="en" messages={messages.en}>
          <p>This text will always display.</p>
          <LoginSignupButton />
        </IntlProvider>
      </MsalProvider>
    );
    const axeResults = await axe(container);
    const userName: string = testAccount.name ? testAccount.name : '';
    const button: HTMLElement = await screen.findByText(userName);

    // Assert
    await waitFor(() => expect(handleRedirectSpy).toHaveBeenCalledTimes(1));
    expect(screen.queryByText('This text will always display.')).toBeInTheDocument();
    expect(button).toBeInTheDocument();
    expect(axeResults).toHaveNoViolations();
  });
});