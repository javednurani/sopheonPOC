// eslint-disable no-console
const keys = ['debug', 'info', 'warn', 'error'];

interface ConsoleIndex extends Console {
  [key: string]: any; // eslint-disable-line @typescript-eslint/no-explicit-any
}

afterAll(() => {
  keys.forEach(key => {
    ((console as ConsoleIndex)[key] as jest.Mock).mockRestore();
  });
});

beforeAll(() => {
  keys.forEach(key => {
    (console as ConsoleIndex)[key] = jest.fn();
  });
});

beforeEach(() => {
  keys.forEach(key => {
    ((console as ConsoleIndex)[key] as jest.Mock).mockReset();
  });
});
