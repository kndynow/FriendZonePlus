import apiClient from "../client";
import type { UpdateUserDto, UserListResponseDto, UserProfileDto } from "../../../types/user";


export const userService = {
    getAllUsers: async (): Promise<UserProfileDto[]> => {
        const response = await apiClient.get('/users');
        return response.data;
    },
    getUserById: async (id: number): Promise<UserProfileDto> => {
        const response = await apiClient.get(`/users/${id}`);
        return response.data;
    },
    getAllUsersWithFollowingStatus: async (): Promise<UserProfileDto[]> => {
        const response = await apiClient.get('/users/with-following-status');
        return response.data;
    },
    updateProfile: async (data: UpdateUserDto): Promise<{message: string}> => {
        const response = await apiClient.put('/users/me', data);
        return response.data;
    },
    deleteAccount: async (): Promise<void> => {
        await apiClient.delete('/users/me');
    },
    followUser: async (userId: number): Promise<{message: string}> => {
        const response = await apiClient.post(`/users/${userId}/follow`);
        return response.data;
    },
    unfollowUser: async (userId: number): Promise<{message: string}> => {
        const response = await apiClient.delete(`/users/${userId}/unfollow`);
        return response.data;
    },
    getFollowers: async (): Promise<UserListResponseDto[]> => {
        const response = await apiClient.get('/users/me/followers');
        return response.data;
    },
    getFollowing: async (): Promise<UserListResponseDto[]> => {
        const response = await apiClient.get<UserListResponseDto[]>('/users/me/following');
        return response.data;
      },
}