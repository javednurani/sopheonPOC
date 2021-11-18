import webpack from 'webpack';

import { MAX_SIZE_DATA_URL } from './config';

export default (): webpack.Configuration => ({
  module: {
    rules: [
      {
        test: /\.woff2(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
        loader: 'url-loader',
        options: { limit: MAX_SIZE_DATA_URL, mimetype: 'application/font-woff2' },
      },
      {
        test: /\.woff(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
        loader: 'url-loader',
        options: { limit: MAX_SIZE_DATA_URL, mimetype: 'application/font-woff' },
      },
      // Load these fonts normally, as files:
      {
        test: /\.(ttf|eot|svg|otf)(\?v=[0-9]\.[0-9]\.[0-9])?$/i,
        loader: 'file-loader',
      },
    ],
  },
});
