import { MessageBar, MessageBarType, Text } from '@fluentui/react';
import { HideAnnouncementAction, ShowAnnouncementModel } from '@sopheon/shell-api';
import React, { FunctionComponent, useEffect } from 'react';

export interface AnnouncementProps {
  hideAnnouncement: () => HideAnnouncementAction;
  announcementContent: ShowAnnouncementModel | null;
}

const Announcement: FunctionComponent<AnnouncementProps> = ({ hideAnnouncement, announcementContent }: AnnouncementProps) => {
  useEffect(() => {
    if (announcementContent) {
      setTimeout(() => hideAnnouncement(), announcementContent.durationSeconds * 1000);
    }
  }, []);

  if (announcementContent) {
    return (
      <>
        <MessageBar messageBarType={MessageBarType.success} isMultiline={false}>
          <Text variant="medium">{announcementContent.message}</Text>
        </MessageBar>
      </>
    );
  }
  return null;
};

export default Announcement;
