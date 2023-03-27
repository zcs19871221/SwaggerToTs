/**
 * This file was auto-generated by the program based on the back-end data structrue.
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
    Cookie: {
      readonly secretId?: string;
      readonly sessionId: string;
    };
    Header: {
      readonly accept?: string;
      readonly gzip?: boolean | null;
    };
    Path: {
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
    };
    Query: BusinessGetQuery;
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

export interface BusinessGetQuery extends Age {
  /**
   * @SerializeFormat application/json
   */
  readonly bu?: string;
}
