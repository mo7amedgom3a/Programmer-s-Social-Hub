"use client"

import React, { useState, useEffect, useCallback } from "react";
import { useInView } from "react-intersection-observer";
import { Button } from "../components/ui/button";
import { Loader2Icon } from 'lucide-react';
import { CreatePost } from './CreatePost';
import PostComponent from './Post';
import { Navbar } from "./Navbar";

interface userMetadata {
  username: string;
  bio: string;
  userId: string;
  name : string;
  imageUrl: string;
}
interface Post {
  id: string;
  content: string;
  title: string;
  language: string;
  userMetadata: userMetadata;
  code: string;
  images: string[];
  likes: number;
  comments: string[];
  saved: boolean;
  liked: boolean;
  showComments: boolean;
  createdAt: string;
  updatedAt: string; 
}

const Home: React.FC = () => {
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);
  const [ref, inView] = useInView();
  const [newPost, setNewPost] = useState("");
  const [newPostCode, setNewPostCode] = useState("");
  const [title, setTitle] = useState("");
  const [posts, setPosts] = useState<Post[]>([]);

  const list_lang = [
    "c", "c#", "cpp", "java" , "React", "Assembly", "python", "javascript", "typescript",
    "html", "css", "shell", "powershell", "sql", "json", "yaml", "xml",
    "markdown", "plaintext"
  ];

  const getToken = () => {
    return localStorage.getItem('authToken');
  };

  const getUserIdFromToken = (token: string) => {
    const decodedToken = JSON.parse(atob(token.split('.')[1]));
    return decodedToken.nameid;
  };

  const loadMorePosts = useCallback(async () => {
    const token = getToken();
    if (!token || loading || !hasMore) return;
    setLoading(true);
    try {
      const response = await fetch(`http://localhost:5108/api/Post?page=${page}&pageSize=10`, {
        headers: {
          'accept': '*/*',
          'Authorization': `Bearer ${token}`,
        },
      });
      if (response.ok) {
        const data = await response.json();
        console.log(data);
        setPosts((prevPosts) => [
          ...prevPosts,
          ...data.posts.map((post: Post) => ({
            id: post.id,
            title: post.title,
            content: post.content,
            userMetadata: post.userMetadata,
            comments : post.comments,
            code: post.code,
            language: post.language,
            images: post.images || [],
            likes: post.likes,
            saved: false,
            liked: false,
            showComments: false,
            createdAt: post.createdAt,
            updatedAt: post.updatedAt,
          })),
        ]);
        setPage((prevPage) => prevPage + 1);
        setHasMore(data.currentPage < data.totalPages);
      } else {
        console.error('Failed to fetch posts:', response.statusText);
      }
    } catch (error) {
      console.error('Error fetching posts:', error);
    } finally {
      setLoading(false);
    }
  }, [loading, hasMore, page]);

  useEffect(() => {
    if (inView) {
      loadMorePosts();
    }
  }, [inView, loadMorePosts]);

  const handlePostSubmit = async ({
    title,
    content,
    code,
    language,
    images,
  }: {
    title: string;
    content: string;
    code: string;
    language: string;
    images: string[];
  }) => {
    if (!content.trim() || !title.trim()) {
      console.error('Post content or title cannot be empty');
      return;
    }

    const token = getToken();
    if (!token) {
      console.error('No auth token found');
      return;
    }

    const authorId = getUserIdFromToken(token);

    const newPostData = {
      authorId,
      title,
      content,
      code,
      language,
      images,
    };

    try {
      const response = await fetch('http://localhost:5108/api/Post', {
        method: 'POST',
        headers: {
          'accept': '*/*',
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newPostData),
      });

      if (response.ok) {
        const data = await response.json();
        console.log(data);
        setPosts([
          {
            id: data.id,
            userMetadata: data.userMetadata,
            title: data.title,
            content: data.content,
            code: data.code,
            language: data.language,
            images: data.images,
            likes: data.likes,
            comments: data.CommentIds || [],
            saved: false,
            liked: false,
            showComments: false,
            createdAt: data.createdAt,
            updatedAt: data.updatedAt,
          },
          ...posts,
        ]);

        // Clear input fields after successful post creation
        setNewPost("");
        setNewPostCode("");
        setTitle("");
      } else {
        console.error('Failed to create post:', response.statusText);
      }
    } catch (error) {
      console.error('Error creating post:', error);
    } finally {
      setLoading(false); // Reset loading state after request completion
    }
  };

  return (
    <div className="space-y-6">
      <CreatePost
        newPost={newPost}
        newPostCode={newPostCode}
        setNewPost={setNewPost}
        setNewPostCode={setNewPostCode}
        onPostSubmit={handlePostSubmit}
        list_lang={list_lang}
      />
      {posts.map((post) => (
        <PostComponent key={post.id} post={post} />
      ))}
      {hasMore && (
        <div ref={ref} className="flex justify-center p-4">
          {loading ? (
            <Loader2Icon className="w-6 h-6 animate-spin" />
          ) : (
            <Button onClick={loadMorePosts}>Load More</Button>
          )}
        </div>
      )}
    </div>
  );
};

export default Home;
