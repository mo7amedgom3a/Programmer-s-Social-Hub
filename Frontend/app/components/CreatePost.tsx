'use client';
import React, { useState } from "react";
import { Button } from "./ui/button";
import { Card, CardContent, CardHeader } from "./ui/card";
import { Input } from "./ui/input";
import dynamic from 'next/dynamic';
import 'react-quill/dist/quill.snow.css';
import 'monaco-editor/min/vs/editor/editor.main.css';
import '/app/globals.css'

const MonacoEditor = dynamic(() => import('@monaco-editor/react'), { ssr: false });
const ReactQuill = dynamic(() => import('react-quill'), { ssr: false });

const toBase64 = (file: File) => new Promise((resolve, reject) => {
  const reader = new FileReader();
  reader.readAsDataURL(file);
  reader.onload = () => resolve(reader.result);
  reader.onerror = (error) => reject(error);
});

const uploadImageToCloudinary = async (imageFile: File) => {
  try {
    const response = await fetch('/api/cloudinary-upload', {
      method: 'POST',
      body: JSON.stringify({ data: await toBase64(imageFile) }),
      headers: { 'Content-Type': 'application/json' },
    });

    const data = await response.json();
    return data.url; // Assuming the response contains the URL of the uploaded image
  } catch (error) {
    console.error('Error uploading image:', error);
    return null;
  }
};

interface CreatePostProps {
  newPost: string;
  newPostCode: string;
  setNewPost: React.Dispatch<React.SetStateAction<string>>;
  setNewPostCode: React.Dispatch<React.SetStateAction<string>>;
  onPostSubmit: (post: { title: string; content: string; code: string; language: string; images: string[] }) => void;
  list_lang: string[];
}

export function CreatePost({ newPost, newPostCode, setNewPost, setNewPostCode, onPostSubmit, list_lang }: CreatePostProps) {
  const [title, setTitle] = useState('');
  const [showCodeEditor, setShowCodeEditor] = useState(false);
  const [language, setLang] = useState('C');
  const [imageUrls, setImageUrls] = useState<string[]>([]);
  const [uploading, setUploading] = useState(false);

  const handle_lang = (e: any) => {
    setLang(e.target.value);
  };

  const handleImageUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const files = Array.from(event.target.files || []);
    setUploading(true);

    const uploadedUrls = await Promise.all(files.map(file => uploadImageToCloudinary(file)));
    setImageUrls(uploadedUrls.filter(url => url !== null) as string[]);
    setUploading(false);
  };

  const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (title.trim() && newPost.trim()) {
      onPostSubmit({ title, content: newPost, code: newPostCode, language, images: imageUrls });
      setTitle('');
      setNewPost('');
      setLang('C');
      setNewPostCode('');
      setImageUrls([]);
    }
  };

  return (
    <Card>
      <CardHeader>
        <h2 className="text-lg font-semibold">Create a Post</h2>
        <h3 className="text-sm text-gray-500">Select Language</h3>
        <select onChange={handle_lang} className="mb-4 lang-list">
          {list_lang.map((lang) => (
            <option key={lang} value={lang}>{lang}</option>
          ))}
        </select>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit}>
          <Input
            placeholder="Post Title"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            className="mb-4"
          />
          <ReactQuill
            value={newPost}
            onChange={setNewPost}
            placeholder="What's on your mind?"
            className="mb-4"
          />

          <Button type="button" onClick={() => setShowCodeEditor(!showCodeEditor)} className="mb-4">
            {showCodeEditor ? 'Hide Code Snippet' : 'Add Code Snippet'}
          </Button>

          {showCodeEditor && (
            <MonacoEditor
              height="300px"
              width="100%"
              language={language}
              value={newPostCode}
              onChange={(value) => setNewPostCode(value || '')}
              options={{
                selectOnLineNumbers: true,
                automaticLayout: true,
              }}
              className="code-editor"
            />
          )}

          <Input type="file" accept="image/*" onChange={handleImageUpload} multiple className="input-file"/>
          
          {uploading && <p>Uploading images...</p>}
          
          {imageUrls.length > 0 && (
            <div className="image-preview">
              {imageUrls.map((url, index) => (
                <img key={index} src={url} alt={`Uploaded ${index}`} />
              ))}
            </div>
          )}
          <Button type="submit" className="mt-4" style={{ backgroundColor: 'green' }}>Post</Button>
        </form>
      </CardContent>
    </Card>
  );
}