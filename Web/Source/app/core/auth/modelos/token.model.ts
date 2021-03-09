import { JsonProperty } from '../../../shared/map-utils';
export class Token {

  @JsonProperty('access_token')
  public accessToken: string;
  @JsonProperty('token_type')
  public tokenType: string;
  @JsonProperty('expires_in')
  public expiresIn: number;

  constructor() {
    this.accessToken = undefined;
    this.tokenType = undefined;
    this.expiresIn = undefined;
  }
}
