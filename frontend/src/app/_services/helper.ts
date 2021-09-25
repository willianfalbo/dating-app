import { User } from "../_models/user";
import { Genders } from "../_shared/types/genders";

export class Helper {
  static checkUserGender(value: string): Genders {
    if (value) {
      const gender = value.toLowerCase().trim() as Genders;
      if (gender === 'male' || gender === 'female' || gender === 'unknown') {
        return gender;
      } else {
        return 'unknown';
      }
    } else {
      return 'unknown';
    }
  }

  static checkEmptyUserPhoto(photoUrl: string, gender: string): string {
    if (!photoUrl || photoUrl.trim() === '') {
      photoUrl = `assets/gender/${this.checkUserGender(gender)}.png`;
    }
    return photoUrl;
  }
}
