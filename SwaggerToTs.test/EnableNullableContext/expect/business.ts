/**
 * This file was auto-generated by the program based on the back-end data structrue.
 * Do not make direct changes to the file.
 */

export interface BusinessGetEP {
  Responses: {
    /**
     * @Description not found
     */
    200: {
      Content: {
        'application/json': {
          readonly int: number;
          readonly object: Age;
          readonly string: string;
        };
      };
    };
  };
}

export interface Age {
  readonly name: string;
}