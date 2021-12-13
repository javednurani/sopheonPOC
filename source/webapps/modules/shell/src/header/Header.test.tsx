import { messages } from '@sopheon/shared-ui';
import { mount } from 'enzyme';
import React from 'react';
import { IntlProvider } from 'react-intl';
import { BrowserRouter } from 'react-router-dom';

import Navbar from '../navbar/Navbar';
import { AppModule, appModules } from '../settings/appModuleSettings';
import ThemeSelector from '../themes/components/themeSelector/ThemeSelector';
import { ChangeThemeAction } from '../themes/themeReducer/themeReducer';
import Header from './Header';

// Value that the useLocation hook returns
// This allows us to change the pathname between tests to hit all branches in CC
const mockUseLocationValue = {
  pathname: '/testPath',
  search: '',
  hash: '',
  state: null,
};
jest.mock('react-router-dom', () => ({
  ...(jest.requireActual('react-router-dom') as Record<string, never>),
  useLocation: jest.fn().mockImplementation(() => mockUseLocationValue),
}));

describe.skip('Header render tests', () => {
  // Enzyme mount tests all of the file
  // If we have tests that change the pathname and FetchStatus, we can get 100% coverage
  it('enzyme mount render test - bad path', () => {
    const mockChangeTheme = jest.fn() as () => ChangeThemeAction;
    mockUseLocationValue.pathname = '/pathWithNoApp';

    const wrapper = mount(
      <BrowserRouter>
        <IntlProvider locale="en" messages={messages.en}>
          <Header changeTheme={mockChangeTheme} />
        </IntlProvider>
      </BrowserRouter>
    );
    expect(wrapper.find('header')).toHaveLength(1);
    expect(wrapper.find('header').getDOMNode()).toHaveAttribute('role', 'banner');
    expect(wrapper.find(Navbar)).toHaveLength(1);
    expect(wrapper.find(ThemeSelector)).toHaveLength(1);
    //expect(wrapper.find('h1').text()).toBe(messages.en.defaultTitle);
  });
  it('enzyme mount render test - happy path', () => {
    const happyApp: AppModule = appModules[0];
    const mockChangeTheme = jest.fn() as () => ChangeThemeAction;
    mockUseLocationValue.pathname = happyApp.routeName;

    const wrapper = mount(
      <BrowserRouter>
        <IntlProvider locale="en" messages={messages.en}>
          <Header changeTheme={mockChangeTheme} />
        </IntlProvider>
      </BrowserRouter>
    );

    expect(wrapper.find('header')).toHaveLength(1);
    expect(wrapper.find('header').getDOMNode()).toHaveAttribute('role', 'banner');
    expect(wrapper.find(Navbar)).toHaveLength(1);
    expect(wrapper.find(ThemeSelector)).toHaveLength(1);
    //expect(wrapper.find('h1').text()).toBe(messages.en[happyApp.displayNameResourceKey]);
  });
});
