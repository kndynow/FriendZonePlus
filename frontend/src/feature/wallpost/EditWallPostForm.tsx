import WallPost from "./WallPost";
import type { WallPostResponseDto } from "../../../types/wallpost";
import { formatPostSubtitle } from "../../utils/wallPostUtils";

type EditWallPostFormProps = {
    post: WallPostResponseDto;
    authorProfilePictureUrl?: string;
    editContent: string;
    onContentChange: (content: string) => void;
    onSave: () => void;
    onCancel: () => void;
    isUpdating?: boolean;
};

export default function EditWallPostForm({
    post,
    authorProfilePictureUrl,
    editContent,
    onContentChange,
    onSave,
    onCancel,
    isUpdating = false,
}: EditWallPostFormProps) {
    return (
        <WallPost
            user={{
                fullName: post.authorName,
                imgPath: authorProfilePictureUrl,
            }}
            content={
                <div>
                    <textarea
                        value={editContent}
                        onChange={(e) => onContentChange(e.target.value)}
                        className="form-control mb-2"
                        rows={3}
                    />
                    <div className="d-flex gap-2">
                        <button
                            onClick={onSave}
                            disabled={isUpdating || !editContent.trim()}
                            className="btn btn-sm btn-primary"
                        >
                            {isUpdating ? "Saving..." : "Save"}
                        </button>
                        <button
                            onClick={onCancel}
                            disabled={isUpdating}
                            className="btn btn-sm btn-secondary"
                        >
                            Cancel
                        </button>
                    </div>
                </div>
            }
            subtitle={formatPostSubtitle(post)}
            userId={post.authorId}
        />
    );
}

