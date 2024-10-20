// pages/api/cloudinary-upload.js

import cloudinary from 'cloudinary';

cloudinary.config({
  cloud_name: process.env.CLOUDINARY_CLOUD_NAME,
  api_key: process.env.CLOUDINARY_API_KEY,
  api_secret: process.env.CLOUDINARY_API_SECRET,
});

export default async function handler(req, res) {
  if (req.method === 'POST') {
    try {
      const fileStr = req.body.data;
      const uploadedResponse = await cloudinary.v2.uploader.upload(fileStr, {
        upload_preset: 'mohamedgomaa',
      });

      res.status(200).json({ url: uploadedResponse.secure_url });
    } catch (error) {
      console.error('Error uploading to Cloudinary:', error);
      res.status(500).json({ error: 'Failed to upload image' });
    }
  } else {
    res.status(405).json({ error: 'Method not allowed' });
  }
}
