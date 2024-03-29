/**
 * This file was auto-generated by the program based on the back-end data structure.
 * Do not make direct changes to the file.
 */

export interface BusinessGetEP {
  Request: {
    Query?: BusinessGetQuery;
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
  /**
   * @Title string field of info
   *
   * @Description string field of info
   *
   * @Format address
   *
   * @MaxLength 50
   *
   * @MinLength 5
   *
   * @Pattern /[a-z]*\/
   */
  readonly address?: string | null;
  /**
   * @Description (5 ~ 17] % 8 == true
   *
   * @ExclusiveMaximum False
   *
   * @ExclusiveMinimum True
   *
   * @Maximum 17
   *
   * @Minimum 5
   *
   * @MultipleOf 8
   */
  readonly age?: number | null;
  readonly any?: unknown;
  /**
   * @MaxItems 8
   *
   * @MinItems 3
   *
   * @UniqueItems False
   */
  readonly array?: readonly (string | null)[] | null;
  readonly city?: 'Beijing' | 'Shanghai' | null;
  readonly isDeprecated?: boolean | null;
  readonly record?: Record<string, unknown> | null;
  /**
   * @MaxProperties 8
   *
   * @MinProperties 3
   */
  readonly versionInfo?: {
    readonly id: number;
    readonly location?: string;
  } | null;
}
