import chokidar from 'chokidar';
import fs from 'fs-extra';
import path from 'path';
import webpack from 'webpack';

import { DEBUG_MODE, DST_DIR, PORT, SRC_DIR } from './config';

const pfxFile = process.env.SSL_PFX_FILE || 'localhost.pfx';
const pfxPassphrase = process.env.SSL_PFX_PASSPHRASE || '';
const httpsEnabled = fs.existsSync(pfxFile);

export default (index = 'index.html'): webpack.Configuration => ({
  devServer: {
    before: (_app, server) => {
      chokidar.watch([path.join(SRC_DIR, '**', '*.html'), path.join(SRC_DIR, '**', '*.ejs')]).on('all', () => {
        server.sockWrite(server.sockets, 'content-changed');
      });
    },
    contentBase: [DST_DIR, SRC_DIR],
    disableHostCheck: true,
    historyApiFallback: true, // Serve index.html for all 404 (required for push-state)
    host: '0.0.0.0',
    hot: DEBUG_MODE,
    https: httpsEnabled,
    index: index,
    open: false,
    overlay: true,
    pfx: httpsEnabled ? pfxFile : undefined,
    pfxPassphrase: httpsEnabled ? pfxPassphrase : undefined,
    port: PORT,
    watchOptions: {
      aggregateTimeout: 300,
      poll: 1000,
    },
    headers: {
      // Allows for shared components to communicate between MFEs.
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, PATCH, OPTIONS',
      'Access-Control-Allow-Headers': 'X-Requested-With, content-type, Authorization',
    },
  },
});
