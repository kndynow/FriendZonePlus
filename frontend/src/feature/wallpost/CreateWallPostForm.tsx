import { useState } from "react";
import { Button, Col, Form, Row } from "react-bootstrap";
import FormField from "../../components/ui/FormField";

type CreateWallPostFormProps = {
  onSubmit: (content: string) => Promise<void>;
  isLoading?: boolean;
};

export default function CreateWallPostForm({
  onSubmit,
  isLoading,
}: CreateWallPostFormProps) {
  const [content, setContent] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!content.trim() || isLoading) return;

    await onSubmit(content.trim());
    setContent("");
  };

  return (
    <>
      <Form
        onSubmit={handleSubmit}
        className="f-border f-shadow semi-transparent-bg p-3"
      >
        <div>
          <FormField
            rows={2}
            value={content}
            onChange={(e) => setContent(e.target.value)}
            placeholder="What's on your mind?"
            disabled={isLoading}
          />
        </div>
        <Row className="mt-2">
          <Col>
            <small className="text-muted">{content.length}/300</small>
          </Col>
          <Col xs={3}>
            <Button
              type="submit"
              disabled={!content.trim() || isLoading}
              className="btn btn-primary"
            >
              {isLoading ? "Posting..." : "Post"}
            </Button>
          </Col>
        </Row>
      </Form>
    </>
  );
}
