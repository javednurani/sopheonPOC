import 'regenerator-runtime/runtime.js';
import './console-mock';
import './storage-mock';

import { configure } from 'enzyme';
import Adapter from 'enzyme-adapter-react-16';

configure({ adapter: new Adapter() });
