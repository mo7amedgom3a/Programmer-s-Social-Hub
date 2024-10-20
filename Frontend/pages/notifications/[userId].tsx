import React from "react";
import RenderNotificationsPage from "../../app/components/notification"; // Assuming RenderNotificationsPage is in the components folder
import '../../app/globals.css';
import { useRouter } from "next/router";
const NotificationsPage = () => {
  const router = useRouter();
  const { userId } = router.query;
  return <RenderNotificationsPage userId={userId as string} />;
};
export default NotificationsPage;

