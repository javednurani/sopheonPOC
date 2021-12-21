import React from 'react';

import { AppDispatchProps, AppStateProps } from './AppContainer';
import { AppProps } from './TempSopheonShellAPI';

export type Props = AppProps<AppStateProps, AppDispatchProps>;

const App: React.FunctionComponent<Props> = () => <div></div>;

export default App;
