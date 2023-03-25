/**
 * This file was auto-generated by the program based on the back-end data structrue.
 * Do not make direct changes to the file.
 */

import { Age } from './data-schema';
import { NonNullAsRequired } from './helper';

export interface BusinessGetEP {
  Responses: {
    /**
     * @Description not found
     */
    200: {
      Content: {
        'application/json': BusinessGet200ApplicationJsonResponse;
      };
    };
  };
}

export interface BusinessGet200ApplicationJson {
  readonly int?: number;
  readonly object?: Age;
  readonly string?: string;
}

export type BusinessGet200ApplicationJsonResponse =
  NonNullAsRequired<BusinessGet200ApplicationJson>;
