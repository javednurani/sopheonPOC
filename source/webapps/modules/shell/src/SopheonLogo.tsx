import { useTheme } from '@fluentui/react-theme-provider';
import { darkTheme } from '@sopheon/shared-ui';
import React, { FunctionComponent, useEffect, useState } from 'react';

import { ReactComponent as SopheonLogoLight } from './images/sopheon_logo_blk_txt.svg';
import { ReactComponent as SopheonLogoDark } from './images/sopheon_logo_wht_txt.svg';
//TODO MOVE DUPLICATED COMPONENT HERE AND PRODUCT_APP TO SHARED-UI PACKAGE AND FIX SVG ISSUE (also remove duplicated images)
interface SopheonLogoProps {
  style: React.CSSProperties;
}

const SopheonLogo: FunctionComponent<SopheonLogoProps> = ({ style }: SopheonLogoProps): JSX.Element => {
  const theme = useTheme();
  const [darkThemeState, setdarkThemeState] = useState(false);

  useEffect(() => {
    if (theme.id?.includes(darkTheme.id ?? 'darkTheme')) {
      setdarkThemeState(true);
    } else {
      setdarkThemeState(false);
    }
  }, [style, theme]);

  if (darkThemeState) {
    return <SopheonLogoDark style={style} />;
  }
  return <SopheonLogoLight style={style} />;
};

export default SopheonLogo;
