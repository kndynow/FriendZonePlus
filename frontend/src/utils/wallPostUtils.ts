import type { WallPostResponseDto } from "../../types/wallpost";
import { formatRelativeDate } from "./dateFormatter";

/**
 * Formats the subtitle for a wall post
 * @param post - The wall post to format subtitle for
 * @returns Formatted subtitle string
 */
export function formatPostSubtitle(post: WallPostResponseDto): string {
    const formattedDate = formatRelativeDate(post.createdAt);

    if (post.authorId === post.targetUserId) {
        return formattedDate;
    }

    return `${formattedDate} • on ${post.targetUserName}'s wall`;
}

/**
 * Determines if a user can edit a post
 * @param post - The wall post to check
 * @param currentUserId - The ID of the current user
 * @returns True if the user can edit the post
 */
export function canUserEditPost(post: WallPostResponseDto, currentUserId?: number): boolean {
    return post.authorId === currentUserId;
}

/**
 * Determines if a user can delete a post
 * @param post - The wall post to check
 * @param currentUserId - The ID of the current user
 * @returns True if the user can delete the post
 */
export function canUserDeletePost(post: WallPostResponseDto, currentUserId?: number): boolean {
    return post.authorId === currentUserId;
}

