import { useState } from "react";
import { userService } from "../api/services/userService";
import toast from "react-hot-toast";

type OptimisticUpdateCallbacks = {
  onOptimisticUpdate: (userId: number, isFollowing: boolean) => void;
  onRollback: (userId: number, isFollowing: boolean) => void;
};

export function useFollowUser() {
    const [loading, setLoading] = useState<number | null>(null);
    const [error, setError] = useState<string | null>(null);

    const followUser = async (
        userId: number,
        callbacks?: OptimisticUpdateCallbacks
    ) => {
        try {
            setLoading(userId);
            setError(null);

            callbacks?.onOptimisticUpdate(userId, true);

            await userService.followUser(userId);
            toast.success('User followed successfully');
        } catch (err) {
            const error = err instanceof Error ? err.message : "Could not follow user";
            setError(error);

            callbacks?.onRollback(userId, false);
            toast.error(error);
            throw error;
        } finally {
            setLoading(null);
        }
    };

    const unfollow = async (
        userId: number,
        callbacks?: OptimisticUpdateCallbacks
    ) => {
        try {
            setLoading(userId);
            setError(null);

            callbacks?.onOptimisticUpdate(userId, false);

            await userService.unfollowUser(userId);
            toast.success('User unfollowed successfully');
        } catch (err) {
            const error = err instanceof Error ? err.message : "Could not unfollow user";
            setError(error);

            callbacks?.onRollback(userId, true);
            toast.error(error);
            throw error;
        } finally {
            setLoading(null);
        }
    };

    const toggleFollow = async (
        userId: number,
        isCurrentlyFollowing: boolean,
        callbacks?: OptimisticUpdateCallbacks
    ) => {
        if (isCurrentlyFollowing) {
            await unfollow(userId, callbacks);
        } else {
            await followUser(userId, callbacks);
        }
    };

    return {
        followUser,
        unfollow,
        toggleFollow,
        loading,
        error
    };
}