// /pages/_app.tsx
import React, { useState, useEffect } from "react";
import { AppProps } from "next/app";
import { useRouter } from 'next/router';
import Sidebar from "../app/components/Sidebar";
import { Navbar } from "../app/components/Navbar";
import Register from "./register";
import Login from "./login";
import '../app/globals.css';

function MyApp({ Component, pageProps }: AppProps) {
  const [isLogin, setIsLogin] = useState(false);
  const [isRegister, setIsRegister] = useState(false);
  const [isTokenValid, setIsTokenValid] = useState(false);
  const router = useRouter();

  useEffect(() => {
    if (window.location.pathname === '/login') {
      setIsLogin(true);
    } else if (window.location.pathname === '/register') {
      setIsRegister(true);
    } else {
      const token = localStorage.getItem("authToken");
      if (token) {
        // Replace this with your actual token validation logic
        const isValid = validateToken(token);
        setIsTokenValid(isValid);
        if (!isValid) {
          router.push('/login');
        }
      } else {
        router.push('/login');
      }
    }
  }, [router]);

  const validateToken = (token: string) => {
    const decodedToken = JSON.parse(atob(token.split('.')[1]));
    const nameid = decodedToken.nameid;
    return nameid ? true : false;
  };

  if (isLogin) {
    return <Login />;
  } else if (isRegister) {
    return <Register />;
  } else if (isTokenValid) {
    return (
      <div className="flex h-screen bg-gray-100">
        <Navbar />
        <main className="flex-1 overflow-auto">
          <header className="sticky top-0 z-10 bg-white border-b border-gray-200 p-4">
            <h1 className="text-2xl font-bold"><span className="header-text">Programmer&apos;s Social Hub </span></h1>
          </header>
          <Component {...pageProps} />
        </main>
        <Sidebar />
      </div>
    );
  } else {
    return null; // Or a loading spinner
  }
}

export default MyApp;
