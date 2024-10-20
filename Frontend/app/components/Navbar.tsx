// Navbar.tsx
import { Button } from "../components/ui/button";
import { HomeIcon, SearchIcon, UserIcon, BellIcon, BookmarkIcon, LogInIcon, LogOutIcon, SettingsIcon } from "lucide-react";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";

export const Navbar = () => {
  const router = useRouter();
  const [userId, setUserId] = useState<string | null>(null);

  useEffect(() => {
    const token = localStorage.getItem('authToken');
    if (token) {
      try {
        const decodedToken = JSON.parse(atob(token.split('.')[1]));
        const nameid = decodedToken.nameid;
        setUserId(nameid);
      } catch (error) {
        console.error("Failed to decode token", error);
      }
    }
  }, []);

  if (!userId) {
    return null; // or a loading spinner, or some other placeholder
  }

  return (
    <nav className="flex flex-col justify-between w-16 bg-white border-r border-gray-200">
      <div className="flex flex-col items-center pt-5 space-y-4">
        <Button
          variant="ghost"
          size="icon"
          aria-label="Home"
          onClick={() => router.push(`/home?userid=${userId}`)}
        >
          <HomeIcon className="w-6 h-6" />
        </Button>
        <Button
          variant="ghost"
          size="icon"
          aria-label="Search"
          onClick={() => router.push("/search")}
        >
          <SearchIcon className="w-6 h-6" />
        </Button>
        <Button
          variant="ghost"
          size="icon"
          aria-label="Profile"
          onClick={() => router.push(`/profile-page/${userId}`)}
        >
          <UserIcon className="w-6 h-6" />
        </Button>
        <Button
          variant="ghost"
          size="icon"
          aria-label="Notifications"
          onClick={() => router.push(`/notifications/${userId}`)}
        >
          <BellIcon className="w-6 h-6" />
        </Button>
        <Button
          variant="ghost"
          size="icon"
          aria-label="Saved Posts"
          onClick={() => router.push(`/save/${userId}`)}
        >
          <BookmarkIcon className="w-6 h-6" />
        </Button>
      </div>
      <div className="pb-5 flex flex-col items-center space-y-4">
        <Button
          variant="ghost"
          size="icon"
          aria-label="Login"
          onClick={() => router.push("/login")}
          className="flex flex-col items-center space-y-1"
        >
          <LogInIcon className="w-6 h-6"/>
          <span className="text-xs">Login</span>
        </Button>
        <Button
          variant="ghost"
          size="icon"
          aria-label="Logout"
          onClick={() => {
            localStorage.removeItem('authToken');
            setUserId(null);
            router.push("/login");
          }}
          className="flex flex-col items-center space-y-1 text-red-500 hover:text-red-600 focus:text-red-600"
        >
          <LogOutIcon className="w-6 h-6"/>
          <span className="text-xs">Logout</span>
        </Button>

      </div>
      <div className="pb-5">
        <Button
          variant="ghost"
          size="icon"
          aria-label="Settings"
          onClick={() => router.push(`/settings/${userId}`)}
        >
          <SettingsIcon className="w-6 h-6" />
        </Button>
      </div>
    </nav>
  );
};
