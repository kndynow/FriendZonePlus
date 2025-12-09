import { useState } from "react";
import { wallPostService } from "../api/services/wallPostService";
import type { CreateWallPostDto, UpdateWallPostDto } from "../../types/wallpost";
import { toast } from "react-hot-toast";

export const CREATE_POST_LOADING_ID = -1;

export function useWallPostActions() {
    const [loading, setLoading] = useState<number | null>(null);

    const createWallPost = async (data: CreateWallPostDto) => {
        try {
            setLoading(CREATE_POST_LOADING_ID);
            const createdPost = await wallPostService.createWallPost(data);
            toast.success('Post created successfully');
            return createdPost;
        }
        catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to create post';
            toast.error(errorMessage);
            console.error('Error creating post:', err);
            throw err;
        } finally {
            setLoading(null);
        }
    };

    const updateWallPost = async (postId: number, data: UpdateWallPostDto) => {
        try {
            setLoading(postId);
            await wallPostService.updateWallPost(postId, data);
            toast.success('Post updated successfully');
            return data.content;
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to update post';
            toast.error(errorMessage);
            console.error('Error updating post:', err);
            throw err;
        } finally {
            setLoading(null);
        }
    };

    const deleteWallPost = async (postId: number) => {
        try {
            setLoading(postId);
            await wallPostService.deleteWallPost(postId);
            toast.success('Post deleted successfully');
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Failed to delete post';
            toast.error(errorMessage);
            console.error('Error deleting post:', err);
            throw err;
        } finally {
            setLoading(null);
        }
    };

    return {
        createWallPost,
        updateWallPost,
        deleteWallPost,
        loading,
    };
}