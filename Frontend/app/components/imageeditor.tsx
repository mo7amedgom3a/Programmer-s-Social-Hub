"use client";
import { useState } from 'react';
import { ReactPhotoEditor } from 'react-photo-editor';
import 'react-photo-editor/dist/style.css';
import { Button } from './ui/button';

interface ImageEditorComponentProps {
  setImageState: (url: string) => void;
}

export default function ImageEditorComponent({ setImageState }: ImageEditorComponentProps) {
  const [file, setFile] = useState<File | undefined>();
  const [showModal, setShowModal] = useState(false);
  const [avatarUrl, setAvatarUrl] = useState<string | undefined>();
  const [uploading, setUploading] = useState(false);

  // Show modal if file is selected
  const showModalHandler = () => {
    if (file) {
      setShowModal(true);
    }
  };

  // Hide modal
  const hideModal = () => {
    setShowModal(false);
  };

  const handleSaveImage = async (editedFile: File) => {
    if (!(editedFile instanceof File)) {
      console.error('Invalid file format');
      return;
    }

    setFile(editedFile);

    // Generate a temporary URL to preview the edited image locally
    const tempUrl = URL.createObjectURL(editedFile);
    setAvatarUrl(tempUrl);
    hideModal();

    // Upload to Cloudinary
    await uploadImageToCloudinary(editedFile);
  };

  const uploadImageToCloudinary = async (imageFile: File) => {
    setUploading(true);

    try {
      const response = await fetch('/api/cloudinary-upload', {
        method: 'POST',
        body: JSON.stringify({ data: await toBase64(imageFile) }),
        headers: { 'Content-Type': 'application/json' },
      });

      const data = await response.json();
      if (response.ok) {
        setAvatarUrl(data.url); // Set the URL from Cloudinary response
        setImageState(data.url); // Set the image URL to setImageState
      } else {
        console.error('Cloudinary upload error:', data.error);
      }
    } catch (error) {
      console.error('Error uploading image:', error);
    } finally {
      setUploading(false);
    }
  };

  const toBase64 = (file: File) => {
    return new Promise<string>((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result as string);
      reader.onerror = (error) => reject(error);
    });
  };

  const setFileData = (e: React.ChangeEvent<HTMLInputElement> | null) => {
    if (e?.target?.files && e.target.files.length > 0) {
      setFile(e.target.files[0]);
    }
  };

  return (
    <>
      <input
        type="file"
        onChange={(e) => setFileData(e)}
        multiple={false}
      />

      <Button onClick={showModalHandler} className="btn btn-primary" style={{ background: "green" }}>
        Edit
      </Button>

      {avatarUrl && (
        <div>
          <img src={avatarUrl} alt="Avatar" style={{ width: '100px', height: '100px', borderRadius: '50%' }} />
        </div>
      )}

      <ReactPhotoEditor
        open={showModal}
        onClose={hideModal}
        file={file}
        onSaveImage={handleSaveImage}
      />

      {file && (
        <Button
          onClick={() => handleSaveImage(file)}
          className="btn btn-secondary"
          style={{ background: "green", margin: '10px' }}
          disabled={uploading}
        >
          {uploading ? 'Uploading...' : 'Upload'}
        </Button>
      )}
    </>
  );
}
