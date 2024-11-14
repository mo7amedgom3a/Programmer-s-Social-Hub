// EnhancedFullPage.tsx
"use client";
import { usePathname } from "next/navigation";
import { useEffect, useState } from "react";

export interface Comment {
  id: number;
  author: string;
  content: string;
}

export interface Post {
  id: number;
  author: string;
  avatar: string;
  content: string;
  title: string;
  lang: string;
  code: string;
  images: string[];
  likes: number;
  comments: Comment[];
  saved: boolean;
  showComments: boolean;
  liked?: boolean;
}

export function EnhancedFullPage() {
  const pathname = usePathname(); // Get current pathname
  const [, setIsToken] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (token) {
      const decodedToken = JSON.parse(atob(token.split('.')[1]));
      const nameid = decodedToken.nameid;
      setIsToken(true);
      
      if (pathname === "/") {
      if (typeof window !== "undefined") {
        window.location.href = '/home' + '?userid=' + nameid;
      }
      } 
      

    } else {
      if (typeof window !== "undefined") {
        window.location.href = '/login';
      }
    }
  }, [pathname]);

  return null;
}
