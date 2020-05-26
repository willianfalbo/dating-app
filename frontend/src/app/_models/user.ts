import { UserPhoto } from './userPhoto';

export class User {
    id: number;
    userName: string;
    gender: string;
    age: number;
    knownAs: string;
    created: Date;
    lastActive: Date;
    city: string;
    country: string;
    photoUrl: string;
    introduction?: string;
    lookingFor?: string;
    interests?: string;
    photos?: UserPhoto[];
    roles: string[];
}
