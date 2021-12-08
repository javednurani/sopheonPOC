import { DefaultButton, mergeStyleSets, Modal } from '@fluentui/react';
import { useBoolean } from '@fluentui/react-hooks';
import React from 'react';

import AddTask from './AddTask';

export interface IToDoListProps {}

const ToDoList: React.FunctionComponent<IToDoListProps> = ({}: IToDoListProps) => {
  const [isTaskModalOpen, { setTrue: showTaskModal, setFalse: hideTaskModal }] = useBoolean(false);

  const contentStyles = mergeStyleSets({
    container: {
      // display: 'flex',
      // flexFlow: 'column nowrap',
      // alignItems: 'stretch',
      height: '560px',
      width: '956px',
    },
  });

  return (
    <>
      <div>To Do List</div>
      <DefaultButton onClick={showTaskModal} text="Open Modal" />
      <Modal
        titleAriaId="TaskModal"
        isOpen={isTaskModalOpen}
        onDismiss={hideTaskModal}
        isBlocking={true}
        containerClassName={contentStyles.container}
      >
        <AddTask hideModal={hideTaskModal} />
      </Modal>
    </>
  );
};

export default ToDoList;
