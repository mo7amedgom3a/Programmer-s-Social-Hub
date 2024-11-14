"use client";

import React, { useEffect, useState, useCallback } from "react";
import { useInView } from "react-intersection-observer";
import '../globals.css';
import {
  Card,
  CardContent,
  CardHeader,
} from "./ui/card";
import { Avatar, AvatarFallback, AvatarImage } from "../components/ui/avatar";
import { Badge } from "../components/ui/badge";
import PostComponent from "./Post";
import { Button } from "./ui/button";
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "./ui/dialog";
import { useRouter } from "next/navigation";

// interface Comment {
//   id: number;
//   author: string;
//   content: string;
// }
interface userMetadata {
  username: string;
  bio: string;
  userId: string;
  name: string;
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
  showComments: boolean;
  liked: boolean;
  createdAt: string;
  updatedAt: string; 
}

interface User {
  userId: string;
  imageUrl: string;
  name: string;
  username: string;
  bio: string;
  skills: string[];
  topicsOfInterest: string[];
  followers: any[];
  following: any[];
  followersCount: number;
  followingCount: number;
  _links: {
    self: string;
    update: string;
    delete: string;
  };
}

const RenderProfilePage = ({ userId }: {userId: string}) => {

  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);
  const [ref, inView] = useInView();
  const [user, setUser] = useState<User | null>(null);
  const [posts, setPosts] = useState<Post[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [popupContent, setPopupContent] = useState<"followers" | "following" | null>(null);
  const [followers, setFollowers] = useState<any[]>([]);
  const [following, setFollowing] = useState<any[]>([]);
  const [CurrentUserId, setCurrentUserId] = useState('');
  const [isCurrentUser, setIsCurrentUser] = useState(false);
  const router = useRouter();

  useEffect(() => {
    const token = localStorage.getItem('authToken');
    if (!token) {
      window.location.href = '/login';
    } else {
      const decodedToken = JSON.parse(atob(token.split('.')[1]));
      setCurrentUserId(decodedToken.nameid);
      setIsCurrentUser(decodedToken.nameid === userId);
    }
  }, [userId]);
  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (!token) {
      // Redirect to login page if not authenticated
      window.location.href = '/login';
    }
    const fetchUserData = async () => {
      try {
        const response = await fetch(`http://localhost:5172/api/User/${userId}`, {
          headers: {
            'Authorization': `Bearer ${token}`,
            'accept': '*/*'
          }
        });
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        const data: User = await response.json();
        setUser(data);
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    };

    fetchUserData();
  }, [userId]);

  const fetchUserPosts = useCallback(async () => {
    if (loading || !hasMore) return;
    setLoading(true);
    try {
      const response = await fetch(`http://localhost:5108/api/Post/user/posts/${userId}?page=${page}&pageSize=10`, {
        headers: {
          'accept': '*/*',
          'Authorization': `Bearer ${localStorage.getItem("authToken")}`,
        },
      });
      if (response.ok) {
        const data = await response.json();
        setPosts((prevPosts) => [
          ...prevPosts,
          ...data.posts.map((post: Post) => ({
            id: post.id,
            title: post.title,
            content: post.content,
            userMetadata: post.userMetadata,
            comments: post.comments,
            code: post.code,
            language: post.language,
            images: post.images || [],
            likes: post.likes,
            saved: post.saved,
            liked: post.liked,
            showComments: post.showComments,
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
  }, [loading, hasMore, page, userId]);

  useEffect(() => {
    if (inView) {
      fetchUserPosts();
    }
  }, [inView, fetchUserPosts]);

  useEffect(() => {
    fetchUserPosts();
  }, [fetchUserPosts]);

  // Reset posts and page when userId changes
  useEffect(() => {
    setPosts([]);
    setPage(1);
    setHasMore(true);
  }, [userId]);

  const handlePopupOpen = async (content: "followers" | "following") => {
    setPopupContent(content);
    setIsPopupOpen(true);
    if (content === "followers") {
      await fetchFollowers();
    } else {
      await fetchFollowing();
    }
  };

  const fetchFollowers = async () => {
    try {
      const response = await fetch(`http://localhost:5172/api/User/${userId}/followers`, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("authToken")}`,
          'accept': '*/*'
        }
      });

      if (!response.ok) {
        throw new Error("Failed to fetch followers");
      }

      const data = await response.json();
      setFollowers(data.data);
    } catch (error) {
      console.error("Error fetching followers:", error);
    }
  }

  const fetchFollowing = async () => {
    try {
      const response = await fetch(`http://localhost:5172/api/User/${userId}/following`, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("authToken")}`,
          'accept': '*/*'
        }
      });
  
      if (!response.ok) {
        throw new Error("Failed to fetch following");
      }
  
      const data = await response.json();
      setFollowing(data.data);
    } catch (error) {
      console.error("Error fetching following:", error);
    }
  };
  
  const handleUnfollowUser = async (targetId: number) => {
    try {
      const response = await fetch(`http://localhost:5172/api/User/${userId}/unfollow/${targetId}`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("authToken")}`,
          'accept': '*/*'
        }
      });

      if (!response.ok) {
        throw new Error("Failed to unfollow user");
      }
      setFollowing((prevFollowing) => prevFollowing.filter((user) => user.userId !== targetId));
      
      
    } catch (error) {
      console.error("Error unfollowing user:", error);
    }
  };
  const handleFollow = async () => {
    if (userId === CurrentUserId) {
      setError("You can't follow yourself.");
      return;
    }

    try {
      const response = await fetch(`http://localhost:5172/api/User/${CurrentUserId}/follow/${userId}`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${localStorage.getItem("authToken")}`,
          'accept': '*/*'
        }
      });

      if (!response.ok) {
        throw new Error("Failed to follow user");
      }

     
    } catch (error) {
      console.error("Error following user:", error);
      setError("Failed to follow user.");
    }
  };

  if (!user) {
    return <div>Loading...</div>;
  }

  return (
    <div className="space-y-6">
      <Card>
        <CardContent className="flex flex-col items-center space-y-4 pt-6">
          <Avatar className="w-24 h-24">
            <AvatarImage src={user.imageUrl} alt={user.name} />
            <AvatarFallback>{user.name[0]}</AvatarFallback>
          </Avatar>
          <h2 className="text-2xl font-bold header-text">{user.name}</h2>
          <p className="text-gray-500">@{user.username}</p>
          <h3 className="text-lg font-semibold ">{user.bio}</h3>
          <div className="flex space-x-4">
            <Button
              variant="ghost"
              className="text-center"
              onClick={() => handlePopupOpen("followers")}
            >
              <div>
                <p className="font-bold">{user.followersCount}</p>
                <p className="text-sm text-muted-foreground">Followers</p>
              </div>
            </Button>
            <Button
              variant="ghost"
              className="text-center"
              onClick={() => handlePopupOpen("following")}
            >
              <div>
                <p className="font-bold">{user.followingCount}</p>
                <p className="text-sm text-muted-foreground">Following</p>
              </div>
            </Button>
          </div>
          {!isCurrentUser && (
            <button 
              className="mt-4 px-4 py-2 bg-blue-500 text-white rounded follow-button"
              onClick={handleFollow}
            >
              Follow
            </button>
          )}
          {error && <p className="text-red-500 mt-2">{error}</p>}
        </CardContent>
      </Card>
      <Dialog open={isPopupOpen} onOpenChange={setIsPopupOpen}>
        <DialogContent className="sm:max-w-md bg-gray-100">
          <DialogHeader>
            <DialogTitle>{popupContent === "followers" ? "Followers" : "Following"}</DialogTitle>
          </DialogHeader>
          <div className="space-y-4 max-h-[60vh] overflow-y-auto">
            {popupContent === "followers"
              ? followers.map((follower) => (
              <div key={follower.id} className="flex items-center justify-between">
                <div className="flex items-center space-x-3">
                <Avatar onClick={() => router.push(`/profile-page/${follower.userId}`)} className="cursor-pointer">
                  <AvatarImage src={follower.imageUrl} alt={follower.name} />
                  <AvatarFallback>{follower.name[0]}</AvatarFallback>
                </Avatar>
                <span>{follower.username}</span>
                </div>
              </div>
              ))
              : following.map((user) => (
              <div key={user.id} className="flex items-center justify-between">
                <div className="flex items-center space-x-3">
                <Avatar onClick={() => router.push(`/profile-page/${user.userId}`)} className="cursor-pointer">
                  <AvatarImage src={user.imageUrl} alt={user.name} />
                  <AvatarFallback>{user.name[0]}</AvatarFallback>
                </Avatar>
                <span>{user.username}</span>
                </div>
                {isCurrentUser && (
                <Button
                  variant="outline"
                  onClick={() => handleUnfollowUser(user.userId)}
                  className="bg-red-500 text-white hover:bg-red-600"
                >
                  Unfollow
                </Button>
                )}
              </div>
              ))}
              

          </div>
        </DialogContent>
      </Dialog>
      <Card>
        <CardHeader>
          <h3 className="text-lg font-semibold">Skills</h3>
        </CardHeader>
        <CardContent>
          <div className="flex flex-wrap gap-2">
            {user.skills.map((skill, index) => (
              <Badge key={index}>{skill}</Badge>
            ))}
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <h3 className="text-lg font-semibold">Topics of Interest</h3>
        </CardHeader>
        <CardContent>
          <div className="flex flex-wrap gap-2">
            {user.topicsOfInterest.map((topic, index) => (
              <Badge key={index}>{topic}</Badge>
            ))}
          </div>
        </CardContent>
      </Card>
      <Card>
        <CardHeader>
        <h3 className="text-lg font-semibold">Posts</h3>
        </CardHeader>
        <CardContent>
        <div className="space-y-4">
        {posts.map((post) => (
          <PostComponent key={post.id} post={post} />
        ))}
        <div ref={ref} />
      </div>
      {loading && <div>Loading more posts...</div>}
        </CardContent>
      </Card>

    </div>
  );
};

export default RenderProfilePage;
