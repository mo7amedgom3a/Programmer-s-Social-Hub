import { useRouter } from "next/router";
import React from "react";
import RenderProfilePage from "../../app/components/UserPage"; // Assuming RenderProfilePage is in the components folder
import '../../app/globals.css';
const ProfilePage = () => {
  const router = useRouter();
  const { userId } = router.query;

  if (typeof userId !== 'string') {
    return null; // or handle the case where userId is not a string
  }

  return <RenderProfilePage userId={userId} />;
};
export default ProfilePage;
