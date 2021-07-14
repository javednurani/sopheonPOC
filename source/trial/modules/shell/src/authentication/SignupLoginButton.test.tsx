import { Configuration, PublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { messages } from '@sopheon/shared-ui';
import { screen, waitFor } from '@testing-library/react';
import { axe, toHaveNoViolations } from 'jest-axe';
import React, { ReactElement } from 'react';
import { IntlProvider } from 'react-intl';

import { RootState } from '../store';
import { getInitState, languageRender, render } from '../testUtils';
import SignupLoginButton from './SignupLoginButton';

expect.extend(toHaveNoViolations);

afterEach(() => {
  // cleanup on exiting
  jest.clearAllMocks();
});

describe('Test SignupLoginButton component', () => {
  test('Unauthenticated signup button renders correctly', async () => {
    // Arrange
    const sut: ReactElement = <SignupLoginButton />;
    const initialState: RootState = getInitState({});

    // Act
    const { getByText, container, getAllByRole } = languageRender(sut, initialState);
    const signup: HTMLElement = getByText(messages.en['auth.signuplogin']);
    const axeResults = await axe(container);

    // Assert
    expect(getAllByRole('button')).toHaveLength(1);
    expect(signup).toBeInTheDocument();
    expect(axeResults).toHaveNoViolations();
  });
  test('Unauthenticated signup button onClick event', async () => {
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
          <SignupLoginButton />
        </IntlProvider>
      </MsalProvider>
    );
    const button: HTMLElement = await screen.findByText(messages.en['auth.signuplogin']);
    button.click();
    // Assert
    await waitFor(() => expect(loginRedirectSpy).toHaveBeenCalledTimes(1));
  });
  test.skip('Authenticated button renders correctly', () => {
    // Mock msal, expect to render with user's name displayed as button text
    // verify no axe errors
    // click the button to expand the split options, verify no axe issues here either
  });
  test.skip('Authenticated button onClick events', () => {
    // Mock msal, expect to render with user's name displayed as button text
    // At this point the menu options do nothing, eventually will need to test logout and more
  });
});
