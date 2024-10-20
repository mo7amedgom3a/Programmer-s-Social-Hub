'use client'
import React, { useState, useEffect } from "react";
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar";
import { Button } from "./ui/button";
import { useRouter } from "next/navigation";
import { ScrollArea } from "./ui/scroll-area";

interface User {
  userId: string;
  username: string;
  bio: string;
  followersCount: number;
  followingCount: number;
  imageUrl: string;
  name: string;
  _links: {
    self: string;
    follow: string;
    unfollow: string;
  };
}

interface Pagination {
  totalItems: number;
  currentPage: number;
  pageSize: number;
  totalPages: number;
  _links: {
    next: string | null;
    previous: string | null;
  };
}

interface UsersResponse {
  data: User[];
  pagination: Pagination;
}

interface FollowedUser {
  username: string;
  userId: string;
  name: string;
  gender: string;
  bio: string;
  age: number;
  skills: string[];
  topicsOfInterest: string[];
  imageUrl: string;
  followers: string[];
  following: string[];
  followersCount: number;
  followingCount: number;
}

interface FollowedUsersResponse {
  data: FollowedUser[];
  pagination: Pagination;
}

const Sidebar = () => {
  const [users, setUsers] = useState<User[]>([]);
  const [followedUsers, setFollowedUsers] = useState<string[]>([]);
  const [showPopup, setShowPopup] = useState(false);
  const [userId, setUserId] = useState<string | null>(null);
  const [page, setPage] = useState(1);
  const token = localStorage.getItem('authToken');
  const router = useRouter();

  useEffect(() => {
    if (token) {
      try {
        const decodedToken = JSON.parse(atob(token.split('.')[1]));
        const nameid = decodedToken.nameid;
        setUserId(nameid);
      } catch (error) {
        console.error("Failed to decode token", error);
      }
    }
  }, [token]);

  useEffect(() => {
    const fetchUsers = async (page: number) => {
      try {
        const response = await fetch(`http://localhost:5172/api/User?page=${page}&pageSize=10`, {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });
        const data: UsersResponse = await response.json();
        setUsers((prevUsers) => page === 1 ? data.data : [...prevUsers, ...data.data]);
      } catch (error) {
        console.error("Error fetching users:", error);
      }
    };

    const fetchFollowedUsers = async () => {
      try {
        const response = await fetch(`http://localhost:5172/api/User/${userId}/following?page=1&pageSize=10`, {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });
        const data: FollowedUsersResponse = await response.json();
        const followedUserIds = data.data.map(user => user.userId);
        setFollowedUsers(followedUserIds);
      } catch (error) {
        console.error("Error fetching followed users:", error);
      }
    };

    if (userId) {
      fetchUsers(page);
      fetchFollowedUsers();
    }
  }, [token, userId, page]);

  const handleFollow = async (followUserId: string) => {
    if (!userId) return;

    try {
      const response = await fetch(`http://localhost:5172/api/User/${userId}/follow/${followUserId}`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        }
      });

      if (response.status >= 400) {
        const errorData = await response.json();
        window.alert(errorData.message || "Error occurred");
        throw new Error(errorData.message || "Error occurred");
      }

      setUsers((prevUsers) => prevUsers.filter(user => user.userId !== followUserId));
      setShowPopup(true);
      setTimeout(() => setShowPopup(false), 2000); // Hide popup after 2 seconds

      // Fetch more users after following
      setPage((prevPage) => prevPage + 1);
    } catch (error) {
      console.error("Error following user:", error);
    }
  };

  const filteredUsers = users.filter(user => !followedUsers.includes(user.userId));

  return (
    <aside className="hidden md:block w-87 bg-white border-l border-gray-200 p-4">
      {showPopup && (
      <div className="absolute top-4 right-4 bg-white border border-gray-200 p-2 rounded-md shadow-md">
        <div className="text-sm text-green-600">User followed successfully</div>
      </div>
      )}
      <h2 className="text-lg font-semibold mb-4">Who to Follow</h2>
      <ScrollArea className="h-[calc(100vh-8rem)]">
      {filteredUsers
        .filter(user => user.userId !== userId) // Skip the current user
        .map((user) => (
        <div key={user.userId} className="flex items-center justify-between mb-4">
          <div className="flex items-center gap-2">
          <Avatar className="cursor-pointer">
            <AvatarImage src={user.imageUrl || '/placeholder-avatar.jpg'} alt={user.name} onClick={() => router.push(`/profile-page/${user.userId}`)} />
            <AvatarFallback>{user.name.charAt(0)}</AvatarFallback>
          </Avatar>
          <div>
            <div className="font-semibold">{user.name} </div>
          </div>
          </div>
          <Button variant="outline" size="sm" onClick={() => handleFollow(user.userId)}>Follow</Button>
        </div>
        ))}
      <Button variant="outline" size="sm" onClick={() => setPage((prevPage) => prevPage + 1)}>Load More</Button>
      </ScrollArea>
    </aside>
  );
};

export default Sidebar;
