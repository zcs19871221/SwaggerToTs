/**
 * This file was auto-generated by the program based on the back-end data structure.
 * Do not make direct changes to the file.
 */

import { BusinessGetEP } from './business';

/**
 * @OpenApi 3.0.1
 *
 * @Title Swagger To Ts
 *
 * @Version V1
 */
export interface Routes {
  /**
   * @Summary business endpoint
   *
   * @Description all business related info
   */
  '/business': {
    GET: BusinessGetEP;
  };
}