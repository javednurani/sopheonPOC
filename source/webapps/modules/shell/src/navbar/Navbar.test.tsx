import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import React from 'react';
import { IntlProvider } from 'react-intl';
import { BrowserRouter, Link } from 'react-router-dom';

import { appModules } from '../settings/appModuleSettings';
import Navbar from './Navbar';

describe('Test Navbar component', () => {
  it('Test navbar render', () => {
    const appCount = appModules.length;
    const wrapper = mount(
      <BrowserRouter>
        <IntlProvider locale="en" messages={messages.en}>
          <Navbar />
        </IntlProvider>
      </BrowserRouter>
    );
    expect(wrapper.find('nav')).toHaveLength(1);
    expect(wrapper.find('nav').getDOMNode()).toHaveAttribute('role', 'navigation');
    expect(wrapper.find(Link)).toHaveLength(appCount);
    wrapper.find(Link).forEach((link, index) => {
      expect(link.props().to).toBe(appModules[index].routeName);
    });
  });
});
