import { DefaultButton, mergeStyleSets, Modal } from '@fluentui/react';
import { useBoolean } from '@fluentui/react-hooks';
import React from 'react';

import AddToDoListItem from './AddToDoListItem';

export interface IToDoListProps {}

const ToDoList: React.FunctionComponent<IToDoListProps> = ({}: IToDoListProps) => {
  const [isModalOpen, { setTrue: showModal, setFalse: hideModal }] = useBoolean(false);

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
      <DefaultButton onClick={showModal} text="Open Modal" />
      <Modal
        titleAriaId="AddToDoListItemModal"
        isOpen={isModalOpen}
        onDismiss={hideModal}
        isBlocking={true}
        containerClassName={contentStyles.container}
      >
        <AddToDoListItem hideModal={hideModal} />
      </Modal>
    </>
  );
};

export default ToDoList;
