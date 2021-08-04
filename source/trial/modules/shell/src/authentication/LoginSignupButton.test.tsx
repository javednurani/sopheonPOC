import { AccountInfo } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { messages } from '@sopheon/shared-ui';
import { screen } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React, { ReactElement } from 'react';
import { IntlProvider } from 'react-intl';

import { RootState } from '../store';
import { getInitState, languageRender, randomMsalAccount, render, testMsalInstance } from '../testUtils';
import { azureSettings } from './../azureSettings';
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
    const pca = testMsalInstance();
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
    expect(loginRedirectSpy).toHaveBeenCalledTimes(1);
  });
});
describe('Test Authenticated LoginSignupButton component', () => {
  test('button renders correctly and a11y compliant', async () => {
    // Arrange
    const pca = testMsalInstance();
    const testAccount: AccountInfo = randomMsalAccount();

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
    expect(handleRedirectSpy).toHaveBeenCalledTimes(1);
    expect(screen.queryByText('This text will always display.')).toBeInTheDocument();
    expect(button).toBeInTheDocument();
    expect(axeResults).toHaveNoViolations();
  });
  test('MyProfile button calls ProfileEdit loginRedirect onClick', async () => {
    // Arrange
    const pca = testMsalInstance();
    const testAccount: AccountInfo = randomMsalAccount();

    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [testAccount]);
    const loginRedirectSpy = jest.spyOn(pca, 'loginRedirect').mockImplementation(request => {
      expect(request?.authority).toContain(azureSettings.AD_B2C_ProfileEdit_Policy);

      return Promise.resolve();
    });

    // Act
    render(
      <MsalProvider instance={pca}>
        <IntlProvider locale="en" messages={messages.en}>
          <p>This text will always display.</p>
          <LoginSignupButton />
        </IntlProvider>
      </MsalProvider>
    );

    const userName: string = testAccount.name ? testAccount.name : '';
    const accountButton: HTMLElement = await screen.findByText(userName);
    accountButton.click();
    const profileButton: HTMLElement = await screen.findByText(messages.en['auth.myprofile']);
    profileButton.click();

    // Assert
    expect(loginRedirectSpy).toHaveBeenCalledTimes(1);
  });
  test('ChangePassword button calls ProfileEdit_PasswordChange loginRedirect onClick', async () => {
    // Arrange
    const pca = testMsalInstance();
    const testAccount: AccountInfo = randomMsalAccount();

    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [testAccount]);
    const loginRedirectSpy = jest.spyOn(pca, 'loginRedirect').mockImplementation(request => {
      expect(request?.authority).toContain(azureSettings.AD_B2C_PasswordChange_Policy);

      return Promise.resolve();
    });

    // Act
    render(
      <MsalProvider instance={pca}>
        <IntlProvider locale="en" messages={messages.en}>
          <p>This text will always display.</p>
          <LoginSignupButton />
        </IntlProvider>
      </MsalProvider>
    );

    const userName: string = testAccount.name ? testAccount.name : '';
    const accountButton: HTMLElement = await screen.findByText(userName);
    accountButton.click();
    const profileButton: HTMLElement = await screen.findByText(messages.en['auth.changepassword']);
    profileButton.click();

    // Assert
    expect(loginRedirectSpy).toHaveBeenCalledTimes(1);
  });
});
