module.exports = {
  preset: 'ts-jest/presets/js-with-ts',
  transformIgnorePatterns: [/* to have node_modules transformed*/],
  testPathIgnorePatterns: ["/node_modules", "/dist"],
  testEnvironment: 'jsdom',
  globals: {
    'ts-jest': {
      isolatedModules: true, // to make type check faster
      tsConfig: {            // to have tsc transform .js files
        allowJs: true,
        checkJs: false,
      },
    }
  }
};