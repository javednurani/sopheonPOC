module.exports = {
  collectCoverageFrom: ['src/**/*.{ts,tsx,js}'],
  moduleFileExtensions: ['ts', 'tsx', 'js', 'scss'],
  modulePathIgnorePatterns: ['<rootDir>/.tmp/', '<rootDir>/build/', '<rootDir>/dist/', '<rootDir>/deploy/', '<rootDir>/webpack/'],
  setupFilesAfterEnv: ['@testing-library/jest-dom/extend-expect', '<rootDir>/webpack/test-setup.ts'],
  snapshotSerializers: ['enzyme-to-json/serializer'],
  testRegex: '(/__tests__/.*|(\\.|/)(test|spec))\\.(tsx?|scss)$',
};
