'use client';
import React, { useEffect, useState } from "react";
import { Card, CardContent, CardHeader } from "./ui/card";
import { Avatar, AvatarFallback, AvatarImage } from "./ui/avatar";
import * as signalR from "@microsoft/signalr";
import PostComponent from "./Post";
import Modal from './ui/modal';

interface UserMetadata {
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
  userMetadata: UserMetadata;
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

interface Notification {
  id: string;
  imageUrl: string;
  message: string;
  senderUserId: string;
  timestamp: string;
}

const RenderNotificationsPage = ({ userId }: { userId: string }) => {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [selectedPost, setSelectedPost] = useState<Post | null>(null);
  const [showPopup, setShowPopup] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (!token) {
      console.error("No auth token found");
      return;
    }

    const decodedToken: any = JSON.parse(atob(token.split('.')[1]));
    const tokenUserId = decodedToken.nameid;

    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5068/hubs/notification")
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    connection
      .start()
      .then(() => {
        console.log("Connected to SignalR" + `${userId}`);
        fetch(`http://localhost:5068/api/Notification/user/${userId}`)
          .then((response) => response.json())
          .then((data: Notification[]) => {
            console.log("Initial notifications:", data);
            const filteredNotifications = data.filter(notification => notification.senderUserId !== tokenUserId);
            setNotifications(filteredNotifications);
          })
          .catch((error) =>
            console.error("Error fetching initial notifications:", error)
          );

        connection.on("NotificationsReceived", (notification: Notification) => {
          if (notification.senderUserId !== tokenUserId) {
            setNotifications((prevNotifications) => [
              notification,
              ...prevNotifications,
            ]);
          }
        });
      })
      .catch((error) => console.error("Error connecting to SignalR:", error));

    return () => {
      connection.stop();
    };
  }, [userId]);

  const handlePostClick = (postId: string) => {
    fetch(`http://localhost:5108/api/Post/${postId}`, {
      headers: {
      Authorization: `Bearer ${localStorage.getItem("authToken")}`,
      },
    })
      .then(async (response) => {
      if (response.ok) {
        const data = await response.json();
        console.log(data);
        setSelectedPost({
        id: data.id,
        title: data.title,
        content: data.content,
        userMetadata: data.userMetadata,
        comments: data.comments,
        code: data.code,
        language: data.language,
        images: data.images || [],
        likes: data.likes,
        saved: false,
        liked: false,
        showComments: false,
        createdAt: data.createdAt,
        updatedAt: data.updatedAt,
        });
        setShowPopup(true);
      } else {
        console.error("Error fetching post details:", response.statusText);
      }
      })
      .catch((error) => console.error("Error fetching post details:", error));
  };

  const closePopup = () => {
    setShowPopup(false);
    setSelectedPost(null);
  };

  const renderMessageWithButton = (message: string) => {
    const regex = /(liked your post|commented on your post) (\w+)/;
    const match = message.match(regex);

    if (match) {
      const postId = match[2];
      const messageText = message.replace(postId, "");
      return (
        <>
          {messageText}
          <button
            className="text-blue-500 underline ml-2"
            onClick={() => handlePostClick(postId)}
          >
            View Post
          </button>
        </>
      );
    }

    return message; // If no post ID is found, return the original message
  };

  return (
    <Card>
      <CardHeader>
      <h2 className="text-2xl font-bold">Notifications</h2>
      </CardHeader>
      <CardContent>
      <div className="space-y-4">
        {notifications.map((notification) => (
        <div key={notification.id} className="flex items-center space-x-4">
            <Avatar>
            <AvatarImage src={notification.imageUrl} alt="Sender's avatar" />
            <AvatarFallback>{notification.senderUserId[0]}</AvatarFallback>
            </Avatar>
          <div>
          <p>{renderMessageWithButton(notification.message)}</p>
          <p className="text-sm text-gray-500">
            {new Date(notification.timestamp).toLocaleString()}
          </p>
          </div>
        </div>
        ))}
      </div>
      </CardContent>

      {showPopup && selectedPost && (
      <Modal isOpen={showPopup} onClose={closePopup} closeOnOverlayClick={true}>
        <div className="bg-white p-4 rounded shadow-lg max-w-3xl w-full">
        <PostComponent post={selectedPost} />
        <button
          className="mt-4 text-red-500 underline"
          onClick={closePopup}
        >
          Close
        </button>
        </div>
      </Modal>
      )}
    </Card>
  );
};

export default RenderNotificationsPage;
