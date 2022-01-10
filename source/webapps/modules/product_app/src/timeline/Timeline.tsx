import { CommandButton, IIconProps, mergeStyleSets, Modal, Stack } from '@fluentui/react';
import { Text } from '@fluentui/react/lib/Text';
import { useBoolean } from '@fluentui/react-hooks';
import { Gantt, GanttMilestone, GanttTask } from '@sopheon/controls';
import React from 'react';
import { useIntl } from 'react-intl';

import MilestoneDialog from '../milestone/MilestoneDialog';
import { CreateMilestoneAction, UpdateProductAction } from '../product/productReducer';
import { Milestone, PostMilestoneModel, Task, UpdateProductModel } from '../types';

export interface ITimelineProps {
  tasks: Task[];
  milestones: Milestone[];
  createMilestone: (milestone: PostMilestoneModel) => CreateMilestoneAction;
  updateProduct: (product: UpdateProductModel) => UpdateProductAction;
}

const Timeline: React.FunctionComponent<ITimelineProps> = ({ tasks, milestones, createMilestone, updateProduct }: ITimelineProps) => {
  const [isMilestoneModalOpen, { setTrue: showMilestoneDialog, setFalse: hideMilestoneModal }] = useBoolean(false);
  const { formatMessage } = useIntl();

  const ganttTasks: GanttTask[] = tasks.map(task => ({
    id: `${task.name}_${task.dueDate}`,
    text: task.name,
    date: task.dueDate,
  }));

  const ganttMilestones: GanttMilestone[] = milestones.map(m => ({
    id: `${m.name}_${m.date}`,
    text: m.name,
    date: m.date,
  }));

  const mainStackStyle: React.CSSProperties = {
    width: '100%',
    height: '98%',
    marginLeft: '10px',
    marginRight: '10px',
    marginTop: '10px',
    paddingBottom: '10px',
  };

  const addMilestoneButtonStyles: React.CSSProperties = {
    display: 'flex',
    marginLeft: 'auto',
    cursor: 'pointer',
  };

  // milestoneDetails modal style
  const milestoneDetailsModalStyles = mergeStyleSets({
    container: {
      height: '480px',
      width: '600px',
      display: 'flex',
      padding: '32px',
      border: '2px',
      borderRadius: '8px',
    },
  });

  const handleCreateMilestoneIconClick = () => {
    showMilestoneDialog();
  };

  const addIcon: IIconProps = { iconName: 'CirclePlus' };

  return (
    <>
      <Stack style={mainStackStyle}>
        <Text variant="xLarge">
          <CommandButton
            style={addMilestoneButtonStyles}
            onClick={handleCreateMilestoneIconClick}
            iconProps={addIcon}
            text={formatMessage({ id: 'milestone.newmilestone' })}
          />
        </Text>
        <Gantt tasks={ganttTasks} milestones={ganttMilestones}></Gantt>
      </Stack>
      <Modal
        titleAriaId="Modal"
        isOpen={isMilestoneModalOpen}
        onDismiss={hideMilestoneModal}
        isBlocking={true}
        containerClassName={milestoneDetailsModalStyles.container}
      >
        <MilestoneDialog hideModal={hideMilestoneModal} updateProduct={updateProduct} environmentKey="" accessToken="" />
      </Modal>
    </>
  );
};

export default Timeline;
