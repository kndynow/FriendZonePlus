import { useState, useEffect, useMemo } from "react";
import type { UserProfileDto } from "../../types/user";
import { userService } from "../api/services/userService";


export function useUserProfiles(userIds: number[]) {
  const [userProfiles, setUserProfiles] = useState<Map<number, UserProfileDto>>(new Map());
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  const userIdsToFetch = useMemo(() => {
    const uniqueIds = Array.from(new Set(userIds));
    return uniqueIds.filter(id => !userProfiles.has(id));
  }, [userIds, userProfiles]);

  useEffect(() => {
    if (userIdsToFetch.length === 0) return;

    const fetchUserProfiles = async () => {
      try {
        setLoading(true);
        setError(null);

        const profiles = await Promise.all(
          userIdsToFetch.map(id => userService.getUserById(id))
        );

        setUserProfiles(prev => {
          const newMap = new Map(prev);
          profiles.forEach(profile => {
            newMap.set(profile.id, profile);
          });
          return newMap;
        });
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load user profiles');
      } finally {
        setLoading(false);
      }
    };

    fetchUserProfiles();
  }, [userIdsToFetch]);


  const getProfilePictureUrl = (userId: number): string | undefined => {
    return userProfiles.get(userId)?.profilePictureUrl;
  };


  const getUserProfile = (userId: number): UserProfileDto | undefined => {
    return userProfiles.get(userId);
  };

  return {
    getProfilePictureUrl,
    getUserProfile,
    loading,
    error,
  };
}

