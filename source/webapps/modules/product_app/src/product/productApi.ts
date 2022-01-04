import axios from 'axios';

import { ChangeEvent } from '../data/changeEvents';
import { Status } from '../data/status';
import { settings } from '../settings';
import { HistoryItem, TaskChangeEventDto } from '../types';

export type IProductApi = {
  getTaskHistory: (environmentKey: string, accessToken: string, productKey: string, taskId: number) => Promise<HistoryItem[]>;
};

const getTaskHistoryInternal = (environmentKey: string, accessToken: string, productKey: string, taskId: number): Promise<HistoryItem[]> =>
  new Promise<HistoryItem[]>((resolve, reject) => {
    axios
      .get(`${settings.ProductManagementApiUrlBase}/environments/${environmentKey}/products/${productKey}/tasks/${taskId}/history`, {
        headers: {
          Authorization: `Bearer ${accessToken}`,
        },
      })
      .then(response => {
        const historyItems: HistoryItem[] = processTaskChangeEvents(response.data);
        resolve(historyItems);
      })
      .catch(response => {
        resolve([]);
      });
  });

const processTaskChangeEvents = (taskChangeEvents: TaskChangeEventDto[]): HistoryItem[] => {
  const taskHistories: HistoryItem[] = [];

  taskChangeEvents.forEach((taskChange: TaskChangeEventDto) => {
    if (taskChange.entityChangeEventType === ChangeEvent.Updated) {
      const preTask = taskChange.preValue;
      const newLocal = taskChange.postValue;

      if (preTask.name !== newLocal.name) {
        taskHistories.push({
          event: taskChange.entityChangeEventType,
          eventDate: new Date(taskChange.timestamp),
          item: 'name',
          previousValue: preTask.name,
        });
      }

      if (preTask.notes !== newLocal.notes) {
        taskHistories.push({
          event: taskChange.entityChangeEventType,
          eventDate: new Date(taskChange.timestamp),
          item: 'toDo.notes',
          previousValue: preTask.notes,
        });
      }

      if (preTask.status !== newLocal.status) {
        taskHistories.push({
          event: taskChange.entityChangeEventType,
          eventDate: new Date(taskChange.timestamp),
          item: 'status',
          previousValue: Status[preTask.status as number],
        });
      }

      if (preTask.dueDate !== newLocal.dueDate) {
        taskHistories.push({
          event: taskChange.entityChangeEventType,
          eventDate: new Date(taskChange.timestamp),
          item: 'toDo.duedate',
          previousValue: preTask.dueDate ? new Date(preTask.dueDate) : null, // INFO, cannot currently remove dates in UI, null value shouldn't be hit here
        });
      }
    } else {
      taskHistories.push({
        event: taskChange.entityChangeEventType,
        eventDate: new Date(taskChange.timestamp),
        item: null, // no property to update for these events (i.e. created, deleted)
        previousValue: null,
      });
    }
  });

  return taskHistories;
};

const ProductApi: IProductApi = {
  getTaskHistory: getTaskHistoryInternal,
};

export default ProductApi;
