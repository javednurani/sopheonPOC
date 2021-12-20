import '../../../controls/ext/Gantt/codebase/dhtmlxgantt.css';
import '../../../controls/ext/Gantt/codebase/Gantt.css';

import React, { useEffect } from 'react';

import { Gantt } from '../../../controls/ext/Gantt/codebase/dhtmlxgantt';

export type GanttProps = {
  taskInfo: {
    data: [],
    links: []
  };
};

const GanttComponent: React.FunctionComponent<GanttProps> = ({ taskInfo }: GanttProps) => {
  useEffect(() => {
    const gantt = Gantt.getGanttInstance();

    gantt.plugins({
      auto_scheduling: true,
      click_drag: false,
      critical_path: true,
      drag_timeline: true,
      // fullscreen: true,
      grouping: true,
      // keyboard_navigation: true,
      marker: true,
      multiselect: false,
      overlay: false,
      quick_info: false,
      tooltip: true,
      undo: false,
    });
    const today = new Date();
    gantt.config.scales = [
      {unit: 'year', step: 1, format: '%Y'},
      {unit: 'month', step: 1, format: '%F'}
    ];

    gantt.addMarker({
      start_date: today, //a Date object that sets the marker's date
      css: 'today', //a CSS class applied to the marker
      text: 'Today', //the marker title
      title: today.toLocaleDateString() // the marker's tooltipd
    });
    gantt.templates.tooltip_text = function(start, end, task) {
      return `<b>Task:</b> ${task.text} <br/><b>Due Date:</b> ${gantt.templates.tooltip_date_format(start)}`;
    };
    gantt.config.readonly = true;
    gantt.config.show_grid = false;
    gantt.config.show_tasks_outside_timescale = true;

    if (taskInfo.data.length === 0) {
      gantt.config.start_date = new Date(today.getFullYear(), today.getMonth() - 1, today.getDate());
      gantt.config.end_date = new Date(today.getFullYear(), today.getMonth() + 1, today.getDate());
    }

    gantt.init('gantt_here');
    gantt.parse(taskInfo);
  });
  return (
    <div className="gantt_here" id="gantt_here"></div>
  );
};

export default GanttComponent;
