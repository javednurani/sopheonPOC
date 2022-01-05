import '../../../controls/ext/Gantt/codebase/dhtmlxgantt.css';
import '../../../controls/ext/Gantt/codebase/sopheon_dhtmlxgantt.css';
import '../../../controls/ext/Gantt/codebase/Gantt.css';

import React, { useEffect } from 'react';

import { Gantt } from '../../../controls/ext/Gantt/codebase/dhtmlxgantt';

export type GanttProps = {
  todoItems: {
    id: string;
    text: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    type: any;
    // eslint-disable-next-line camelcase
    start_date: Date | null;
  }[];
};

const GanttComponent: React.FunctionComponent<GanttProps> = ({ todoItems }: GanttProps) => {
  useEffect(() => {
    const gantt = Gantt.getGanttInstance();
    const tasksData = {
      data: todoItems,
      links: []
    };
    const today = new Date();

    gantt.plugins({
      auto_scheduling: true,
      click_drag: false,
      drag_timeline: true,
      grouping: true,
      marker: true,
      multiselect: false,
      overlay: false,
      quick_info: false,
      tooltip: true,
      undo: false,
    });

    gantt.config.scales = [
      {unit: 'year', step: 1, format: '%Y'},
      {unit: 'month', step: 1, format: '%F'}
    ];
    gantt.config.readonly = true;
    gantt.config.show_grid = false;
    gantt.config.show_tasks_outside_timescale = true;

    gantt.addMarker({
      start_date: today, //a Date object that sets the marker's date
      css: 'today', //a CSS class applied to the marker
      text: 'Today', //the marker title
      title: today.toLocaleDateString() // the marker's tooltip
    });

    gantt.templates.tooltip_date_format = function(date) {
      const formatFunc = gantt.date.date_to_str('%m/%d/%y');
      return formatFunc(date);
    };

    gantt.templates.tooltip_text = function(start, end, task) {
      return `<b>Task:</b> ${task.text} <br/><b>Due Date:</b> ${gantt.templates.tooltip_date_format(start)}`;
    };

    if (tasksData.data.length === 0) {
      gantt.config.start_date = new Date(today.getFullYear(), today.getMonth(), today.getDate());
      gantt.config.end_date = new Date(today.getFullYear(), today.getMonth() + 2, today.getDate());
    }

    gantt.init('gantt_here');
    gantt.parse(tasksData);
  });

  return (
    <div className="gantt_here" id="gantt_here"></div>
  );
};

export default GanttComponent;
