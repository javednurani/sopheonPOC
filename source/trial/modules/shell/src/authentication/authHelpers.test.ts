import { AccountInfo } from '@azure/msal-browser';

import { azureSettings, getAuthorityDomain } from '../azureSettings';
import { randomMsalAccount, randomString, testMsalInstance } from '../testUtils';
import { getMsalAccount } from './authHelpers';

describe('Test authHelpers', () => {
  it('getMsalAccount - 0 accounts - returns undefined', async () => {
    // Arrange
    const pca = testMsalInstance();

    // 0 accounts
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => []);

    // Act
    const result = getMsalAccount(pca);

    // Assert
    expect(result).toBeUndefined();
  });
  it('getMsalAccount - 2 signupsignin accounts (same user) - returns 1st signupsignin account', async () => {
    // Arrange
    const sharedLocalAccountId: string = randomString();

    const signUpSignInAccount1: AccountInfo = randomMsalAccount();
    signUpSignInAccount1.homeAccountId = `${randomString()}-${azureSettings.AD_B2C_SignUpSignIn_Policy}`;
    signUpSignInAccount1.localAccountId = sharedLocalAccountId;
    signUpSignInAccount1.idTokenClaims = {
      iss: `${randomString()}-${getAuthorityDomain()}`,
      aud: azureSettings.AD_B2C_ClientId
    };

    const signUpSignInAccount2: AccountInfo = randomMsalAccount();
    signUpSignInAccount2.homeAccountId = `${randomString()}-${azureSettings.AD_B2C_SignUpSignIn_Policy}`;
    signUpSignInAccount2.localAccountId = sharedLocalAccountId;
    signUpSignInAccount2.idTokenClaims = {
      iss: `${randomString()}-${getAuthorityDomain()}`,
      aud: azureSettings.AD_B2C_ClientId
    };

    const pca = testMsalInstance();

    // 2 signupsignin accounts (same user)
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [signUpSignInAccount1, signUpSignInAccount2]);

    // Act
    const result = getMsalAccount(pca);

    // Assert
    expect(result).toBe(signUpSignInAccount1);
  });
  it('getMsalAccount - 2 signupsignin accounts (different users) - returns undefined, logout() called', async () => {
    // Arrange
    const signUpSignInAccount1: AccountInfo = randomMsalAccount();
    signUpSignInAccount1.homeAccountId = `${randomString()}-${azureSettings.AD_B2C_SignUpSignIn_Policy}`;
    signUpSignInAccount1.idTokenClaims = {
      iss: `${randomString()}-${getAuthorityDomain()}`,
      aud: azureSettings.AD_B2C_ClientId
    };

    const signUpSignInAccount2: AccountInfo = randomMsalAccount();
    signUpSignInAccount2.homeAccountId = `${randomString()}-${azureSettings.AD_B2C_SignUpSignIn_Policy}`;
    signUpSignInAccount2.idTokenClaims = {
      iss: `${randomString()}-${getAuthorityDomain()}`,
      aud: azureSettings.AD_B2C_ClientId
    };

    const pca = testMsalInstance();

    // 2 signupsignin accounts (different users)
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
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
    const sharedLocalAccountId: string = randomString();

    const signUpSignInAccount: AccountInfo = randomMsalAccount();
    signUpSignInAccount.homeAccountId = `${randomString()}-${azureSettings.AD_B2C_SignUpSignIn_Policy}`;
    signUpSignInAccount.localAccountId = sharedLocalAccountId;
    signUpSignInAccount.idTokenClaims = {
      iss: `${randomString()}-${getAuthorityDomain()}`,
      aud: azureSettings.AD_B2C_ClientId
    };

    const otherUserFlowAccount: AccountInfo = randomMsalAccount();
    otherUserFlowAccount.localAccountId = sharedLocalAccountId;

    const pca = testMsalInstance();

    // 1 signupsignin account and 1 other account (of same user)
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [signUpSignInAccount, otherUserFlowAccount]);

    // Act
    const result = getMsalAccount(pca);

    // Assert
    expect(result).toBe(signUpSignInAccount);
  });
  it('getMsalAccount - 1 account - returns account', async () => {
    // Arrange

    const testAccount: AccountInfo = randomMsalAccount();

    const pca = testMsalInstance();

    // 1 account
    const getAllAccountsSpy = jest.spyOn(pca, 'getAllAccounts');
    getAllAccountsSpy.mockImplementation(() => [testAccount]);

    // Act
    const result = getMsalAccount(pca);

    // Assert
    expect(result).toBe(testAccount);
  });
});
