/**
 * Formats a date string as a relative time (e.g., "5 min ago", "2 hours ago")
 * @param dateString - ISO date string to format
 * @returns Formatted relative date string
 */
export function formatRelativeDate(dateString: string): string {

    let dateToParse = dateString;
    if (!dateString.endsWith('Z') && !dateString.match(/[+-]\d{2}:\d{2}$/)) {
        dateToParse = dateString + 'Z';
    }

    const date = new Date(dateToParse);
    const now = new Date();

    const diffInSeconds = Math.floor((now.getTime() - date.getTime()) / 1000);

    if (diffInSeconds < 0) return 'just now';

    if (diffInSeconds < 60) return 'just now';
    if (diffInSeconds < 3600) {
        const minutes = Math.floor(diffInSeconds / 60);
        return `${minutes} min ago`;
    }
    if (diffInSeconds < 86400) {
        const hours = Math.floor(diffInSeconds / 3600);
        return `${hours} ${hours === 1 ? 'hour' : 'hours'} ago`;
    }
    if (diffInSeconds < 604800) {
        const days = Math.floor(diffInSeconds / 86400);
        return `${days} ${days === 1 ? 'day' : 'days'} ago`;
    }

    return date.toLocaleDateString();
}

