export interface UserProfileDto {
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    profilePictureUrl: string;
    followersCount: number;
    followingCount: number;
    isFollowing?: boolean;
}

export interface UpdateUserDto{
    firstName: string;
    lastName: string;
    profilePictureUrl: string;
}

export interface UserListResponseDto {
    id: number;
    username: string;
    email: string;
}