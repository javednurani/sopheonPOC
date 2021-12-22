import { CleanWebpackPlugin } from 'clean-webpack-plugin';
import path from 'path';
import webpack from 'webpack';

import { DEBUG_MODE, DST_DIR, FOLDER_PATH, IS_PROD, PORT, PRJ_DIR, SRC_DIR } from './config';

// eslint-disable-next-line @typescript-eslint/no-var-requires
const { ModuleFederationPlugin } = require('webpack').container;

export default (): webpack.Configuration => ({
  devtool: DEBUG_MODE ? 'inline-source-map' : false,
  entry: {
    index: path.resolve(SRC_DIR, 'index.ts'),
  },
  mode: DEBUG_MODE ? 'development' : 'production',
  output: {
    path: DST_DIR,
    publicPath: IS_PROD ? FOLDER_PATH : `https://localhost:${PORT}/`,
  },
  performance: { hints: false },
  plugins: [
    new CleanWebpackPlugin(),
    new ModuleFederationPlugin({
      name: 'mainshellModule',
      filename: 'remoteEntry.js',
      remotes: {},
      exposes: {},
      shared: {
        'react': {},
        'react-intl': {},
        'react-redux': {},
        'react-router-dom': {},
        '@azure/msal-react': { singleton: true },
        '@azure/msal-browser': { singleton: true },
      },
    }),
    /* Hides warning on start in development mode in axe-core
    Critical dependency: require function is used in a way in which dependencies cannot be statically extracted  */
    new webpack.ContextReplacementPlugin(/axe-core/, false),
    /* Fixes runtime error with 4/27/2021 bug introduced in @microsoft/applicationinsights
    See: https://github.com/microsoft/ApplicationInsights-JS/issues/1527
    See: https://github.com/microsoft/ApplicationInsights-JS/issues/1553
    See: https://github.com/digitalpalitools/web-ui/commit/b6b89706028ab34082f2bf001cab12ceff84e850
    */
    new webpack.ProvidePlugin({
      __assign: ['@microsoft/applicationinsights-shims', '__assignFn'],
      __extends: ['@microsoft/applicationinsights-shims', '__extendsFn'],
    }),
  ],
  resolve: {
    alias: {
      isarray: path.resolve(PRJ_DIR, 'node_modules/isarray'),
    },
    extensions: ['.ts', '.tsx', '.js', '.jsx'],
    fallback: { crypto: false },
    modules: [PRJ_DIR, 'node_modules'],
  },
});
