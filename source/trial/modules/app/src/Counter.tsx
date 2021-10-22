import { Stack } from '@fluentui/react';
import Button from '@sopheon/controls/dist/components/Button';
import React, { CSSProperties } from 'react';
import { useIntl } from 'react-intl';

import { AppDispatchProps, AppStateProps } from './AppContainer';

export type CounterProps = AppStateProps & AppDispatchProps;

const Counter: React.FunctionComponent<CounterProps> = ({
  counterValue,
  decrementCounter,
  incrementCounter,
  incrementCounterAsync,
}: CounterProps) => {
  const { formatMessage } = useIntl();

  const countStyles: CSSProperties = {
    fontSize: '48px',
  };

  return (
    <Stack className="counterContainer" horizontalAlign="center">
      <span id="counterValue" style={countStyles}>
        {counterValue}
      </span>
      <Stack horizontal>
        <Button variant="icon" icon="Add" aria-label={formatMessage({ id: 'aria.increment' })} onClick={() => incrementCounter()} />
        <Button variant="icon" icon="Remove" aria-label={formatMessage({ id: 'aria.decrement' })} onClick={() => decrementCounter()} />
        <Button
          label={formatMessage({ id: 'app.add5' })}
          aria-label={formatMessage({ id: 'app.add5_aria' })}
          onClick={() => incrementCounterAsync()}
        />
      </Stack>
    </Stack>
  );
};

export default Counter;
