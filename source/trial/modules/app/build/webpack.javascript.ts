import path from 'path';
import webpack from 'webpack';

import { DEBUG_MODE, PRJ_DIR } from './config';

export default (): webpack.Configuration => ({
  module: {
    rules: [
      {
        test: /\.(ts|js)x?$/i,
        exclude: path.join(PRJ_DIR, 'node_modules'),
        use: [
          {
            loader: 'babel-loader',
            options: {
              plugins: [
                ['@babel/plugin-proposal-decorators', { legacy: true }],
                ['@babel/plugin-proposal-class-properties', { loose: true }],
                ['@babel/plugin-proposal-object-rest-spread', { loose: true }],
                '@babel/plugin-proposal-optional-chaining',
                '@babel/plugin-syntax-dynamic-import',
                '@babel/plugin-transform-react-jsx',
                '@babel/plugin-transform-runtime',
              ],
              presets: [
                [
                  '@babel/preset-env',
                  {
                    corejs: { proposals: true, version: 3 },
                    loose: true,
                    modules: false, // Let Webpack handle the imports.
                    useBuiltIns: 'entry', //https://github.com/zloirock/core-js/issues/743 ( vs 'usage'), prevents $ is not a function error in core-js/modules/es-global-this
                  },
                ],
                '@babel/preset-typescript',
                '@babel/preset-react',
              ],
            },
          },
        ],
      },
    ],
  },
  output: {
    chunkFilename: DEBUG_MODE ? '[name].[hash].chunk.js' : '[name].[chunkhash].chunk.js',
    filename: DEBUG_MODE ? '[name].[hash].bundle.js' : '[name].[chunkhash].bundle.js',
    sourceMapFilename: DEBUG_MODE ? '[name].[hash].bundle.map' : '[name].[chunkhash].bundle.map',
  },
});
