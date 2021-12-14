import '../../../controls/ext/Gantt/codebase/dhtmlxgantt.css';
import './Gantt.css';

import { useEffect } from '@storybook/addons';
import React from 'react';

import { Gantt } from '../../../controls/ext/Gantt/codebase/dhtmlxgantt';

export type GanttProps = {
  taskInfo?: any;
};

const GanttComponent: React.FunctionComponent<GanttProps> = ({ taskInfo }: GanttProps) => {
  useEffect(() => {
    const gantt = Gantt.getGanttInstance();

    gantt.plugins({
      auto_scheduling: true,
      click_drag: true,
      critical_path: true,
      drag_timeline: true,
      // fullscreen: true,
      grouping: true,
      // keyboard_navigation: true,
      marker: true,
      multiselect: true,
      overlay: true,
      quick_info: true,
      tooltip: true,
      undo: true,
    });


    // Just hard coding some data for now.
    const taskData = {
      data: [
        { id: 1, text: 'Task #1', start_date: '15-11-2021', duration: 1, progress: 0.6 },
        { id: 2, text: 'Task #2', start_date: '18-01-2022', duration: 1, progress: 0.4 },
      ],
      links: [],
    };

    // @ts-expect-error: Let's ignore a compil
    const { ganttContainer } = this;

    // , new Date(2018, 3, 30), new Date(2020, 3, 30)
    gantt.init(ganttContainer);
    gantt.parse(taskData);
  });
  return (
    <div
      ref={input => {
        // @ts-expect-error: Let's ignore a compil
        this.ganttContainer = input;
      }}
      style={{}}
    ></div>
  );
};

export default GanttComponent;
