import { Link } from '@fluentui/react';
import React, { useState } from 'react';

export interface IExpandablePanelProps {
  title: string;
  onExpand?: () => void;
  onCollapse?: () => void;
}

const ExpandablePanel: React.FC<IExpandablePanelProps> = ({
  children,
  title,
  onExpand,
  onCollapse,
}: React.PropsWithChildren<IExpandablePanelProps>) => {
  const [expanded, setExpanded] = useState(false);

  const handleExpandClick = () => {
    setExpanded(true);
    onExpand && onExpand();
  };

  const handleCollapseClick = () => {
    setExpanded(false);
    onCollapse && onCollapse();
  };

  return (
    <>
      <Link variant="large" onClick={expanded ? handleCollapseClick : handleExpandClick}>
        {title}
      </Link>
      {expanded && children && <div>{children}</div>}
    </>
  );
};

export default ExpandablePanel;
