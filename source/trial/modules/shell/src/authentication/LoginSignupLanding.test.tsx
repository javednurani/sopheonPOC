import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import { axe, toHaveNoViolations } from 'jest-axe';
import React from 'react';
import { IntlProvider } from 'react-intl';

import LoginSignupLanding, { LandingMode } from './LoginSignupLanding';

expect.extend(toHaveNoViolations);

describe('Test LoginSignupLanding component', () => {
  it('LoginSignupLanding component in Login LandingMode renders and is a11y compliant', async () => {
    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <LoginSignupLanding landingMode={LandingMode.Login} />
      </IntlProvider>
    );
    const axeResults = await axe(wrapper.getDOMNode(), {
      rules: {
        // LoginSignupLanding page is correctly rendered in <body> region at runtime.
        region: { enabled: false },
      },
    });

    expect(wrapper.find('.ms-Spinner')).toHaveLength(1);
    expect(wrapper.find('.ms-Spinner-label').text()).toEqual(messages.en['loginsignuplanding.loginspinner']);
    expect(axeResults).toHaveNoViolations();
  });
  it('LoginSignupLanding component in Signup LandingMode renders and is a11y compliant', async () => {
    const wrapper = mount(
      <IntlProvider locale="en" messages={messages.en}>
        <LoginSignupLanding landingMode={LandingMode.Signup} />
      </IntlProvider>
    );
    const axeResults = await axe(wrapper.getDOMNode(), {
      rules: {
        // LoginSignupLanding page is correctly rendered in <body> region at runtime.
        region: { enabled: false },
      },
    });

    expect(wrapper.find('.ms-Spinner')).toHaveLength(1);
    expect(wrapper.find('.ms-Spinner-label').text()).toEqual(messages.en['loginsignuplanding.signupspinner']);
    expect(axeResults).toHaveNoViolations();
  });
});
