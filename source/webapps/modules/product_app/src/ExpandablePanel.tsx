import { FontIcon, Link, Text } from '@fluentui/react';
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

  const icon = <FontIcon iconName={expanded ? 'ChevronUpMed' : 'ChevronDownMed'} style={{ verticalAlign: 'middle' }} />;

  return (
    <>
      <Link onClick={expanded ? handleCollapseClick : handleExpandClick}>
        <Text variant="medium">
          {icon} {title}
        </Text>
      </Link>
      {expanded && children && <div>{children}</div>}
    </>
  );
};

export default ExpandablePanel;
