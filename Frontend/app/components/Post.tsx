'use client';
import React, { useState, FormEvent, ChangeEvent, useEffect } from "react";
import { Avatar, AvatarFallback, AvatarImage } from "../components/ui/avatar";
import { Card, CardContent, CardFooter, CardHeader } from "../components/ui/card";
import { Button } from "../components/ui/button";
import { Input } from "../components/ui/input";
import dynamic from 'next/dynamic';
import { HeartIcon, MessageCircleIcon, BookmarkIcon } from "lucide-react";
import DOMPurify from 'isomorphic-dompurify';
import Popup from "reactjs-popup";
import 'reactjs-popup/dist/index.css';
import RenderProfilePage from "./UserPage";
import { useRouter } from "next/navigation";
import { CommentSection } from "./CommentSection";
const MonacoEditor = dynamic(() => import('@monaco-editor/react'), { ssr: false });

interface Comment {
  id: number;
  author: string;
  content: string;
  code: string;
  language: string;
}
interface userMetadata {
  userId : string;
  bio : string;
  username: string;
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
  showComments: boolean;
  liked: boolean;
  createdAt: string;
  updatedAt: string; 
}

const PostComponent: React.FC<{ post: Post }> = ({ post }) => {
  const [postState, setPostState] = useState(post); // Hold the post state for like, save, and comment actions.
  const [showComments, setShowComments] = useState(false); // State to manage visibility of comments
  const [isPopupOpen, setIsPopupOpen] = useState(false);
  const [commnetsNumber, setCommentsNumber] = useState(post.comments.length);
  const [userId, setUserId] = useState('');
  const router = useRouter();
  const token = localStorage.getItem('authToken');
  if (!token) {
    router.push('/login');
  }
  useEffect(() => {
    const token = localStorage.getItem('authToken');
    if (token) {
      const decodedToken = JSON.parse(atob(token.split('.')[1]));
      setUserId(decodedToken.nameid);
    }
  }, [token]);
  const togglePopup = () => setIsPopupOpen(!isPopupOpen);
  console.log(postState);

  const createMarkup = (content: string) => {
    const sanitizedContent = DOMPurify.sanitize(content);
    return { __html: sanitizedContent };
  };
  useEffect(() => {
    // Fetch the list of likes for this post when the component mounts
    const fetchLikes = async () => {
      try {
        const response = await fetch(`http://localhost:5108/api/Post/likes/${post.id}`, {
          method: 'GET',
          headers: {
            'accept': '*/*',
            'Authorization': `Bearer ${token}`,
          },
        });
        const likes = await response.json();
        const userHasLiked = likes.some((like: any) => like.userMetadata.userId === userId);
        
        // Update the post state with the like status
        setPostState((prevState) => ({
          ...prevState,
          liked: userHasLiked,
          likes: likes.length,
        }));
      } catch (error) {
        console.error('Error fetching likes:', error);
      }
    };

    fetchLikes();
  }, [post.id, userId, token]);
  // Handle like/dislike action
  const handleLike = async () => {
    const updatedPost = {
      ...postState,
      liked: !postState.liked,
      likes: postState.liked ? postState.likes - 1 : postState.likes + 1,
    };
    setPostState(updatedPost);

    const url = `http://localhost:5108/api/Post/likes/${postState.id}/${userId}`;
    const method = postState.liked ? 'DELETE' : 'POST';

    try {
      await fetch(url, {
        method,
        headers: {
          'accept': '*/*',
          'Authorization': `Bearer ${token}`,
        },
      });
    } catch (error) {
      console.error('Error updating like status:', error);
      // Revert the state if the API call fails
      setPostState(post);
    }
  };

  useEffect(() => {
    // Fetch the list of saved posts for the user when the component mounts
    const fetchSavedPosts = async () => {
      try {
        const response = await fetch(`http://localhost:5108/api/Post/saved/${userId}`, {
          method: 'GET',
          headers: {
            'accept': '*/*',
            'Authorization': `Bearer ${token}`,
          },
        });
        const savedPosts = await response.json();
        const userHasSavedPost = savedPosts.some((savedPost: any) => savedPost.postDto.id === post.id);

        // Update the post state with the saved status
        setPostState((prevState) => ({
          ...prevState,
          saved: userHasSavedPost,
        }));
      } catch (error) {
        console.error('Error fetching saved posts:', error);
      }
    };

    fetchSavedPosts();
  }, [post.id, userId, token]);

  // Handle save/unsave action
  const handleSave = async () => {
    const updatedPost = { ...postState, saved: !postState.saved };
    setPostState(updatedPost);

    const url = `http://localhost:5108/api/Post/saved/${postState.id}/${userId}`;
    const method = postState.saved ? 'DELETE' : 'POST';

    try {
      await fetch(url, {
        method,
        headers: {
          'accept': '*/*',
          'Authorization': `Bearer ${token}`,
        },
      });
    } catch (error) {
      console.error('Error updating save status:', error);
      // Revert the state if the API call fails
      setPostState(post);
    }
  };

  return (
    <Card key={postState.id} className="overflow-hidden">
 
      <CardHeader className="flex flex-row items-center gap-4">
        <Avatar className="cursor-pointer" onClick={() => router.push(`/profile-page/${postState.userMetadata?.userId || 'defaultUser'}`)}>
          <AvatarImage src={postState.userMetadata?.imageUrl || '/default-avatar.png'} alt={postState.userMetadata?.name || 'Default User'} />
          <AvatarFallback>{postState.userMetadata?.name || 'Default User'}</AvatarFallback>
        </Avatar>
        <div className="cursor-pointer flex-1 min-w-0">
          {postState.userMetadata?.name ? (
            <h3 className="text-lg font-semibold cursor-pointer truncate" onClick={() => router.push(`/profile-page/${postState.userMetadata.userId}`)}>
              {postState.userMetadata.name}
            </h3>
          ) : (
            <h3 className="text-lg font-semibold truncate">Default User</h3>
          )}
          <p className="text-sm text-gray-500 truncate">{postState.userMetadata?.bio || 'No bio available'}</p>
          <p className="text-sm text-gray-500">Posted {new Date(postState.createdAt).toLocaleString()}</p>
        </div>
      </CardHeader>

      <CardContent className="space-y-4">
        <h2 className="text-xl font-semibold truncate">{postState.title}</h2>

        {postState.content && (
          <div className="post-content" dangerouslySetInnerHTML={createMarkup(postState.content)} />
        )}

        {postState.code && (
          <pre className="bg-gray-100 p-4 rounded-md overflow-x-auto">
            <MonacoEditor
              height="200px"
              language={postState.language}
              value={postState.code}
              options={{ readOnly: true }}
            />
          </pre>
        )}
        {postState.images.length > 0 && (
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-2">
            {postState.images.map((image, index) => (
              <div key={index} className="relative aspect-w-1 aspect-h-1 group">
                <img src={image} alt="Post" className="object-cover w-full h-full rounded-md" />
                <div className="absolute inset-0 flex items-center justify-center gap-2 opacity-0 group-hover:opacity-100 bg-opacity-100 rounded-md">
                  {postState.images.length > 0 && <Button variant="ghost" onClick={togglePopup}>View</Button>}
                </div>
                <Popup open={isPopupOpen} onClose={togglePopup}>
                  <img src={image} alt="Post" className="object-cover w-full h-full rounded-md img-card" />
                </Popup>
              </div>
            ))}
          </div>
        )}
      </CardContent>

      <CardFooter className="flex justify-between">
        <div className="flex space-x-2">
          <Button variant="ghost" onClick={handleLike}>
            <HeartIcon className={`w-5 h-5 ${postState.liked ? 'text-red-500 fill-red-500' : 'text-gray-500'}`} />
            <span>{postState.likes} {postState.likes === 1 ? 'Like' : 'Likes'}</span>
          </Button>
          <Button variant="ghost" onClick={() => setShowComments(!showComments)}>
            <MessageCircleIcon className="w-5 h-5" />
            <span>{commnetsNumber} Comments</span>
          </Button>
        </div>
        <Button variant="ghost" onClick={handleSave}>
          <BookmarkIcon className={`w-5 h-5 ${postState.saved ? 'text-blue-500 fill-blue-500' : 'text-gray-500'}`} />
          <span>{postState.saved ? 'Saved' : 'Save'}</span>
        </Button>
      </CardFooter>
      {showComments && <CommentSection PostId={postState.id} commentsNumber={commnetsNumber} setCommentsNumber={setCommentsNumber} />}
    </Card>
  );
};

export default PostComponent;
