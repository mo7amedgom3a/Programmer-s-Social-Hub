import dynamic from 'next/dynamic';
import { useState, useEffect } from 'react';
import { Card, CardHeader, CardContent } from './ui/card';
import { Button } from './ui/button';
import Modal from './ui/modal';
import { Avatar, AvatarFallback, AvatarImage } from "../components/ui/avatar";
import '../globals.css';
import { useRouter } from 'next/navigation';

const MonacoEditor = dynamic(() => import('@monaco-editor/react'), { ssr: false });
const ReactQuill = dynamic(() => import('react-quill'), { ssr: false });

interface Comment {
    id: string;
    userMetadata: {
        userId: string;
        username: string;
        name: string;
        bio: string;
        imageUrl: string;
    };
    postId: string;
    content: string;
    language: string;
    codeSection: string;
    createdAt: string;
    updatedAt: string;
}

export function CommentSection({ PostId, commentsNumber, setCommentsNumber }: { PostId: string; commentsNumber: number, setCommentsNumber: any }) {
    const [commentText, setCommentText] = useState('');
    const [commentCode, setCommentCode] = useState('');
    const [showCodeEditor, setShowCodeEditor] = useState(false);
    const [lang, setLang] = useState('');
    const router = useRouter();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [comments, setComments] = useState<Comment[]>([]);
    const [authorId, setAuthorId] = useState('');
    const token = localStorage.getItem('authToken');
    if (!token) {
        router.push('/login');
    }
    else {
        useEffect(() => {
            const decodedToken = JSON.parse(atob(token.split('.')[1]));
            setAuthorId(decodedToken.nameid);
        } , [token]);
    }
    const [showComments, setShowComments] = useState(true); // State to manage visibility of comments

    const list_lang = [
        "c", "c#", "cpp", "java", "Assembly", "python", "javascript", "typescript",
        "html", "css", "shell", "powershell", "sql", "json", "yaml", "xml", 
        "markdown", "plaintext",
    ];

    const fetchComments = async () => {
        try {
            const response = await fetch(`http://localhost:5108/api/Comment/${PostId}`, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'accept': '*/*'
                }
            });
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            const data = await response.json();
            console.log(data);
            setComments(data);
        } catch (error) {
            console.error('Error fetching comments:', error);
        }
    };

    useEffect(() => {
        fetchComments();
    }, [PostId]);

    const handleLangChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setLang(e.target.value);
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        if (commentText.trim() || commentCode.trim()) {
            const newComment = {
                authorId: authorId, // Replace this with the actual user ID
                content: commentText,
                language: lang,
                code: commentCode,
                postId: PostId
            };
            console.log(newComment);
            try {
                const response = await fetch(`http://localhost:5108/api/Comment/${PostId}`, {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json',
                        'accept': '*/*'
                    },
                    body: JSON.stringify(newComment)
                });
                
                if (!response.ok) {
                    throw new Error('Failed to submit comment');
                }

                const savedComment = await response.json();
                
                setCommentsNumber(commentsNumber + 1);
                setCommentText('');
                setCommentCode('');
                setIsModalOpen(false); // Close the modal after saving the comment
                fetchComments(); // Fetch the updated comments
            } catch (error) {
                console.error('Error submitting comment:', error);
            }
        }
    };

    return (
        <div>
            <Modal isOpen={isModalOpen} onClose={() => setIsModalOpen(false)}>
                <Card>
                    <CardHeader>
                        <h2 className="text-lg font-semibold">Add Your Comment</h2>
                        <h3 className="text-sm text-gray-500">Select Language</h3>
                        <select onChange={handleLangChange} className="mb-4 lang-list">
                            {list_lang.map((lang) => (
                                <option key={lang} value={lang}>{lang}</option>
                            ))}
                        </select>
                    </CardHeader>
                    <CardContent>
                        <form onSubmit={handleSubmit}>
                            <ReactQuill
                                value={commentText}
                                onChange={setCommentText}
                                placeholder="Write your comment here..."
                                className="mb-4"
                            />

                            <Button type="button" onClick={() => setShowCodeEditor(!showCodeEditor)} className="mb-4">
                                {showCodeEditor ? 'Hide Code Snippet' : 'Add Code Snippet'}
                            </Button>

                            {showCodeEditor && (
                                <MonacoEditor
                                    height="200px"
                                    width="100%"
                                    language={lang}
                                    value={commentCode}
                                    onChange={(value) => setCommentCode(value || '')}
                                    options={{
                                        selectOnLineNumbers: true,
                                        automaticLayout: true,
                                    }}
                                    className="code-editor"
                                />
                            )}

                            <div className="modal-actions">
                                <Button onClick={() => setIsModalOpen(false)} className="mr-2">Close</Button>
                                <Button type="submit" style={{ backgroundColor: 'green' }}>Save Comment</Button>
                            </div>
                        </form>
                    </CardContent>
                </Card>
            </Modal>
            {showComments && (
                <div className="comment-section" style={{ maxHeight: '400px', overflowY: 'auto' }}>
                    {comments.map((comment) => (
                        <div key={comment.id} className="comment-card">
                            <div className="comment-header">
                                <Avatar 
                                    className="flex flex-row items-center gap-4" 
                                    onClick={() => router.push(`/profile-page/${comment.userMetadata?.userId}`)}
                                    style={{ cursor: 'pointer' }}
                                >
                                    <AvatarImage src={comment.userMetadata.imageUrl || '/default-avatar.png'} alt="User Avatar" />
                                    <AvatarFallback>{comment.userMetadata.name.charAt(0)}</AvatarFallback>
                                </Avatar>
                                <div 
                                    className="comment-user-info" 
                                    onClick={() => router.push(`/profile-page/${comment.userMetadata.userId}`)}
                                    style={{ cursor: 'pointer' }}
                                >
                                    <h3 className="comment-username">{comment.userMetadata.name}</h3>
                                    <p className="comment-title">{comment.userMetadata.bio}</p>
                                    <p className="comment-lang">Posted on: {new Date(comment.createdAt).toLocaleDateString()}</p>
                                </div>
                            </div>
                            <div className="comment-body">
                                <div className="comment-text" dangerouslySetInnerHTML={{ __html: comment.content }} />
                                {comment.codeSection && (
                                    <MonacoEditor
                                        height="200px"
                                        width="100%"
                                        language={comment.language}
                                        value={comment.codeSection}
                                        options={{
                                            readOnly: true,
                                            selectOnLineNumbers: true,
                                            automaticLayout: true,
                                        }}
                                        className="comment-code-editor"
                                    />
                                )}
                            </div>
                        </div>
                    ))}
                </div>
            )}
            <div className='p-2 m-2'>
                <Button 
                    onClick={() => setIsModalOpen(true)} 
                    style={{ backgroundColor: 'green' }} 
                    className='flex items-center justify-center px-4 py-2 text-white font-semibold rounded-lg shadow-md hover:bg-green-700 transition duration-300'
                >
                    Add Comment
                </Button>
            </div>
        </div>
    );
}
