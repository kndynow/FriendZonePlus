import { useState } from "react";
import { Button, Form } from "react-bootstrap";

type CreateWallPostFormProps = {
    onSubmit: (content: string) => Promise<void>;
    isLoading?: boolean;
}

export default function CreateWallPostForm({ onSubmit, isLoading }: CreateWallPostFormProps) {
    const [content, setContent] = useState('');

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!content.trim() || isLoading) return;

        await onSubmit(content.trim());
        setContent('');
    }

    return (
        <div className="mb-3">
            <Form onSubmit={handleSubmit} className="f-border f-shadow semi-transparent-bg p-3">
                <div className="mb-3">
                    <textarea
                        value={content}
                        onChange={(e) => setContent(e.target.value)}
                        placeholder="What's on your mind?"
                        className="form-control"
                        rows={3}
                        maxLength={300}
                        disabled={isLoading}
                    />
                    <small className="text-muted">
                        {content.length}/300
                    </small>
                </div>
                <Button type="submit" disabled={!content.trim() || isLoading} className="btn btn-primary">
                    {isLoading ? 'Posting...' : 'Post'}
                </Button>
            </Form>
        </div>
    )
}