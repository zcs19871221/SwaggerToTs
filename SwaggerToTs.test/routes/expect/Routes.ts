/**
 * This file was auto-generated by the program based on the back-end data structrue.
 * Do not make direct changes to the file.
 */

import { AgenciesGetEP } from './Operations/agencies';
import { BusinessGetEP } from './Operations/business';

/**
 * @OpenApi 3.0.1
 *
 * @Description test openapi doc
 *
 * @Title Swagger To Ts
 *
 * @Version V1
 */
export interface Routes {
  /**
   * @Summary agencies endpoint
   *
   * @Description all agencies related info
   * hello
   */
  '/agencies': {
    GET: AgenciesGetEP;
  };
  /**
   * @Summary business endpoint
   *
   * @Description all business related info
   * hello
   */
  '/business': {
    GET: BusinessGetEP;
  };
}
