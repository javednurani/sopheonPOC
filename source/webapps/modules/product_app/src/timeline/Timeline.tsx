import Gantt from '@sopheon/controls/src/components/Gantt';
import { Stack } from 'office-ui-fabric-react';
import React from 'react';

export interface ITimelineProps {}

const Timeline: React.FunctionComponent<ITimelineProps> = ({}: ITimelineProps) => {
  const propData = {
    data: [
      { id: 1, text: 'Alpha release', type: gantt.config.types.milestone,
        start_date: new Date(2021, 10, 12) },
      { id: 2, text: 'beta release', type: gantt.config.types.milestone,
        start_date: new Date(2021, 12, 12) },
      { id: 3, text: 'charlie release', type: gantt.config.types.milestone,
        start_date: new Date(2022, 10, 12) }
    ],
    links: [],
  };
  const mainStackStyle: React.CSSProperties = {
    width: '100%',
    height: '100%',
    marginLeft: '10px',
    marginRight: '10px',
    marginTop: '10px',
    paddingBottom: '20px',
  };

  return (
    <Stack style={mainStackStyle}>
      <Gantt taskInfo={propData}></Gantt>
    </Stack>
  );
};

export default Timeline;
