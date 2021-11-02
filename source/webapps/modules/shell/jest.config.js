module.exports = {
  collectCoverageFrom: ['src/**/*.{ts,tsx,js}'],
  globals: {
    crypto: require('crypto'),
  },
  moduleFileExtensions: ['ts', 'tsx', 'js', 'scss'],
  moduleNameMapper: {
    //'^.+\\.(css|less|scss|sass)$': 'identity-obj-proxy',
    '\\.(jpg|jpeg|gif|svg)$': 'identity-obj-proxy',
    '\\.png$': '<rootDir>/src/__mocks__/fileMock.js',
  },
  modulePathIgnorePatterns: ['<rootDir>/.tmp/', '<rootDir>/build/', '<rootDir>/dist/'],
  setupFilesAfterEnv: ['@testing-library/jest-dom/extend-expect', '<rootDir>/build/test-setup.ts'],
  snapshotSerializers: ['enzyme-to-json/serializer'],
  testRegex: '(/__tests__/.*|(\\.|/)(test|spec))\\.(tsx?|scss)$',
  // transform: {
  //   '^.+\\.tsx?$': 'ts-jest'
  // }
};
