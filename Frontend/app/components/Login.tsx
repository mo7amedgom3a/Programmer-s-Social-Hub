import { useEffect, useState } from 'react';
import { Button } from "../components/ui/button";
import { Input } from "../components/ui/input";
import Label from "../components/ui/label";
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "../components/ui/card";
import { TerminalIcon, UserIcon, KeyIcon, EyeIcon, EyeOffIcon, CodeXmlIcon } from 'lucide-react';

export default function LoginComponent() {
  const [showPassword, setShowPassword] = useState(false);
  const [Username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [loginDate, setLoginDate] = useState('');
  const [error, setError] = useState(''); // State for error message

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(''); // Clear any previous errors
    try {
      const response = await fetch('http://localhost:5217/api/Auth/login', {
        method: "POST",
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Username, password }),
      });

      if (!response.ok) {
        throw new Error('Login failed. Please check your credentials.');
      }

      const token = await response.text();
    

      // Save token in local storage
      localStorage.setItem('authToken', token);

      // Decode token to extract the nameid
      const decodedToken = JSON.parse(atob(token.split('.')[1]));
      const nameid = decodedToken.nameid;

      // Redirect to home page with nameid
      window.location.href = `/home?userid=${nameid}`;
    } catch (error: any) {
      if (error instanceof Error) {
        setError(error.message); // Set the error message to display
      } else {
        setError('An unknown error occurred.');
      }
    }
  };

  useEffect(() => {
    const timer = setInterval(() => {
      setLoginDate(new Date().toLocaleString());
    }, 1000);

    return () => clearInterval(timer);
  }, []);

  return (
    <div className="min-h-screen flex items-center justify-center bg-white p-4">
      <Card className="w-full max-w-2xl bg-white text-black-400 border-green-500 font-mono">
        <CardHeader className="border-b border-green-500 items-center justify-center">
          <div className="flex items-center space-x-2">
            <TerminalIcon className="w-5 h-5 text-green-500" />
            <CardTitle className="text-xl font-bold text-green-500">Programmer&apos;s Social Hub</CardTitle>
          </div>
          <CardDescription className="text-green-400 text-lg">
            Authenticate to access the system
          </CardDescription>
        </CardHeader>
        <CardContent className="mt-4 space-y-4">
          <div className="bg-white border border-green-500 p-4 rounded">
            <p className="mb-2 text-green-500 text-center">Login Date: {loginDate}</p>
            <p className="mb-4 text-green-500">Welcome to the Programmer&apos;s Social Hub. Please log in.</p>
            {error && <p className="text-red-500 text-center">{error}</p>} {/* Display error message if present */}
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="space-y-2">
                <Label htmlFor="Username" className="text-black-400 flex items-center">
                  <UserIcon className="w-4 h-4 mr-2 text-green-500" />
                  Username:
                </Label>
                <div className="flex items-center">
                  <span className="text-green-500 mr-2">$</span>
                  <Input
                    id="Username"
                    type="text"
                    value={Username}
                    onChange={(e) => setUsername(e.target.value)}
                    placeholder="user@example.com"
                    className="bg-white border-green-500 text-black-400 focus:ring-green-500 focus:border-green-500"
                    required
                  />
                </div>
              </div>
              <div className="space-y-2">
                <Label htmlFor="password" className="text-black-400 flex items-center">
                  <KeyIcon className="w-4 h-4 mr-2 text-green-500" />
                  Password:
                </Label>
                <div className="flex items-center relative">
                  <span className="text-green-500 mr-2">$</span>
                  <Input
                    id="password"
                    type={showPassword ? "text" : "password"}
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    placeholder="Enter password"
                    className="bg-white border-green-500 text-black-400 focus:ring-green-500 focus:border-green-500 pr-10"
                    required
                  />
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    className="absolute right-2 top-1/2 transform -translate-y-1/2"
                    onClick={() => setShowPassword(!showPassword)}
                  >
                    {showPassword ? (
                      <EyeOffIcon className="text-black-500 w-4 h-4" />
                    ) : (
                      <EyeIcon className="text-black-500 w-4 h-4" />
                    )}
                    <span className="sr-only">
                      {showPassword ? "Hide password" : "Show password"}
                    </span>
                  </Button>
                </div>
              </div>
              <Button type="submit" className="w-full bg-green-600 hover:bg-green-700 text-black">
                Login
              </Button>
            </form>
          </div>
            <div className="flex items-center justify-center space-x-2">
            <p className="text-green-500">Don&apos;t have an account? </p>
            <Button
              variant="ghost"
              size="icon"
              className="text-green-600 hover:text-green-500 underline bg-white hover:bg-white-500"
              onClick={() => window.location.href = '/register' }
            >
              <span className="text-xm space-y-3 ml-8">Register</span>
            </Button>
            
            </div>
        </CardContent>
        <CardFooter className="text-center text-black-500 border-t border-green-500">
          <div className="flex items-center justify-center space-x-1">
            <p className="text-xs w-full text-green-500">[System]: Connection secure. Remember to log out when finished.</p>
            <CodeXmlIcon className="w-4 h-4 text-green-500" />
          </div>
        </CardFooter>
      </Card>
    </div>
  );
}
