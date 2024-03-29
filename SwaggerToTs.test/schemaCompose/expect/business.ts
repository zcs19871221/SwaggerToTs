/**
 * This file was auto-generated by the program based on the back-end data structure.
 * Do not make direct changes to the file.
 */

import { Profile, RecordArray } from './data-schema';

export interface BusinessGetEP {
  Responses: {
    /**
     * @Description success
     */
    200: {
      Content: {
        'application/json': {
          readonly info?: {
            readonly enumArrayItems?: readonly ('hello' | 'world' | null)[][];
            readonly enums?: 1 | '2' | 4444444444.2344 | null;
            readonly objectArrayItems?: readonly Profile[];
            readonly profile?: Profile;
            readonly recordArrayItems?: RecordArray;
            readonly recordObject?: Record<string, unknown> | null;
            readonly unknown?: unknown;
            readonly unknownArrayItems?: readonly (unknown[] | null)[];
          };
        };
      };
    };
  };
}
