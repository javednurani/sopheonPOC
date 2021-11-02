import { Action, PayloadAction } from './types';

// https://github.com/redux-utilities/flux-standard-action
// Flux Standard Action - metadata can be any type of value, (extra information not part of payload)

export const createAction = <Type extends string, Meta>(type: Type, meta?: Meta): Action<Type, Meta> => ({
  type,
  meta,
});

export const createPayloadAction = <Type extends string, Payload, Meta>(
  type: Type,
  payload: Payload,
  meta?: Meta
): PayloadAction<Type, Payload, Meta> => ({
  ...createAction(type, meta),
  payload,
});
