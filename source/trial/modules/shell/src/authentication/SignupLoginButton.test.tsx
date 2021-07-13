import { Configuration, PublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { messages } from '@sopheon/shared-ui';
import { axe, toHaveNoViolations } from 'jest-axe';
import React, { ReactElement } from 'react';

import { RootState } from '../store';
import { getInitState, languageRender } from '../testUtils';
import SignupLoginButton from './SignupLoginButton';

expect.extend(toHaveNoViolations);

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
  test.skip('Unauthenticated signup button onClick event', async () => {
    // Arrange
    const msalConfig: Configuration = {
      auth: {
        clientId: '8bdfb9a7-913a-48a8-9fe0-5b2877fb844d', // TODO application (clientId) of app registration
      },
    };

    const pca = new PublicClientApplication(msalConfig);
    // Not sure why, but wrapping with MsalProvider causes debug to only contain a div
    const sut: ReactElement = (
      <MsalProvider instance={pca}>
        <SignupLoginButton />
      </MsalProvider>
    );
    // const sut: ReactElement = <SignupLoginButton />;
    const initialState: RootState = getInitState({});
    const mockUseMsal = {
      instance: {
        loginRedirect: jest.fn(),
      },
      accounts: [],
    };
    jest.mock('@azure/msal-react', () => ({
      ...(jest.requireActual('@azure/msal-react') as {}),
      useMsal: jest.fn().mockImplementation(() => mockUseMsal),
    }));

    // Act
    const { debug, getByRole } = languageRender(sut, initialState);
    console.log(debug());
    const button: HTMLElement = getByRole('button');
    button.click();

    // Assert
    expect(mockUseMsal.instance.loginRedirect).toBeCalled();
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
