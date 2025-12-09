import { useState, useEffect } from "react";
import type { UserProfileDto } from "../../types/user";
import { userService } from "../api/services/userService";

export function useUsers() {
  const [users, setUsers] = useState<UserProfileDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const fetchUserProfile = async (userId: number) => {
    try {
      const res = await fetch(`/api/users/${userId}`, {
        credentials: "include",
      });

      if (!res.ok) return null;

      const data = await res.json();
      return {
        firstName: data.firstName ?? "",
        lastName: data.lastName ?? "",
        profilePictureUrl: data.profilePictureUrl ?? "",
      };
    } catch {
      return null;
    }
  };

  const fetchUsers = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await userService.getAllUsers();
      setUsers(data);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Failed to load users");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const incrementFollowersCount = (userId: number) => {
    setUsers((prev) =>
      prev.map((user) =>
        user.id === userId
          ? { ...user, followersCount: user.followersCount + 1 }
          : user
      )
    );
  };

  const decrementFollowersCount = (userId: number) => {
    setUsers((prev) =>
      prev.map((user) =>
        user.id === userId
          ? { ...user, followersCount: Math.max(0, user.followersCount - 1) }
          : user
      )
    );
  };

  return {
    users,
    loading,
    error,
    refetch: fetchUsers,
    fetchUserProfile,
    incrementFollowersCount,
    decrementFollowersCount,
  };
}
