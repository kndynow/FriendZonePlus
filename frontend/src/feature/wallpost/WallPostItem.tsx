import { useState } from "react";
import WallPost from "./WallPost";
import DeletePostModal from "./DeletePostModal";
import EditWallPostForm from "./EditWallPostForm";
import type { WallPostResponseDto } from "../../../types/wallpost";
import type { ActionButton } from "../../components/ui/ActionDropdown";
import { formatPostSubtitle, canUserEditPost, canUserDeletePost } from "../../utils/wallPostUtils";
import { useAuth } from "../../../context/AuthProvider";

type WallPostItemProps = {
    post: WallPostResponseDto;
    authorProfilePictureUrl?: string;
    onUpdate: (postId: number, newContent: string) => Promise<void>;
    onDelete: (postId: number) => Promise<void>;
    isUpdating?: boolean;
    isDeleting?: boolean;
};

export default function WallPostItem({
    post,
    authorProfilePictureUrl,
    onUpdate,
    onDelete,
    isUpdating = false,
    isDeleting = false,
}: WallPostItemProps) {
    const { user: currentUser } = useAuth();
    const [isEditing, setIsEditing] = useState(false);
    const [editContent, setEditContent] = useState(post.content);
    const [showDeleteConfirm, setShowDeleteConfirm] = useState(false);

    const canEdit = canUserEditPost(post, currentUser?.id);
    const canDelete = canUserDeletePost(post, currentUser?.id);

    const handleSaveEdit = async () => {
        if (editContent.trim() && editContent !== post.content) {
            await onUpdate(post.id, editContent.trim());
        }
        setIsEditing(false);
    };

    const handleCancelEdit = () => {
        setEditContent(post.content);
        setIsEditing(false);
    };

    const handleDelete = async () => {
        await onDelete(post.id);
        setShowDeleteConfirm(false);
    };

    if (isEditing) {
        return (
            <EditWallPostForm
                post={post}
                authorProfilePictureUrl={authorProfilePictureUrl}
                editContent={editContent}
                onContentChange={setEditContent}
                onSave={handleSaveEdit}
                onCancel={handleCancelEdit}
                isUpdating={isUpdating}
            />
        );
    }

    const actions: ActionButton[] = [];

    if (canEdit) {
        actions.push({
            icon: "bi bi-pencil-fill",
            onClick: () => setIsEditing(true),
            label: "Edit",
        });
    }

    if (canDelete) {
        actions.push({
            icon: isDeleting ? "bi-hourglass-split" : "bi-trash",
            onClick: () => setShowDeleteConfirm(true),
            label: "Delete",
        });
    }

    return (
        <>
            <WallPost
                user={{
                    fullName: post.authorName,
                    imgPath: authorProfilePictureUrl,
                }}
                content={post.content}
                subtitle={formatPostSubtitle(post)}
                actions={actions.length > 0 ? actions : undefined}
            />

            <DeletePostModal
                show={showDeleteConfirm}
                onHide={() => setShowDeleteConfirm(false)}
                onConfirm={handleDelete}
                isDeleting={isDeleting}
            />
        </>
    );
}

