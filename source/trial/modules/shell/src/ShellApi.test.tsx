import { createStore, Reducer, Store } from 'redux';

import { ShellApi } from './ShellApi';

describe('ShellApi', () => {
  test('ShellApi constructor with store', () => {
    // Arrange
    const fakeReducer: Reducer = (state, _action) => state;
    const fakeStore: Store = createStore(fakeReducer);
    const shellApi = new ShellApi(fakeStore);

    // Act
    const retrievedStore: Store = shellApi.getStore;

    // Assert
    expect(retrievedStore).toBe(fakeStore);
  });
});
