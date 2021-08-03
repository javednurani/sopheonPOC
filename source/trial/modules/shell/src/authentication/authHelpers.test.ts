import { Configuration, PublicClientApplication } from '@azure/msal-browser';

import { randomString } from '../testUtils';
import { getMsalAccount } from './authHelpers';

describe('Test authHelpers', () => {
  it('getMsalAccount with 0 accounts does not call setAccount', async () => {
    // Arrange
    const msalConfig: Configuration = {
      auth: {
        clientId: randomString(),
      },
    };
    const pca = new PublicClientApplication(msalConfig);

    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    // 0 accounts
    getAllAccountsSpy.mockImplementation(() => []);

    // Act
    getMsalAccount(pca);

    // Assert
    // undefined here? TBD...
  });
});
