import { CommandButton, IIconProps, mergeStyleSets, Modal, Stack } from '@fluentui/react';
import { Text } from '@fluentui/react/lib/Text';
import { useBoolean } from '@fluentui/react-hooks';
import { Gantt, GanttMilestone, GanttTask } from '@sopheon/controls';
import React from 'react';
import { useIntl } from 'react-intl';

import MilestoneDialog from '../milestone/MilestoneDialog';
import { CreateMilestoneAction } from '../product/productReducer';
import { Milestone, PostMilestoneModel, Task } from '../types';

export interface ITimelineProps {
  accessToken: string;
  createMilestone: (milestone: PostMilestoneModel) => CreateMilestoneAction;
  environmentKey: string;
  milestones: Milestone[];
  productKey: string;
  tasks: Task[];
}

const Timeline: React.FunctionComponent<ITimelineProps> = ({
  accessToken,
  createMilestone,
  environmentKey,
  milestones,
  productKey,
  tasks,
}: ITimelineProps) => {
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
        <MilestoneDialog
          accessToken={accessToken}
          hideModal={hideMilestoneModal}
          createMilestone={createMilestone}
          environmentKey={environmentKey}
          productKey={productKey}
        />
      </Modal>
    </>
  );
};

export default Timeline;
