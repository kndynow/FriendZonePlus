import { useEffect, useState, useCallback } from "react";
import type { WallPostResponseDto } from "../../types/wallpost";
import { wallPostService } from "../api/services/wallPostService";


export function useWallPost(userId?: number) {
    const [wallPosts, setWallPosts] = useState<WallPostResponseDto[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const fetchWallPosts = useCallback(async () => {
        try {
            setLoading(true);
            setError(null);
            const data = userId ? await wallPostService.getWallPostsForUser(userId) : await wallPostService.getWallPostFeed();
            setWallPosts(data);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Failed to load wall posts');
        } finally {
            setLoading(false);
        }
    }, [userId]);

    useEffect(() => {
        fetchWallPosts();
    }, [fetchWallPosts]);

    const addWallPost = (post: WallPostResponseDto) => {
        setWallPosts(prev => [post, ...prev]);
    }

    const removeWallPost = (postId: number) => {
        setWallPosts(prev => prev.filter(post => post.id !== postId));
    }

    const updateWallPost = (postId: number, updatedPost: WallPostResponseDto) => {
        setWallPosts(prev => prev.map(post => post.id === postId ? updatedPost : post));
    }

    return {
        wallPosts,
        loading,
        error,
        refetch: fetchWallPosts,
        addWallPost,
        removeWallPost,
        updateWallPost,
    };
}