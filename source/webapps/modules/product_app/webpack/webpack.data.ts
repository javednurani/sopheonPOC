import webpack from 'webpack';

import { BUILD_ENV, SRC_DIR } from './config';

export default (): webpack.Configuration => ({
  module: {
    rules: [
      {
        test: /settings\.json$/i,
        use: [{ loader: 'app-settings-loader', options: { env: BUILD_ENV } }],
      },
      {
        test: /\.(csv|tsv)$/,
        include: SRC_DIR,
        use: [
          {
            loader: 'csv-loader',
            options: {
              dynamicTyping: true,
              header: true,
              skipEmptyLines: true,
            },
          },
        ],
      },
      {
        test: /\.cson$/,
        include: SRC_DIR,
        loader: 'cson-loader',
      },
      {
        test: /\.json5?$/,
        include: SRC_DIR,
        loader: 'json5-loader',
      },
      {
        test: /\.xml$/,
        include: SRC_DIR,
        use: [
          {
            loader: 'xml-loader',
            options: {
              explicitArray: false,
              explicitRoot: false,
              mergeAttrs: true,
              normalize: true,
            },
          },
        ],
      },
    ],
  },
});
