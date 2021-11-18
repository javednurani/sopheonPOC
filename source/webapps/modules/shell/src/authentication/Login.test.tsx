import { shallow } from 'enzyme';
import React from 'react';

import { azureSettings } from '../settings/azureSettings';
import AuthLanding from './AuthLanding';
import Login from './Login';

describe('Test Login component', () => {
  test('Renders AuthLanding with expected props.', () => {
    // Arrange
    const loginSpinnerResourceKey = 'authlanding.loginspinner';

    // Act
    const wrapper = shallow(<Login />);

    // Assert
    const authLanding = wrapper.find(AuthLanding);
    expect(authLanding).toHaveLength(1);
    expect(authLanding.prop('adB2cPolicyName')).toBe(azureSettings.AD_B2C_SignUpSignIn_Policy);
    expect(authLanding.prop('spinnerMessageResourceKey')).toBe(loginSpinnerResourceKey);
  });
});
