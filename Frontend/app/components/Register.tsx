'use client'

import { useEffect, useState } from 'react'

import { Input } from "../components/ui/input"
import '../globals.css'
import Label from "../components/ui/label"
import { Textarea } from "../components/ui/textarea"
import Popup from 'reactjs-popup'
import { EyeIcon } from 'lucide-react'
import { Button } from '../components/ui/button'
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "../components/ui/select"
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "../components/ui/card"
import { TerminalIcon, UserIcon, InfoIcon, CodeIcon, BookOpenIcon, ImageIcon } from 'lucide-react'
import { MultiSelect } from "react-multi-select-component"
import ImageEditorComponent from './imageeditor'

export function RegisterComponent() {
  const [formData, setFormData] = useState<{
    username: string,
    password: string,
    name: string,
    gender: string,
    bio: string,
    age: string,
    skills: { label: string, value: string }[],
    topicsOfInterest: { label: string, value: string }[],
    imageUrl: string,
  }>({
    username: '',
    password: '',
    name: '',
    gender: '',
    bio: '',
    age: '',
    skills: [],
    topicsOfInterest: [],
    imageUrl: '',
  })
  const [imageState, setImageState] = useState('')
  const [showPassword, setShowPassword] = useState(false)
  const [showPopup, setShowPopup] = useState(false)
 
  useEffect(() => {
    setFormData(prevData => ({ ...prevData, imageUrl: imageState }))
  }, [imageState])

  const [RegisterDate, setLoginDate] = useState('')

  useEffect(() => {
    const timer = setInterval(() => {
      setLoginDate(new Date().toLocaleString())
    }, 1000)

    return () => clearInterval(timer)
  }, [])

  const SkillsOptions = [
    { label: 'Python', value: 'python' },
    { label: 'Java', value: 'java' },
    { label: 'JavaScript', value: 'javascript' },
    { label: 'C++', value: 'c++' },
    { label: 'C#', value: 'c#' },
    { label: 'Ruby', value: 'ruby' },
    { label: 'PHP', value: 'php' },
    { label: 'Swift', value: 'swift' },
    { label: 'Kotlin', value: 'kotlin' },
    { label: 'Rust', value: 'rust' },
    { label: 'Docker', value: 'docker' },
    { label: 'Kubernetes', value: 'kubernetes' },
    { label: 'React', value: 'react' },
    { label: 'Angular', value: 'angular' },
    { label: '.NET', value: '.net' },
    { label: 'Node.js', value: 'node.js' },
    { label: 'Vue.js', value: 'vue.js' },
    { label: 'TypeScript', value: 'typescript' },
  ]

  const TopicsOfInterestOptions = [
    { label: 'Web Development', value: 'web-development' },
    { label: 'Mobile Development', value: 'mobile-development' },
    { label: 'Data Science', value: 'data-science' },
    { label: 'Machine Learning', value: 'machine-learning' },
    { label: 'Artificial Intelligence', value: 'artificial-intelligence' },
    { label: 'Cybersecurity', value: 'cybersecurity' },
    { label: 'Game Development', value: 'game-development' },
    { label: 'DevOps', value: 'devops' },
    { label: 'Cloud Computing', value: 'cloud-computing' },
    { label: 'Blockchain', value: 'blockchain' },
  ]

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target
    setFormData(prevData => ({ ...prevData, [name]: value }))
  }

  const handleSelectChange = (name: string, value: string) => {
    setFormData(prevData => ({ ...prevData, [name]: value }))
  }

  const handleMultiSelectChange = (selectedOptions: any[], name: string) => {
    setFormData(prevData => ({ ...prevData, [name]: selectedOptions }))
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()

    const payload = {
      ...formData,
      age: parseInt(formData.age, 10), // Ensure age is a number
      skills: formData.skills.map(skill => skill.value), // Extract values from selected options
      topicsOfInterest: formData.topicsOfInterest.map(topic => topic.value) // Extract values from selected options
    }

    try {
      const response = await fetch('http://localhost:5217/api/Auth/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'accept': '*/*'
        },
        body: JSON.stringify(payload)
      })

      if (response.ok) {
        const data = await response.text()
        // Clear the form inputs
        setFormData({
          username: '',
          password: '',
          name: '',
          gender: '',
          bio: '',
          age: '',
          skills: [],
          topicsOfInterest: [],
          imageUrl: '',
        })
        // Show success popup
        setShowPopup(true)
        // Redirect to login page after a delay
        setTimeout(() => {
          window.location.href = '/login'
        }, 2000) // Adjust the delay as needed
      } else {
        throw new Error('Registration failed')
      }
    } catch (error) {
      console.error('Error during registration:', error)
      // Handle registration error
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-white p-4">
      {showPopup && (
        <Popup
          open={showPopup}
          closeOnDocumentClick={false}
          onClose={() => setShowPopup(false)}
          modal
          nested
          className="bg-white border border-green-500 p-4 rounded"
        >
          <div className="text-center space-y-4">
            <p className="text-green-500">Registration successful! 🎉</p>
          </div>
        </Popup>
      )}
      <Card className="w-full max-w-2xl bg-white text-black-400 border-green-500 font-mono">
        <CardHeader className="border-b border-green-500 items-center justify-center">
          <div className="flex items-center space-x-2">
            <TerminalIcon className="w-5 h-5 text-green-500" />
            <CardTitle className="text-xl font-bold text-green-500">Registration</CardTitle>
          </div>
          <CardDescription className="text-green-500 text-lg">
            Initialize new user account
          </CardDescription>
        </CardHeader>
        <CardContent className="mt-4 space-y-4">
          <div className="bg-white border border-green-500 p-4 rounded">
          <p className="mb-2 text-green-500 text-center">register Date: {RegisterDate}</p>
            <p className="mb-4 text-green-500">Welcome to the Programmer's Social Hub. Please enter your details to register.</p>
            <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-2">
                <Label htmlFor="username" className="text-black-400 flex items-center">
                  <UserIcon className="w-4 h-4 mr-2 text-green-500" />
                  Username:
                </Label>
                <div className="flex items-center">
                  <span className="text-green-500 mr-2">$</span>
                  <Input
                    id="username"
                    name="username"
                    value={formData.username}
                    onChange={handleInputChange}
                    placeholder="Enter username"
                    className="bg-white border-green-500 text-black-400 focus:ring-green-500 focus:border-green-500"
                    required
                  />
                </div>
              </div>
                <div className="space-y-2">
                <Label htmlFor="password" className="text-black-400 flex items-center">
                  <UserIcon className="w-4 h-4 mr-2 text-green-500" />
                  Password:
                </Label>
                <div className="flex items-center">
                  <span className="text-green-500 mr-2">$</span>
                  <Input
                  id="password"
                  name="password"
                  type={showPassword ? "text" : "password"}
                  value={formData.password}
                  onChange={handleInputChange}
                  placeholder="Enter password"
                  className="bg-white border-green-500 text-black-400 focus:ring-green-500 focus:border-green-500"
                  required
                  />
                  <EyeIcon
                  className="w-5 h-5 ml-2 text-green-500 cursor-pointer"
                  onClick={() => setShowPassword(!showPassword)}
                  />
                </div>

                </div>
              <div className="space-y-2">
                <Label htmlFor="name" className="text-black-400 flex items-center">
                  <UserIcon className="w-4 h-4 mr-2 text-green-500" />
                  Name:
                </Label>
                <div className="flex items-center">
                  <span className="text-green-500 mr-2">$</span>
                  <Input
                    id="name"
                    name="name"
                    value={formData.name}
                    onChange={handleInputChange}
                    placeholder="Enter full name"
                    className="bg-white border-green-500 text-black-400 focus:ring-green-500 focus:border-green-500"
                    required
                  />
                </div>
              </div>
              <div className="space-y-2">
                <Label htmlFor="gender" className="text-black-400 flex items-center">
                  <InfoIcon className="w-4 h-4 mr-2 text-green-500" />
                  Gender:
                </Label>
                <div className="flex items-center">
                  <span className="text-green-500 mr-2">$</span>
                  <Select onValueChange={(value) => handleSelectChange('gender', value)}>
                    <SelectTrigger className="bg-white border-green-500 text-black-400 focus:ring-green-500 focus:border-green-500">
                      <SelectValue placeholder="Select gender" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="male">Male</SelectItem>
                      <SelectItem value="female">Female</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
              <div className="space-y-2">
                <Label htmlFor="bio" className="text-black-400 flex items-center">
                  <InfoIcon className="w-4 h-4 mr-2 text-green-500" />
                  Bio:
                </Label>
                <div className="flex items-center">
                  <span className="text-green-500 mr-2">$</span>
                  <Textarea
                    id="bio"
                    name="bio"
                    value={formData.bio}
                    onChange={handleInputChange}
                    placeholder="Enter your bio"
                    className="bg-white border-green-500 text-black-400 focus:ring-green-500 focus:border-green-500"
                    required
                  />
                </div>
              </div>
              <div className="space-y-2">
                <Label htmlFor="age" className="text-black-400 flex items-center">
                  <InfoIcon className="w-4 h-4 mr-2 text-green-500" />
                  Age:
                </Label>
                <div className="flex items-center">
                  <span className="text-green-500 mr-2">$</span>
                  <Input
                    id="age"
                    name="age"
                    type="number"
                    value={formData.age}
                    onChange={handleInputChange}
                    placeholder="Enter your age"
                    className="bg-white border-green-500 text-black-400 focus:ring-green-500 focus:border-green-500"
                    required
                  />
                </div>
              </div>
                <div className="space-y-2">
                <Label htmlFor="skills" className="text-black-400 flex items-center">
                  <CodeIcon className="w-4 h-4 mr-2 text-green-500" />
                  Skills:
                </Label>
                <MultiSelect
                  className='bg-white border-green-500 text-black-400 focus:ring-green-500 focus:border-green-500 input text-black-400'
                  options={SkillsOptions}
                  value={formData.skills}
                  onChange={(selected:any) => handleMultiSelectChange(selected, 'skills')}
                  labelledBy="Select skills"
                />
                </div>
                <div className="space-y-2">
                <Label htmlFor="topicsOfInterest" className="text-black-400 flex items-center">
                  <BookOpenIcon className="w-4 h-4 mr-2 text-green-500" />
                  Topics of Interest:
                </Label>
                <MultiSelect
                  className='bg-black border-green-500 text-black-400 focus:black-green-500 focus:border-green-500'
                  options={TopicsOfInterestOptions}
                  value={formData.topicsOfInterest}
                  onChange={(selected:any) => handleMultiSelectChange(selected, 'topicsOfInterest')}
                  labelledBy="Select topics"
                />
                </div>
              <div className="space-y-2">
                <Label htmlFor="image" className="text-black-400 flex items-center">
                  <ImageIcon className="w-4 h-4 mr-2 text-green-500" />
                  Profile Image:
                </Label>
                <div className="flex items-center">
                  <span className="text-black-500 mr-2">$</span>
                  <ImageEditorComponent setImageState={setImageState} />
                  </div>
              </div>
              <Button type="submit" className="w-full bg-green-600 hover:bg-green-700 text-black">
                Register
              </Button>
            </form>
          </div>
          <div className="flex items-center justify-center space-x-2">
            <p className="text-green-500">Already have an account?</p>
            <Button
              variant="ghost"
              size="icon"
              aria-label="Login"
              onClick={() => window.location.href = '/login'}
              className="text-green-600 hover:text-green-500 underline bg-white hover:bg-white-500"
            >
              
              <span className="text-xm">Login</span>
            </Button>
            </div>
        </CardContent>
        <CardFooter className="text-center text-black-500 border-t border-green-500">
          <p className="text-xs w-full text-green-500">[System]: Secure connection established. Your data will be encrypted.🔒</p>
        </CardFooter>
      </Card>
    </div>
  )
}
