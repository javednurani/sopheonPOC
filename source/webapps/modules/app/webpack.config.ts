import merge from 'webpack-merge';

import { DEBUG_MODE } from './build/config';
import common from './build/webpack.common';
import data from './build/webpack.data';
import devServer from './build/webpack.devServer';
import fonts from './build/webpack.fonts';
import html from './build/webpack.html';
import images from './build/webpack.images';
import javascript from './build/webpack.javascript';
import optimize from './build/webpack.optimize';
import styles from './build/webpack.styles';

export default !DEBUG_MODE
  ? merge(common(), html(), javascript(), styles(), images(), fonts(), data(), optimize())
  : merge(common(), html(), javascript(), styles(), images(), fonts(), data(), devServer());
