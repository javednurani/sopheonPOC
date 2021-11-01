import { createAction, createPayloadAction } from './actions';

describe('@sopheon/shell-api action creators', () => {
  const META_OBJECT: unknown = {
    someKey: 123,
    someOtherKey: 'arbitrary value',
  };
  const PAYLOAD_OBJECT: unknown = {
    someKey: 456,
    someOtherKey: 'arbitrary value 2',
  };
  const ACTION_TYPE = 'ACTION_TYPE';

  test('createAction', () => {
    // Act
    const createdAction = createAction(ACTION_TYPE);

    // Assert
    expect(createdAction.type).toBe(ACTION_TYPE);
    expect(createdAction.meta).toBeFalsy();
  });
  test('createAction with meta', () => {
    // Act
    const createdAction = createAction(ACTION_TYPE, META_OBJECT);

    // Assert
    expect(createdAction.type).toBe(ACTION_TYPE);
    expect(createdAction.meta).toBe(META_OBJECT);
  });
  test('createPayloadAction', () => {
    // Act
    const createdPayloadAction = createPayloadAction(ACTION_TYPE, PAYLOAD_OBJECT);
    // Assert
    expect(createdPayloadAction.type).toBe(ACTION_TYPE);
    expect(createdPayloadAction.payload).toBe(PAYLOAD_OBJECT);
    expect(createdPayloadAction.meta).toBeFalsy();
  });
  test('createPayloadAction with meta', () => {
    // Act
    const createdPayloadAction = createPayloadAction(ACTION_TYPE, PAYLOAD_OBJECT, META_OBJECT);
    // Assert
    expect(createdPayloadAction.type).toBe(ACTION_TYPE);
    expect(createdPayloadAction.payload).toBe(PAYLOAD_OBJECT);
    expect(createdPayloadAction.meta).toBe(META_OBJECT);
  });
});
