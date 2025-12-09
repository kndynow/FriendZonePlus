import apiClient from "../client";
import type { CreateWallPostDto, UpdateWallPostDto, WallPostResponseDto } from "../../../types/wallpost";

export const wallPostService = {

    createWallPost: async (data: CreateWallPostDto): Promise<WallPostResponseDto> => {

        const response = await apiClient.post('/wallposts', data);
        return response.data;
    },
    updateWallPost: async (id: number, data: UpdateWallPostDto): Promise<{message: string}> => {
        const response = await apiClient.put(`/wallposts/${id}`, data);
        return response.data;
    },
    deleteWallPost: async (id: number): Promise<void> => {
        await apiClient.delete(`/wallposts/${id}`);
    },
    getWallPostFeed: async (): Promise<WallPostResponseDto[]> => {
        const response = await apiClient.get('/wallposts/feed');
        return response.data;
    },
    getWallPostsForUser: async (userId: number): Promise<WallPostResponseDto[]> => {
        const response = await apiClient.get(`/users/${userId}/wall`);
        return response.data;
    }
}