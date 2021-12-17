import { Label } from '@fluentui/react';
import { mount } from 'enzyme';
import toJson from 'enzyme-to-json';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';
import { IntlProvider } from 'react-intl';

import App, { Props } from './App';
import Counter from './Counter';
import { messages } from './TempSopheonSharedUI';

expect.extend(toHaveNoViolations);

describe('Testing the App component', () => {
  it('Render test for the App component', () => {
    const appProps: Props = {
      counterValue: 1,
      decrementCounter: jest.fn(),
      incrementCounter: jest.fn(),
      incrementCounterAsync: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <App
          counterValue={appProps.counterValue}
          decrementCounter={appProps.decrementCounter}
          incrementCounter={appProps.incrementCounter}
          incrementCounterAsync={appProps.incrementCounterAsync}
        />
      </IntlProvider>
    );
    expect(wrapper.find(Label)).toHaveLength(1);
    expect(wrapper.find(Label).text()).toBe(messages.en['app.welcome']);
    expect(wrapper.find(Counter)).toHaveLength(1);
    expect(wrapper.find(Counter).props()).toStrictEqual(appProps);
  });
  it('Accessibility test for the App component', async () => {
    const appProps: Props = {
      counterValue: 1,
      decrementCounter: jest.fn(),
      incrementCounter: jest.fn(),
      incrementCounterAsync: jest.fn(),
    };

    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <App
          counterValue={appProps.counterValue}
          decrementCounter={appProps.decrementCounter}
          incrementCounter={appProps.incrementCounter}
          incrementCounterAsync={appProps.incrementCounterAsync}
        />
      </IntlProvider>
    );
    const results = await axe(wrapper.getDOMNode(), {
      rules: {
        // These axe rules are fixed when app is run in the shell
        region: { enabled: false },
      },
    });
    expect(results).toHaveNoViolations();
  });
  it('App snapshot render test', () => {
    const appProps: Props = {
      counterValue: 1,
      decrementCounter: jest.fn(),
      incrementCounter: jest.fn(),
      incrementCounterAsync: jest.fn(),
    };

    const tree = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <App {...appProps} />
      </IntlProvider>
    );

    expect(
      toJson(tree, {
        noKey: false,
        mode: 'deep',
      })
    ).toMatchSnapshot();
  });
});
