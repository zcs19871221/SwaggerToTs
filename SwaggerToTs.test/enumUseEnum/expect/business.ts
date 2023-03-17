/**
 * This file was auto-generated by the program based on the back-end data structrue.
 * Do not make direct changes to the file.
 */

import { City, Color } from './data-schema';

export interface BusinessGetEP {
  Request: {
    Query: BusinessGetQuery;
  };
  Responses: {
    /**
     * @Description success
     */
    200: {
      Content: null;
    };
  };
}

export interface BusinessGetQuery {
  readonly city?: City;
  readonly color?: Color;
}
