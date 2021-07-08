import axe from '@axe-core/react';
import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import Shell from './Shell';
import { shellApi } from './ShellApi';

if (process.env.NODE_ENV !== 'production') {
  axe(React, ReactDOM, 1000);
}

ReactDOM.render(
  <Provider store={shellApi.getStore}>
    <Shell />
  </Provider>,
  document.getElementById('root')
);
