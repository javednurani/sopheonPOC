import { AccountInfo, Configuration, PublicClientApplication } from '@azure/msal-browser';
import { MsalProvider } from '@azure/msal-react';
import { messages } from '@sopheon/shared-ui';
import { shallow } from 'enzyme';
import React, { ReactElement } from 'react';
import { IntlProvider } from 'react-intl';

import { randomString } from '../testUtils';
import Signup from './Signup';

const sut: ReactElement = <Signup />;
let mockLogoutSpy: jest.Mock<Promise<void>, []>;
let mockAccounts: AccountInfo[] = [];
const mockAccount: AccountInfo = {
  homeAccountId: randomString(),
  localAccountId: randomString(),
  environment: 'login.windows.net',
  tenantId: randomString(),
  username: 'test@test.com',
  name: randomString(),
};
const msalConfig: Configuration = {
  auth: {
    clientId: randomString(),
  },
};
const pca = new PublicClientApplication(msalConfig);

jest.mock('@azure/msal-react', () => ({
  ...jest.requireActual('@azure/msal-react'),
  // redefine useMsal as a jest mock function
  useMsal: jest.fn(() => ({
    instance: {
      logout: mockLogoutSpy,
    },
    accounts: mockAccounts,
  })),
}));

beforeEach(() => {
  // reset mock spy for each test
  mockLogoutSpy = jest.fn(() => Promise.resolve());
});

afterEach(() => {
  // clear mocks after each test (best practice?)
  jest.clearAllMocks();
});

describe('Signup when logged in', () => {
  test('calls logout once.', async () => {
    // Arrange
    mockAccounts = [mockAccount]; // user is logged in

    // Act
    const wrapper = shallow(
      <MsalProvider instance={pca}>
        <IntlProvider locale="en" messages={messages.en}>
          {sut}
        </IntlProvider>
      </MsalProvider>
    );

    // dive to deeper levels to find/exercise our sut
    wrapper.find(Signup).dive();

    // Assert
    expect(mockLogoutSpy).toHaveBeenCalledTimes(1);
  });
});

describe('Signup when NOT logged in', () => {
  test('does not log user out', async () => {
    // Arrange
    mockAccounts = []; // user is logged out

    // Act
    const wrapper = shallow(
      <MsalProvider instance={pca}>
        <IntlProvider locale="en" messages={messages.en}>
          {sut}
        </IntlProvider>
      </MsalProvider>
    );

    // dive to deeper levels to find/exercise our sut
    wrapper.find(Signup).dive();

    // Assert
    expect(mockLogoutSpy).not.toHaveBeenCalled();
  });
});
