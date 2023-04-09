/**
 * This file was auto-generated by the program based on the back-end data structure.
 * Do not make direct changes to the file.
 */

import { BusinessGetEP, BusinessGetEP1, BusinessGetEP2 } from './business';

/**
 * @OpenApi 3.0.1
 *
 * @Title Swagger To Ts
 *
 * @Version V1
 */
export interface Routes {
  '/api/business': {
    GET: BusinessGetEP;
  };
  '/api/business/{id}': {
    GET: BusinessGetEP1;
  };
  '/api/business/{name}/{id}': {
    GET: BusinessGetEP2;
  };
}
