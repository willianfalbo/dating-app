import { User } from "../_models/user";

export class Helper {
  static checkUserGender(gender: string): string {
    if (gender) {
      gender = gender.toLowerCase().trim();
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

  static checkUserPhoto(user: User): User {
    if (user) {
      user.photoUrl = this.checkEmptyUserPhoto(user.photoUrl, user.gender);
      return user;
    } else {
      return user;
    }
  }
}
