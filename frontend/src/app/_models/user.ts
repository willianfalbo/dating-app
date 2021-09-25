import { Genders } from '../_shared/types/genders';
import { Photo } from './photo';

export class User {
  id: number;
  userName: string;
  gender: Genders;
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
  photos?: Photo[];
  roles: string[];
}
