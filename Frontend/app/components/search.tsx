'use client';
import React, { useState, useEffect } from "react";
import { SearchIcon } from 'lucide-react';
import { Button } from "./ui/button";
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar";
import { Card, CardContent, CardHeader } from "./ui/card";
import { Input } from "./ui/input";
import '../globals.css';
import { useRouter } from "next/navigation";

interface Topic {
  id: number;
  name: string;
  posts: number;
}

interface User {
  userId: string;
  username: string;
  name: string;
  bio: string;
  imageUrl: string;
  followersCount: number;
}

interface SearchResults {
  topics: Topic[];
  users: User[];
}

const PAGE_SIZE = 10;

const getUserIdFromToken = (token: string) => {
  const decodedToken = JSON.parse(atob(token.split('.')[1]));
  return decodedToken.nameid;
};

export default function SearchPage() {
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState<SearchResults>({ topics: [], users: [] });
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(false);
  const [followedUsers, setFollowedUsers] = useState<string[]>([]);
  const [showPopup, setShowPopup] = useState(false);
  const token = localStorage.getItem('authToken');
  const userId = token ? getUserIdFromToken(token) : null;
  const router = useRouter();

  useEffect(() => {
    const fetchFollowedUsers = async () => {
      if (!userId) return;
      try {
        const response = await fetch(`http://localhost:5172/api/User/${userId}/following?page=1&pageSize=10`, {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });
        const data = await response.json();
        const followedUserIds = data.data.map((user: User) => user.userId);
        setFollowedUsers(followedUserIds);
      } catch (error) {
        console.error("Error fetching followed users:", error);
      }
    };

    fetchFollowedUsers();
  }, [userId, token]);

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      const response = await fetch(`http://localhost:5172/api/User/search?query=${searchQuery}&page=${page}&pageSize=${PAGE_SIZE}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
          'accept': '*/*'
        }
      });
      const data: User[] = await response.json();
      const filteredData = data.filter(user => user.userId !== userId && !followedUsers.includes(user.userId));
      setSearchResults(prevResults => ({
        ...prevResults,
        users: page === 1 ? filteredData : [...prevResults.users, ...filteredData]
      }));
    } catch (error) {
      console.error('Error fetching search results:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadMore = () => {
    setPage(prevPage => prevPage + 1);
  };

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

      setFollowedUsers(prevFollowedUsers => [...prevFollowedUsers, followUserId]);
      setSearchResults(prevResults => ({
        ...prevResults,
        users: prevResults.users.filter(user => user.userId !== followUserId)
      }));
      setShowPopup(true);
      setTimeout(() => setShowPopup(false), 2000); // Hide popup after 2 seconds
    } catch (error) {
      console.error("Error following user:", error);
    }
  };

  useEffect(() => {
    if (page > 1) {
      handleSearch(new Event('submit') as unknown as React.FormEvent);
    }
  }, [page]);

  return (
    <div className="space-y-6">
      {showPopup && (
        <div className="absolute top-4 right-4 bg-white border border-gray-200 p-2 rounded-md shadow-md">
          <div className="text-sm text-green-600">User followed successfully</div>
        </div>
      )}
      <Card>
        <CardHeader>
          <h2 className="text-2xl font-bold">Search</h2>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSearch} className="space-y-4">
            <div className="flex space-x-2">
              <Input
                placeholder="Search for topics or users"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="flex-1"
              />
              <Button type="submit" disabled={loading}>
                <SearchIcon className="w-4 h-4 mr-2" />
                Search
              </Button>
            </div>
          </form>
        </CardContent>
      </Card>

      {searchResults.users.length > 0 && (
        <Card>
          <CardHeader>
            <h3 className="text-xl font-semibold">Users</h3>
          </CardHeader>
          <CardContent>
            <div className="space-y-4">
              {searchResults.users.map((user) => (
                <div key={user.userId} className="flex justify-between items-center">
                  <div className="flex items-center space-x-4">
                    <Avatar className="cursor-pointer" onClick={() => router.push(`/profile-page/${user.userId || 'defaultUser'}`)}>
                      <AvatarImage src={user.imageUrl || '/placeholder-avatar.jpg'} alt={user.name} />
                      <AvatarFallback>{user.name[0]}</AvatarFallback>
                    </Avatar>
                    <div>
                      <h4 className="font-semibold">{user.name}</h4>
                      <p className="text-sm text-gray-500">{user.bio}</p>
                      <p className="text-sm text-gray-500">{user.followersCount} followers</p>
                    </div>
                  </div>
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => handleFollow(user.userId)}
                    disabled={followedUsers.includes(user.userId)}
                    className={followedUsers.includes(user.userId) ? 'text-green-500' : ''}
                  >
                    {followedUsers.includes(user.userId) ? 'Followed' : 'Follow'}
                  </Button>
                </div>
              ))}
            </div>
            {searchResults.users.length >= PAGE_SIZE && (
              <Button onClick={loadMore} disabled={loading}>
                {loading ? 'Loading...' : 'Load More'}
              </Button>
            )}
          </CardContent>
        </Card>
      )}
    </div>
  );
}