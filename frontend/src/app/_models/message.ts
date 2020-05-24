export interface Message {
    id: number;
    senderId: number;
    senderKnownAs: string;
    senderPhotoUrl: string;
    senderGender: string;
    recipientId: number;
    recipientKnownAs: string;
    recipientPhotoUrl: string;
    recipientGender: string;
    content: string;
    isRead: boolean;
    dateRead: Date;
    messageSent: Date;
}
