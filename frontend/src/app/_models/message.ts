import { Genders } from "../_shared/types/genders";

export interface Message {
  id: number;
  senderId: number;
  senderKnownAs: string;
  senderPhotoUrl: string;
  senderGender: Genders;
  recipientId: number;
  recipientKnownAs: string;
  recipientPhotoUrl: string;
  recipientGender: Genders;
  content: string;
  isRead: boolean;
  dateRead: Date;
  messageSent: Date;
}
