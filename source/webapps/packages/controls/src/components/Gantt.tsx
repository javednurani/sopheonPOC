import '../../../controls/ext/Gantt/codebase/dhtmlxgantt.css';
import '../../../controls/ext/Gantt/codebase/sopheon_dhtmlxgantt.css';
import '../../../controls/ext/Gantt/codebase/Gantt.css';

import React, { useEffect } from 'react';

import { Gantt } from '../../../controls/ext/Gantt/codebase/dhtmlxgantt';

interface DhtmlxGanttData {
  id: string;
  text: string;
  type: string; // could be any accordig to dxhtmlgantt
  // eslint-disable-next-line camelcase
  start_date: Date | null;
}

export interface GanttTask {
  id: string;
  text: string;
  date: Date | null;
}

// TODO: should these duplciate types be handled differently?
export interface GanttMilestone {
  id: string;
  text: string;
  date: Date | null;
}

export type GanttProps = {
  tasks: GanttTask[];
  milestones: GanttMilestone[];
};

const GanttComponent: React.FunctionComponent<GanttProps> = ({ tasks, milestones }: GanttProps) => {
  useEffect(() => {
    const gantt = Gantt.getGanttInstance();

    const taskData: DhtmlxGanttData[] = tasks.map(task => ({
      id: task.id,
      text: task.text,
      type: 'milestone',
      start_date: task.date,
    }));

    // const milestoneData: DhtmlxGanttData[] = milestones.map(m => ({
    //   id: m.id,
    //   text: m.text,
    //   type: 'milestone', // TODO: new type
    //   start_date: m.date,
    // }));

    const ganttdata = {
      data: [...taskData],
      links: [],
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
      { unit: 'year', step: 1, format: '%Y' },
      { unit: 'month', step: 1, format: '%F' },
    ];
    gantt.config.readonly = true;
    gantt.config.show_grid = false;
    gantt.config.show_tasks_outside_timescale = true;

    gantt.addMarker({
      start_date: today, //a Date object that sets the marker's date
      css: 'today', //a CSS class applied to the marker
      text: 'Today', //the marker title
      title: today.toLocaleDateString(), // the marker's tooltip
    });

    gantt.templates.tooltip_date_format = function (date) {
      const formatFunc = gantt.date.date_to_str('%m/%d/%y');
      return formatFunc(date);
    };

    gantt.templates.tooltip_text = function (start, end, task) {
      return `<b>Task:</b> ${task.text} <br/><b>Due Date:</b> ${gantt.templates.tooltip_date_format(start)}`;
    };

    if (ganttdata.data.length === 0) {
      gantt.config.start_date = new Date(today.getFullYear(), today.getMonth(), today.getDate());
      gantt.config.end_date = new Date(today.getFullYear(), today.getMonth() + 2, today.getDate());
    }

    gantt.init('gantt_here');
    gantt.parse(ganttdata);
  });

  return <div className="gantt_here" id="gantt_here"></div>;
};

export default GanttComponent;
