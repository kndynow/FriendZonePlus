import { useState, useEffect } from "react";
import type { UserProfileDto } from "../../types/user";
import { userService } from "../api/services/userService";


export function useFindFriends() {
  const [users, setUsers] = useState<UserProfileDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const fetchUsers = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await userService.getAllUsersWithFollowingStatus();
      setUsers(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load users');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const isFollowing = (userId: number): boolean => {
    const user = users.find(u => u.id === userId);
    return user?.isFollowing ?? false;
  };

  const setFollowingStatus = (userId: number, isFollowing: boolean) => {
    setUsers(prev => prev.map(user =>
      user.id === userId
        ? { ...user, isFollowing }
        : user
    ));
  };

  const incrementFollowersCount = (userId: number) => {
    setUsers(prev => prev.map(user =>
      user.id === userId
        ? { ...user, followersCount: user.followersCount + 1 }
        : user
    ));
  };

  const decrementFollowersCount = (userId: number) => {
    setUsers(prev => prev.map(user =>
      user.id === userId
        ? { ...user, followersCount: Math.max(0, user.followersCount - 1) }
        : user
    ));
  };

  return {
    users,
    loading,
    error,
    isFollowing,
    setFollowingStatus,
    incrementFollowersCount,
    decrementFollowersCount,
    refetch: fetchUsers,
  };
}