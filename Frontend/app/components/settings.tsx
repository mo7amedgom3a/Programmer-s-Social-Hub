"use client"

import React, { useState, useEffect } from "react"
import { Card, CardContent, CardHeader } from "./ui/card"
import { Input } from "./ui/input"
import { Textarea } from "./ui/textarea"
import { Button } from "./ui/button"
import { MultiSelect } from "react-multi-select-component"
import ImageEditorComponent from "./imageeditor"
import Label from "./ui/label"
import { EyeIcon, UserIcon, InfoIcon, CodeIcon, BookOpenIcon, ImageIcon } from 'lucide-react'

interface User {
  userId: string;
  imageUrl: string;
  name: string;
  username: string;
  bio: string;
  skills: string[];
  topicsOfInterest: string[];
  followers: string[];
  following: string[];
  followersCount: number;
  followingCount: number;
  _links: {
    self: string;
    update: string;
    delete: string;
  };
}

const RenderSettingsPage = ({ userId }: { userId: string }) => {
  const [imageState, setImageState] = useState('');
  useEffect(() => {
    setFormData(prevData => ({ ...prevData, imageUrl: imageState }))
  }, [imageState])
  const [user, setUser] = useState<User | null>(null);
  const token = localStorage.getItem('authToken');
  const [formData, setFormData] = useState({
    name: '',
    gender: '',
    bio: '',
    age: 0,
    skills: [] as { label: string, value: string }[],
    topicsOfInterest: [] as { label: string, value: string }[],
    imageUrl: ''
  });
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    const fetchUserDetails = async () => {
      try {
        const response = await fetch(`http://localhost:5172/api/User/${userId}`, {
          headers: {
            'Authorization': `Bearer ${token}`,
            'accept': '*/*'
          }
        });
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        const data: User = await response.json();
        setUser(data);
        setFormData({
          name: data.name,
          gender: '',
          bio: data.bio,
          age: 0,
          skills: data.skills.map(skill => ({ label: skill, value: skill })),
          topicsOfInterest: data.topicsOfInterest.map(topic => ({ label: topic, value: topic })),
          imageUrl: data.imageUrl
        });
      } catch (error) {
        console.error('Error fetching user details:', error);
      }
    };

    fetchUserDetails();
  }, [userId]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { id, value } = e.target;
    setFormData(prevState => ({
      ...prevState,
      [id]: value
    }));
  };

  const handleMultiSelectChange = (selected: { label: string, value: string }[], field: 'skills' | 'topicsOfInterest') => {
    setFormData(prevState => ({
      ...prevState,
      [field]: selected
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await fetch(`http://localhost:5172/api/User/${userId}`, {
        method: 'PUT',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
          'accept': '*/*'
        },
        body: JSON.stringify({
          ...formData,
          skills: formData.skills.map(skill => skill.value),
          topicsOfInterest: formData.topicsOfInterest.map(topic => topic.value)
        })
      });
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      setSuccess(true);
    } catch (error) {
      console.error('Error updating user details:', error);
    }
  };

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
  ];

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
  ];

  return (
    <Card>
      <CardHeader>
        <h2 className="text-2xl font-bold">Account Settings</h2>
      </CardHeader>
      <CardContent>
        {user ? (
          <form className="space-y-4" onSubmit={handleSubmit}>
            <div className="space-y-2">
              <Label htmlFor="name" className="text-black-400 flex items-center">
                <UserIcon className="w-4 h-4 mr-2 text-green-500" />
                Name:
              </Label>
              <Input id="name" value={formData.name} onChange={handleInputChange} />
            </div>
            <div className="space-y-2">
              <Label htmlFor="bio" className="text-black-400 flex items-center">
                <InfoIcon className="w-4 h-4 mr-2 text-green-500" />
                Bio:
              </Label>
              <Textarea id="bio" value={formData.bio} onChange={handleInputChange} />
            </div>
            <div className="space-y-2">
              <Label htmlFor="imageUrl" className="text-black-400 flex items-center">
                <ImageIcon className="w-4 h-4 mr-2 text-green-500" />
                Profile Image:
              </Label>
              <ImageEditorComponent setImageState={setImageState} />
            </div>
            <div className="space-y-2">
              <Label htmlFor="skills" className="text-black-400 flex items-center">
                <CodeIcon className="w-4 h-4 mr-2 text-green-500" />
                Skills:
              </Label>
              <MultiSelect
                options={SkillsOptions}
                value={formData.skills}
                onChange={(selected: { label: string, value: string }[]) => handleMultiSelectChange(selected, 'skills')}
                labelledBy="Select Skills"
              />
            </div>
            <div className="space-y-2">
              <Label htmlFor="topicsOfInterest" className="text-black-400 flex items-center">
                <BookOpenIcon className="w-4 h-4 mr-2 text-green-500" />
                Topics of Interest:
              </Label>
              <MultiSelect
                options={TopicsOfInterestOptions}
                value={formData.topicsOfInterest}
                onChange={(selected: { label: string, value: string }[]) => handleMultiSelectChange(selected, 'topicsOfInterest')}
                labelledBy="Select Topics of Interest"
              />
            </div>
            <Button type="submit">Save Changes</Button>
          </form>
        ) : (
          <div>Loading...</div>
        )}
        {success && <div className="mt-4 text-green-500">User details updated successfully!</div>}
      </CardContent>
    </Card>
  );
};

export default RenderSettingsPage;