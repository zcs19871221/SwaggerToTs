/**
 * This file was auto-generated by the program based on the back-end data structrue.
 * Do not make direct changes to the file.
 */

export interface BusinessGetCookie {
  readonly secretId?: string;
  readonly sessionId: string;
}

export interface BusinessGetHeader {
  readonly accept?: string;
  readonly gzip: boolean;
}

export interface BusinessGetPath {
  /**
   * @Description id in path
   *
   * @Deprecated False
   *
   * @AllowEmptyValue False
   *
   * @Explode True
   *
   * @AllowReserved True
   *
   * @SerializeFormat form
   */
  readonly id: number;
  readonly name: string;
}

export interface BusinessGetQuery extends Age {
  /**
   * @SerializeFormat application/json
   */
  readonly bu?: string;
}

/**
 * @Description age query
 *
 * @Format base64
 */
export interface Age {
  readonly ag?: string;
}
