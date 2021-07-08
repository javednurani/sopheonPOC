function clean(storage: globalThis.Storage): void {
  for (const key of Object.keys(storage)) {
    if (key.startsWith('@')) {
      delete storage[key];
    }
  }
}

function mock(storage: globalThis.Storage): void {
  storage.__getItem__ = storage.getItem;
  storage.__setItem__ = storage.setItem;
  storage.__removeItem__ = storage.removeItem;

  storage.getItem = jest.fn(key => storage[key]);
  storage.setItem = jest.fn((key, value) => (storage[key] = value));
  storage.removeItem = jest.fn(key => delete storage[key]);
}

function reset(storage: globalThis.Storage): void {
  clean(storage);

  storage.getItem = jest.fn(key => storage[key]);
  storage.setItem = jest.fn((key, value) => (storage[key] = value));
  storage.removeItem = jest.fn(key => delete storage[key]);
}

function restore(storage: globalThis.Storage): void {
  clean(storage);

  storage.getItem = storage.__getItem__;
  storage.setItem = storage.__setItem__;
  storage.removeItem = storage.__removeItem__;

  delete storage.__getItem__;
  delete storage.__setItem__;
  delete storage.__removeItem__;
}

afterAll(() => {
  restore(localStorage);
  restore(sessionStorage);
});

beforeAll(() => {
  mock(localStorage);
  mock(sessionStorage);
});

beforeEach(() => {
  reset(localStorage);
  reset(sessionStorage);
});
