import { useState, useEffect } from 'react';
import { userService } from '../api/services/userService';
import type { UserListResponseDto } from '../../types/user';

export function useFollowing() {
  const [following, setFollowing] = useState<UserListResponseDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchFollowing = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await userService.getFollowing();
      setFollowing(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load following');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchFollowing();
  }, []);

  const isFollowing = (userId: number): boolean => {
    return following.some(user => user.id === userId);
  };

  const addFollowing = (userId: number) => {
    setFollowing(prev => {
      if (prev.some(user => user.id === userId)) {
        return prev;
      }
      return [...prev, { id: userId, username: '', email: '' }];
    });
  };

  const removeFollowing = (userId: number) => {
    setFollowing(prev => prev.filter(user => user.id !== userId));
  };

  return {
    following,
    loading,
    error,
    isFollowing,
    refetch: fetchFollowing,
    addFollowing,
    removeFollowing,
  };
}