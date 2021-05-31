// Payload Token
export class DecodedToken {
  constructor(public userId: string, public userName: string, public role: string | string[]) { }
}
