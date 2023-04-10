/**
 * This file was auto-generated by the program based on the back-end data structure.
 * Do not make direct changes to the file.
 */

import { Age } from './data-schema';

/**
 * @Summary business get operation summary
 *
 * @Description business get operation
 */
export interface BusinessGetEP {
  Request: {
    Cookie: BusinessGetCookie;
    Header?: BusinessGetHeader;
    Path: BusinessGetPath;
    Query?: BusinessGetQuery;
  };
  Responses: {
    /**
     * @Description not found
     */
    200: {
      Content: null;
    };
  };
}

export interface BusinessGetCookie {
  readonly secretId?: string;
  readonly sessionId: string;
}

export interface BusinessGetHeader {
  readonly accept?: string;
  readonly gzip?: boolean | null;
}

export interface BusinessGetPath {
  /**
   * @Description id in path
   *
   * @AllowReserved True
   *
   * @Explode True
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
