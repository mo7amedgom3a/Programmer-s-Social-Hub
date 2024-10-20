import { useRouter } from "next/router";
import React from "react";
import RenderSavedPostsPage from "../../app/components/SavePage"; // Assuming RenderSavedPostsPage is in the components folder
import '../../app/globals.css';
const SavedPostsPage = () => {
  const router = useRouter();
  const token = localStorage.getItem("authToken");
  if (!token) {
    return <div>Error: No auth token found</div>;
  }
  const decodedToken = JSON.parse(atob(token.split('.')[1]));
  const userid = decodedToken.nameid;

  return <RenderSavedPostsPage userId={userid}/>;
};
export default SavedPostsPage;

