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
      // Type is always set to milestone so dhtmlxgantt uses the milestone renderer
      type: 'milestone',
      start_date: task.date,
      // The sopheon type is used to customize the plotted icon
      sopheonType: 'sopheonTask',
    }));

    const milestoneData: DhtmlxGanttData[] = milestones.map(m => ({
      id: m.id,
      text: m.text,
      // Type is always set to milestone so dhtmlxgantt uses the milestone renderer
      type: 'milestone',
      start_date: m.date,
      // The sopheon type is used to customize the plotted icon
      sopheonType: 'sopheonMilestone',
    }));

    const ganttdata = {
      data: [...taskData, ...milestoneData],
      links: [],
    };
    const today = new Date();

    gantt.config.type_renderers[gantt.config.types.milestone] = function (task, defaultRender) {
      const mainEl = document.createElement('div');
      mainEl.setAttribute(gantt.config.task_attribute, task.id);
      const size = gantt.getTaskPosition(task, task.start_date, task.end_date);
      if (task.sopheonType === 'sopheonMilestone') {
        mainEl.innerHTML = `<div class='sopheon_milestone'></div>`;
      } else {
        mainEl.innerHTML = `<div class='sopheon_task'></div>`;
      }

      mainEl.className = 'gantt_task_content';

      mainEl.style.left = `${size.left - 12}px`;
      mainEl.style.top = `${size.top + 7}px`;
      // Hardcode the width since there's no end date to go to for milestones
      // Needs to be the width of the icons you're using
      mainEl.style.width = `24px`;

      return mainEl;
    };

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
