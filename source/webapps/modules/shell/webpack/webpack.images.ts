// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
import webpack from 'webpack';

import { DEBUG_MODE, MAX_SIZE_DATA_URL } from './config';

export default (): webpack.Configuration => ({
  module: {
    rules: [
      {
        test: /\.(png|jpe?g|gif|webp|cur)$/,
        use: [
          {
            loader: 'url-loader',
            options: {
              compress: !DEBUG_MODE,
              limit: MAX_SIZE_DATA_URL,
              name: 'images/[name].[ext]',
            },
          },
        ],
      },
      {
        test: /\.svg$/,
        use: ['@svgr/webpack'],
      },
    ],
  },
});
