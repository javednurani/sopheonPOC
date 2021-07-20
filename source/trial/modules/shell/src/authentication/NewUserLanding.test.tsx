import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';
import { IntlProvider } from 'react-intl';

import { randomString } from '../testUtils';
import NewUserLanding from './NewUserLanding';

expect.extend(toHaveNoViolations);

describe('Test NewUserLanding component', () => {
  it('NewUserLanding component with Login Props renders and is a11y compliant', async () => {
    const policyName = randomString();
    const resourceKey = 'newuserlanding.loginspinner';
    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <NewUserLanding adB2cPolicyName={policyName} spinnerMessageResourceKey={resourceKey} />
      </IntlProvider>
    );
    const axeResults = await axe(wrapper.getDOMNode(), {
      rules: {
        // NewUserLanding page is correctly rendered in <body> region at runtime.
        region: { enabled: false },
      },
    });

    expect(wrapper.find('.ms-Spinner')).toHaveLength(1);
    expect(wrapper.find('.ms-Spinner-label').text()).toEqual(messages.en[resourceKey]);
    expect(axeResults).toHaveNoViolations();
  });
  it('NewUserLanding component with Signup Props renders and is a11y compliant', async () => {
    const policyName = randomString();
    const resourceKey = 'newuserlanding.signupspinner';
    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <NewUserLanding adB2cPolicyName={policyName} spinnerMessageResourceKey={resourceKey} />
      </IntlProvider>
    );
    const axeResults = await axe(wrapper.getDOMNode(), {
      rules: {
        // NewUserLanding page is correctly rendered in <body> region at runtime.
        region: { enabled: false },
      },
    });

    expect(wrapper.find('.ms-Spinner')).toHaveLength(1);
    expect(wrapper.find('.ms-Spinner-label').text()).toEqual(messages.en[resourceKey]);
    expect(axeResults).toHaveNoViolations();
  });
});
