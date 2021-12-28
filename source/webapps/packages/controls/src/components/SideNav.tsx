import { INavLinkGroup, INavStyleProps, INavStyles, IStyleFunctionOrObject, Nav } from '@fluentui/react';
import React from 'react';

export type SideBarProps = {
  menuItems: INavLinkGroup[];
  selectedMenuKey: string;
};

const sideBarStyles: IStyleFunctionOrObject<INavStyleProps, INavStyles> = {
  root: {
    width: '150px',
    height: '100%',
    boxSizing: 'border-box',
    backgroundColor: '#eAeef0',
    overflowY: 'auto',
  },
};

const SideNav: React.FC<SideBarProps> = ({ menuItems, selectedMenuKey }): JSX.Element => (
  <Nav selectedKey={selectedMenuKey} ariaLabel="SideBar" styles={sideBarStyles} groups={menuItems} />
);

export default SideNav;
