export interface RegisterRequest {
  /**
   * @minLength 3
   * @maxLength 50
   */
  name: string;
  /**
   * @format email
   * @minLength 1
   */
  email: string;
  /**
   * @minLength 6
   * @pattern ^\S+$
   */
  password: string;
}

export interface LoginRequest {
  /** @minLength 1 */
  email: string;
  /** @minLength 1 */
  password: string;
}