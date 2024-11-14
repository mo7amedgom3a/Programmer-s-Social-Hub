import React from "react";
import RenderSavedPostsPage from "../../app/components/SavePage";
import '../../app/globals.css';
const SavedPostsPage = () => {
  const token = localStorage.getItem("authToken");
  if (!token) {
    return <div>Error: No auth token found</div>;
  }
  const decodedToken = JSON.parse(atob(token.split('.')[1]));
  const userid = decodedToken.nameid;

  return <RenderSavedPostsPage userId={userid}/>;
};
export default SavedPostsPage;

