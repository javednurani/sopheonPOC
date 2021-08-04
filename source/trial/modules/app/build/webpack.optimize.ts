import webpack from 'webpack';

import { DEBUG_MODE } from './config';

export default (): webpack.Configuration => ({
  optimization: {
    minimize: !DEBUG_MODE,
    moduleIds: 'hashed',
    runtimeChunk: false,
    splitChunks: {
      cacheGroups: {
        default: false,

        // This is the HTTP/1.1 optimised cacheGroup configuration
        vendors: {
          enforce: true,
          minSize: 30000,
          name: 'vendors',
          priority: 19,
          test: /[\\/]node_modules[\\/]/,
        },
        vendorsAsync: {
          chunks: 'async',
          minSize: 10000,
          name: 'vendors.async',
          priority: 9,
          reuseExistingChunk: true,
          test: /[\\/]node_modules[\\/]/,
        },
        commonsAsync: {
          chunks: 'async',
          minChunks: 2,
          minSize: 10000,
          name: 'commons.async',
          priority: 0,
          reuseExistingChunk: true,
        },
      },
      chunks: 'initial',
      hidePathInfo: true,
      maxSize: 200000,
    },
  },
});
