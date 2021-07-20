import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';
import { IntlProvider } from 'react-intl';

import NewUserLanding from './NewUserLanding';

expect.extend(toHaveNoViolations);

describe('Test NewUserLanding component', () => {
  it('NewUserLanding component renders and is a11y compliant', async () => {
    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <NewUserLanding />
      </IntlProvider>
    );
    const axeResults = await axe(wrapper.getDOMNode(), {
      rules: {
        // NewUserLanding page is correctly rendered in <body> region at runtime.
        region: { enabled: false },
      },
    });

    expect(wrapper.find('.ms-Spinner')).toHaveLength(1);
    expect(wrapper.find('.ms-Spinner-label').text()).toEqual(messages.en['newuserlanding.enteringflow']);
    expect(axeResults).toHaveNoViolations();
  });
});
