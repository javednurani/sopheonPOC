import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';
import { IntlProvider } from 'react-intl';

import { randomString } from '../testUtils';
import AuthLanding from './AuthLanding';

expect.extend(toHaveNoViolations);

describe('Test AuthLanding component', () => {
  it('AuthLanding component with Login Props renders and is a11y compliant', async () => {
    const policyName = randomString();
    const resourceKey = 'authlanding.loginspinner';
    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <AuthLanding adB2cPolicyName={policyName} spinnerMessageResourceKey={resourceKey} />
      </IntlProvider>
    );
    const axeResults = await axe(wrapper.getDOMNode(), {
      rules: {
        // AuthLanding page is correctly rendered in <body> region at runtime.
        region: { enabled: false },
      },
    });

    expect(wrapper.find('.ms-Spinner')).toHaveLength(1);
    expect(wrapper.find('.ms-Spinner-label').text()).toEqual(messages.en[resourceKey]);
    expect(axeResults).toHaveNoViolations();
  });
  it('AuthLanding component with Signup Props renders and is a11y compliant', async () => {
    const policyName = randomString();
    const resourceKey = 'authlanding.signupspinner';
    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <AuthLanding adB2cPolicyName={policyName} spinnerMessageResourceKey={resourceKey} />
      </IntlProvider>
    );
    const axeResults = await axe(wrapper.getDOMNode(), {
      rules: {
        // AuthLanding page is correctly rendered in <body> region at runtime.
        region: { enabled: false },
      },
    });

    expect(wrapper.find('.ms-Spinner')).toHaveLength(1);
    expect(wrapper.find('.ms-Spinner-label').text()).toEqual(messages.en[resourceKey]);
    expect(axeResults).toHaveNoViolations();
  });
});
