import HtmlWebpackPlugin from 'html-webpack-plugin';
import path from 'path';
import webpack from 'webpack';

import { DEBUG_MODE, PUB_DIR } from './config';

function htmlMinifyOptions(): Record<string, boolean> | undefined {
  return !DEBUG_MODE
    ? {
        collapseWhitespace: true,
        keepClosingSlash: true,
        minifyCSS: true,
        minifyJS: true,
        minifyURLs: true,
        removeComments: true,
        removeEmptyAttributes: true,
        removeRedundantAttributes: true,
        removeStyleLinkTypeAttributes: true,
        useShortDoctype: true,
      }
    : undefined;
}

export default (): webpack.Configuration => ({
  module: {
    rules: [
      {
        test: /\.html$/i,
        // eslint-disable-next-line sort-keys
        loader: 'html-loader',
      },
    ],
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: path.join(PUB_DIR, 'index.html'),
      minify: htmlMinifyOptions(),
      favicon: path.join(PUB_DIR, 'Lucy16_logo.png'),
    }),
  ],
});
