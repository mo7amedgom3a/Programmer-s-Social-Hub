"use client";

import React, { useEffect, useState } from "react";
import PostComponent from "./Post";
import { Card, CardContent, CardHeader } from "./ui/card";

interface UserMetadata {
  userId: string;
  bio: string;
  username: string;
  name : string;
  imageUrl: string;
}

interface Post {
  id: string;
  content: string;
  title: string;
  language: string;
  userMetadata: UserMetadata;
  code: string;
  images: string[];
  likes: number;
  comments: string[];
  saved: boolean;
  showComments: boolean;
  liked: boolean;
  createdAt: string;
  updatedAt: string;
}

interface SavedPost {
  id: string;
  postDto: Post;
  userId: string;
  savedAt: string;
}

const RenderSavedPostsPage = ({ userId }:{userId:any}) => {
  const [posts, setPosts] = useState<Post[]>([]);
  const token = localStorage.getItem("authToken");
  const fetchSavedPosts = async () => {
    try {
      const response = await fetch(
        `http://localhost:5108/api/Post/saved/${userId}`,
        {
          method: "GET",
          headers: {
            Accept: "/",
            Authorization: `Bearer ${token}`,
          },
        }
      );
      const data = await response.json();
      console.log(data);
      const formattedPosts = data.map((savedPost: SavedPost) => ({
        id: savedPost.postDto.id,
        content: savedPost.postDto.content,
        title: savedPost.postDto.title,
        language: savedPost.postDto.language,
        userMetadata: savedPost.postDto.userMetadata,
        code: savedPost.postDto.code,
        images: savedPost.postDto.images,
        likes: savedPost.postDto.likes,
        comments: savedPost.postDto.comments,
        saved: true,
        showComments: false,
        liked: false,
        createdAt: savedPost.postDto.createdAt,
        updatedAt: savedPost.postDto.updatedAt,
      }));
      setPosts(formattedPosts);
    } catch (error) {
      console.error("Error fetching saved posts:", error);
    }
  };

  useEffect(() => {
    fetchSavedPosts();
  }, [userId]);

  return (
    <div className="space-y-4">
      <Card>
        <CardHeader>
          <h2 className="text-2xl font-bold">Saved Posts</h2>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            {posts.map((post) => (
              <PostComponent key={post.id} post={post} />
            ))}
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default RenderSavedPostsPage;
