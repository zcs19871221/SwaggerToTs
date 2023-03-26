/**
 * This file was auto-generated by the program based on the back-end data structrue.
 * Do not make direct changes to the file.
 */

import { NonNullAsRequired } from './helper';

export type Encoding = number;

/**
 * @Description have header and content
 */
export interface HeaderAndContent {
  Content: {
    'application/json': HeaderAndContentApplicationJson;
    'application/octet-stream': HeaderAndContentApplicationOctetStream;
  };
  Headers: {
    'content-type'?: string;
  };
}

export type HeaderAndContentApplicationJson = NonNullAsRequired<string>;

export type HeaderAndContentApplicationOctetStream = NonNullAsRequired<number>;

/**
 * @Description only have content
 */
export interface OnlyContent {
  Content: {
    'application/octet-stream': OnlyContentApplicationOctetStream;
  };
}

export type OnlyContentApplicationOctetStream =
  NonNullAsRequired<{
  readonly age?: number;
  readonly name?: string;
}>;

/**
 * @Description only have header
 */
export interface OnlyHeader {
  Content: null;
  Headers: {
    cookie?: string;
  };
}
