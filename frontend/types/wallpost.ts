export interface CreateWallPostDto {
    targetUserId: number;
    content: string;
}

export interface UpdateWallPostDto {
    content: string;
}

export interface WallPostResponseDto {
    id: number;
    content: string;
    createdAt: string;
    authorId: number;
    authorName: string;
    targetUserId: number;
    targetUserName: string;
}