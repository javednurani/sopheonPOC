import Gantt from '@sopheon/controls/src/components/Gantt';
import { Stack } from 'office-ui-fabric-react';
import React from 'react';

export interface ITimelineProps {}

const Timeline: React.FunctionComponent<ITimelineProps> = ({}: ITimelineProps) => {
  const propData = {
    data: [
      { id: 1, text: 'Task #1', start_date: '15-11-2021', duration: 1, progress: 0.6 },
      { id: 2, text: 'Task #2', start_date: '18-01-2022', duration: 1, progress: 0.4 },
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
