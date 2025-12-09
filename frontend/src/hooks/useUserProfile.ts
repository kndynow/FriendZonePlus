import { useState, useEffect, useCallback } from "react";
import toast from "react-hot-toast";

export function useUserProfile(userId: number | null) {
  const [profile, setProfile] = useState({
    firstName: "",
    lastName: "",
    profilePictureUrl: "",
  });

  const [loading, setLoading] = useState(false);

  const fetchProfile = useCallback(async () => {
    if (!userId) return;

    try {
      setLoading(true);

      const res = await fetch(`/api/users/${userId}`, {
        credentials: "include",
      });

      if (!res.ok) return;

      const data = await res.json();

      setProfile({
        firstName: data.firstName ?? "",
        lastName: data.lastName ?? "",
        profilePictureUrl: data.profilePictureUrl ?? "",
      });
    } finally {
      setLoading(false);
    }
  }, [userId]);

  const updateProfile = async () => {
    try {
      setLoading(true);

      const res = await fetch("/api/users/me", {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(profile),
      });

      if (!res.ok) {
        toast.error("Failed to save changes");
        throw new Error("Failed to update profile");
      }

      toast.success("Changes have been saved");

      await fetchProfile();
    } catch {
      toast.error("Failed to save changes");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchProfile();
  }, [fetchProfile]);

  return {
    profile,
    setProfile,
    loading,
    updateProfile,
    refetchProfile: fetchProfile,
  };
}
