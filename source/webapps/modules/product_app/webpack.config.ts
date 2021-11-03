import merge from 'webpack-merge';

import { DEBUG_MODE } from './webpack/config';
import common from './webpack/webpack.common';
import data from './webpack/webpack.data';
import devServer from './webpack/webpack.devServer';
import fonts from './webpack/webpack.fonts';
import html from './webpack/webpack.html';
import images from './webpack/webpack.images';
import javascript from './webpack/webpack.javascript';
import optimize from './webpack/webpack.optimize';
import styles from './webpack/webpack.styles';

export default !DEBUG_MODE
  ? merge(common(), html(), javascript(), styles(), images(), fonts(), data(), optimize())
  : merge(common(), html(), javascript(), styles(), images(), fonts(), data(), devServer());
