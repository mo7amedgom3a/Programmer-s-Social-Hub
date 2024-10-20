import { useRouter } from "next/router";
import React from "react";
import RenderSettingsPage from "../../app/components/settings"; // Assuming RenderSettingsPage is in the components folder
import '../../app/globals.css';
const SettingsPage = () => {
  const router = useRouter();
  const { userId } = router.query;

  return <RenderSettingsPage userId= {userId as string} />;
};

export default SettingsPage;