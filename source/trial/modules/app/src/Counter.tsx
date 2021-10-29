import { IconButton, IIconProps, PrimaryButton, Stack } from '@fluentui/react';
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

  const addIcon: IIconProps = { iconName: 'Add' };
  const subtractIcon: IIconProps = { iconName: 'Remove' };

  const countStyles: CSSProperties = {
    fontSize: '48px',
  };

  return (
    <Stack className="counterContainer" horizontalAlign="center">
      <span id="counterValue" style={countStyles}>
        {counterValue}
      </span>
      <Stack horizontal>
        <IconButton iconProps={addIcon} aria-label={formatMessage({ id: 'aria.increment' })} onClick={() => incrementCounter()} />
        <IconButton iconProps={subtractIcon} aria-label={formatMessage({ id: 'aria.decrement' })} onClick={() => decrementCounter()} />
        <PrimaryButton
          text={formatMessage({ id: 'app.add5' })}
          aria-label={formatMessage({ id: 'app.add5_aria' })}
          onClick={() => incrementCounterAsync()}
        />
      </Stack>
    </Stack>
  );
};

export default Counter;
