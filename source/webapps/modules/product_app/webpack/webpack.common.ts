import { CleanWebpackPlugin } from 'clean-webpack-plugin';
import MiniCssExtractPlugin from 'mini-css-extract-plugin';
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
    crossOriginLoading: 'anonymous', // Allows for shared components to communicate between MFEs.
    path: DST_DIR,
    publicPath: IS_PROD ? FOLDER_PATH : `https://localhost:${PORT}/`,
  },
  performance: { hints: false },
  plugins: [
    new CleanWebpackPlugin(),
    new MiniCssExtractPlugin(),
    new ModuleFederationPlugin({
      name: 'productAppModule',
      filename: 'remoteEntry.js',
      remotes: {},
      exposes: {
        './App': './src/AppContainer',
      },
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
