import AutoPrefixer from 'autoprefixer';
import CssNano from 'cssnano';
import MiniCssExtractPlugin from 'mini-css-extract-plugin';
// eslint-disable-next-line @typescript-eslint/ban-ts-comment
// @ts-ignore
import PostCssPresetEnv from 'postcss-preset-env';
import webpack from 'webpack';

import { DEBUG_MODE } from './config';

const extractCss = !DEBUG_MODE;

const cssRules = [
  {
    loader: 'css-loader',
    options: {
      sourceMap: DEBUG_MODE,
    },
  },
  {
    loader: 'postcss-loader',
    options: {
      postcssOptions: {
        ident: 'postcss',
        plugins: () => [
          PostCssPresetEnv,
          AutoPrefixer,
          // eslint-disable-next-line new-cap
          CssNano({
            preset: 'default',
          }),
        ],
      },
      sourceMap: DEBUG_MODE,
    },
  },
];

const sassRules = [
  {
    loader: 'resolve-url-loader',
  },
  {
    loader: 'sass-loader',
    options: {
      additionalData: `$debug-mode: ${DEBUG_MODE};`,
      sassOptions: {
        includePaths: ['node_modules'],
        precision: 8, // Needed for Bootstrap.
        sourceMap: DEBUG_MODE,
      },
    },
  },
];

const configuration: webpack.Configuration = {
  module: {
    rules: [
      // CSS required in JS/TS files should use the style-loader that auto-injects it into the website
      // only when the issuer is a .js/.ts file, so the loaders are not applied inside html templates
      {
        test: /\.css$/i,
        // eslint-disable-next-line sort-keys
        //issuer: [{ not: [{ test: /\.html$/i }] }],
        use: extractCss
          ? [
              {
                loader: MiniCssExtractPlugin.loader,
              },
              ...cssRules,
            ]
          : ['style-loader', ...cssRules],
      },
      {
        test: /\.css$/i,
        // eslint-disable-next-line sort-keys
        //issuer: [{ test: /\.html$/i }],
        // CSS required in templates cannot be extracted safely
        // because Aurelia would try to require it again in runtime
        use: cssRules,
      },
      {
        test: /\.scss$/,
        // eslint-disable-next-line sort-keys
        issuer: /\.[tj]sx?$/i,
        use: extractCss
          ? [
              {
                loader: MiniCssExtractPlugin.loader,
              },
              ...cssRules,
              ...sassRules,
            ]
          : ['style-loader', ...cssRules, ...sassRules],
      },
      {
        test: /\.scss$/,
        // eslint-disable-next-line sort-keys
        issuer: /\.html?$/i,
        use: [...cssRules, ...sassRules],
      },
    ],
  },
};

export default (): webpack.Configuration => configuration;
