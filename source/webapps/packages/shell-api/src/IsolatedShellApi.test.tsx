import { createStore, Reducer, Store } from 'redux';

import { IsolatedShellApi } from './IsolatedShellApi';

describe('@sopheon/shell-api IsolatedShellApi', () => {
  test('IsolatedShellApi constructor with store', () => {
    // Arrange
    const fakeReducer: Reducer = (state, _action) => state;
    const fakeStore: Store = createStore(fakeReducer);
    const shellApi = new IsolatedShellApi(fakeStore);

    // Act
    const retrievedStore: Store = shellApi.getStore;

    // Assert
    expect(retrievedStore).toBe(fakeStore);
  });
});
