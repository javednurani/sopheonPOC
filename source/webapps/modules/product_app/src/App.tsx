import { Label } from '@fluentui/react';
import { AppProps } from '@sopheon/shell-api';
import React from 'react';
import { FormattedMessage } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';
import Counter from './Counter';

export type Props = AppProps<AppStateProps, AppDispatchProps>;

const App: React.FunctionComponent<Props> = ({ counterValue, decrementCounter, incrementCounter, incrementCounterAsync }: Props) => (
  <div>
    <Label>
      <FormattedMessage id="app.welcome" />
    </Label>
    <Counter
      counterValue={counterValue}
      decrementCounter={decrementCounter}
      incrementCounter={incrementCounter}
      incrementCounterAsync={incrementCounterAsync}
    />
  </div>
);

export default App;
