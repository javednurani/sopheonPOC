import { FunctionComponent } from 'react';

import App from './App';
import { ShellApiProps } from './TempSopheonShellAPI';

export type AppStateProps = unknown;

export type AppDispatchProps = unknown;

const AppContainer: FunctionComponent<ShellApiProps> = ({ shellApi }: ShellApiProps) => shellApi.connectApp(App);

export default AppContainer;
