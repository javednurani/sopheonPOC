import { IconButton, PrimaryButton } from '@fluentui/react';
import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import React from 'react';
import { IntlProvider } from 'react-intl';

import { Props } from './App';
import Counter from './Counter';

describe('Testing the Counter component', () => {
  it('Render test for the Counter component', () => {
    const appProps: Props = {
      counterValue: 1,
      decrementCounter: jest.fn(),
      incrementCounter: jest.fn(),
      incrementCounterAsync: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <Counter
          counterValue={appProps.counterValue}
          decrementCounter={appProps.decrementCounter}
          incrementCounter={appProps.incrementCounter}
          incrementCounterAsync={appProps.incrementCounterAsync}
        />
      </IntlProvider>
    );
    expect(wrapper.find(IconButton)).toHaveLength(2);
    expect(wrapper.find('#counterValue').text()).toBe(appProps.counterValue.toString());
    expect(wrapper.find(PrimaryButton)).toHaveLength(1);
  });
  it('userEvent test for the Counter component', () => {
    const appProps: Props = {
      counterValue: 1,
      decrementCounter: jest.fn(),
      incrementCounter: jest.fn(),
      incrementCounterAsync: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <Counter
          counterValue={appProps.counterValue}
          decrementCounter={appProps.decrementCounter}
          incrementCounter={appProps.incrementCounter}
          incrementCounterAsync={appProps.incrementCounterAsync}
        />
      </IntlProvider>
    );
    wrapper.find(PrimaryButton).simulate('click');
    expect(appProps.incrementCounterAsync).toHaveBeenCalled();
    wrapper.find(IconButton).forEach(iconButton => iconButton.simulate('click'));
    expect(appProps.incrementCounter).toHaveBeenCalled();
    expect(appProps.decrementCounter).toHaveBeenCalled();
  });
});
