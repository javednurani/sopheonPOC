import { AccountInfo, Configuration, PublicClientApplication } from '@azure/msal-browser';

import { azureSettings, getAuthorityDomain } from '../azureSettings';
import { randomString } from '../testUtils';
import { getMsalAccount } from './authHelpers';

describe('Test authHelpers', () => {
  it('getMsalAccount - 0 accounts - returns undefined', async () => {
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
    const result = getMsalAccount(pca);

    // Assert
    expect(result).toBeUndefined();
  });
  it('getMsalAccount - 2 signupsignin accounts (same user) - returns 1st signupsignin account', async () => {
    // Arrange
    const sharedLocalAccountId: string = randomString();

    const signUpSignInAccount1: AccountInfo = {
      homeAccountId: randomString() + azureSettings.AD_B2C_SignUpSignIn_Policy,
      localAccountId: sharedLocalAccountId,
      environment: 'login.windows.net',
      tenantId: randomString(),
      username: 'test@test.com',
      name: randomString(), // This value will appear on button
      idTokenClaims: {
        iss: randomString() + getAuthorityDomain(),
        aud: azureSettings.AD_B2C_ClientId
      }
    };

    const signUpSignInAccount2: AccountInfo = {
      homeAccountId: randomString() + azureSettings.AD_B2C_SignUpSignIn_Policy,
      localAccountId: sharedLocalAccountId,
      environment: 'login.windows.net',
      tenantId: randomString(),
      username: 'test@test.com',
      name: randomString(), // This value will appear on button
      idTokenClaims: {
        iss: randomString() + getAuthorityDomain(),
        aud: azureSettings.AD_B2C_ClientId
      }
    };

    const msalConfig: Configuration = {
      auth: {
        clientId: randomString(),
      },
    };
    const pca = new PublicClientApplication(msalConfig);

    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    // 0 accounts
    getAllAccountsSpy.mockImplementation(() => [signUpSignInAccount1, signUpSignInAccount2]);

    // Act
    const result = getMsalAccount(pca);

    // Assert
    expect(result).toBe(signUpSignInAccount1);
  });
  it('getMsalAccount - 2 signupsignin accounts (different users) - returns undefined, logout() called', async () => {
    // Arrange

    const signUpSignInAccount1: AccountInfo = {
      homeAccountId: randomString() + azureSettings.AD_B2C_SignUpSignIn_Policy,
      localAccountId: randomString(),
      environment: 'login.windows.net',
      tenantId: randomString(),
      username: 'test@test.com',
      name: randomString(), // This value will appear on button
      idTokenClaims: {
        iss: randomString() + getAuthorityDomain(),
        aud: azureSettings.AD_B2C_ClientId
      }
    };

    const signUpSignInAccount2: AccountInfo = {
      homeAccountId: randomString() + azureSettings.AD_B2C_SignUpSignIn_Policy,
      localAccountId: randomString(),
      environment: 'login.windows.net',
      tenantId: randomString(),
      username: 'test@test.com',
      name: randomString(), // This value will appear on button
      idTokenClaims: {
        iss: randomString() + getAuthorityDomain(),
        aud: azureSettings.AD_B2C_ClientId
      }
    };

    const msalConfig: Configuration = {
      auth: {
        clientId: randomString(),
      },
    };
    const pca = new PublicClientApplication(msalConfig);

    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    // 0 accounts
    getAllAccountsSpy.mockImplementation(() => [signUpSignInAccount1, signUpSignInAccount2]);

    const logoutSpy = jest.spyOn(pca, 'logout');

    // Act
    const result = getMsalAccount(pca);

    // Assert
    expect(result).toBe(undefined);
    expect(logoutSpy).toHaveBeenCalledTimes(1);
  });
  it('getMsalAccount - 1 signupsignin account and 1 other account (of same user) - returns signupsignin account', async () => {
    // Arrange

    const signUpSignInAccount: AccountInfo = {
      homeAccountId: randomString() + azureSettings.AD_B2C_SignUpSignIn_Policy,
      localAccountId: randomString(),
      environment: 'login.windows.net',
      tenantId: randomString(),
      username: 'test@test.com',
      name: randomString(), // This value will appear on button
      idTokenClaims: {
        iss: randomString() + getAuthorityDomain(),
        aud: azureSettings.AD_B2C_ClientId
      }
    };

    const otherUserFlowAccount: AccountInfo = {
      homeAccountId: randomString(),
      localAccountId: randomString(),
      environment: 'login.windows.net',
      tenantId: randomString(),
      username: 'test@test.com',
      name: randomString(), // This value will appear on button
    };

    const msalConfig: Configuration = {
      auth: {
        clientId: randomString(),
      },
    };
    const pca = new PublicClientApplication(msalConfig);

    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    // 0 accounts
    getAllAccountsSpy.mockImplementation(() => [signUpSignInAccount, otherUserFlowAccount]);

    // Act
    const result = getMsalAccount(pca);

    // Assert
    expect(result).toBe(signUpSignInAccount);
  });
  it('getMsalAccount - 1 account - returns account', async () => {
    // Arrange

    const testAccount: AccountInfo = {
      homeAccountId: randomString(),
      localAccountId: randomString(),
      environment: 'login.windows.net',
      tenantId: randomString(),
      username: 'test@test.com',
      name: randomString(), // This value will appear on button
    };

    const msalConfig: Configuration = {
      auth: {
        clientId: randomString(),
      },
    };
    const pca = new PublicClientApplication(msalConfig);

    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    // 0 accounts
    getAllAccountsSpy.mockImplementation(() => [testAccount]);

    // Act
    const result = getMsalAccount(pca);

    // Assert
    expect(result).toBe(testAccount);
  });
});
