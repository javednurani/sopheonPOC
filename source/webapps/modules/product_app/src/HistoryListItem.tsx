import { Link } from '@fluentui/react';
import { useTheme } from '@fluentui/react-theme-provider';
import React, { CSSProperties } from 'react';
import { useIntl } from 'react-intl';

import { ChangeEvent } from './data/changeEvents';
import { HistoryItem } from './types';

const HistoryListItem: React.FC<HistoryItem> = ({ event: changeEvent, item: fieldName, eventDate: date, previousValue }: HistoryItem) => {
  const { formatMessage } = useIntl();
  const theme = useTheme();
  const linkStyles: CSSProperties = {
    color: theme.palette.themePrimary,
  };
  const handleTogglePreviousValueClick = () => {
    alert('clicked');
    // const previousValueElement = event.target.parentElement.nextElementSibling;
    // if (previousValueElement.style.display === 'none') {
    //   previousValueElement.style.display = 'block';
    //   event.target.innerHTML = formatMessage({ id: 'history.hidePreviousValue' });
    // } else {
    //   previousValueElement.style.display = 'none';
    //   event.target.innerHTML = formatMessage({ id: 'history.showPreviousValue' });
    // }
  };

  return (
    <>
      <div>
        {ChangeEvent[changeEvent]} {fieldName}
      </div>
      <div>{date.toLocaleString()}</div>
      <div>
        <Link variant="xSmall" onClick={handleTogglePreviousValueClick} style={linkStyles}>
          {formatMessage({ id: 'history.showPreviousValue' })}
        </Link>
      </div>
      <div style={{ display: 'none' }}>{previousValue}</div>
    </>
  );
};

export default HistoryListItem;
