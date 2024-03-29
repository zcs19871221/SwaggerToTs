/**
 * This file was auto-generated by the program based on the back-end data structure.
 * Do not make direct changes to the file.
 */

import { AgenciesGetEP } from './agencies';
import { BusinessGetEP } from './business';

/**
 * @OpenApi 3.0.1
 *
 * @Title Swagger To Ts
 *
 * @Description test openapi doc
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
