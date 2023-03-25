/* eslint-disable @typescript-eslint/no-explicit-any */
type IntersectionTuple<S, T extends any[]> = T extends [infer F, ...infer R]
  ? [S & F, ...IntersectionTuple<S, R>]
  : T;

type Permutations<T extends readonly unknown[]> = T['length'] extends 0 | 1
  ? T
  : T extends [infer F, ...infer R]
  ? [F, ...IntersectionTuple<F, Permutations<R>>, ...Permutations<R>]
  : T;

type AllKeysOf<T> = T extends any ? keyof T : never;

type ProhibitKeys<K extends keyof any> = { [P in K]?: never };

export type OneOf<T extends any[]> = {
  [K in keyof T]: T[K] &
    ProhibitKeys<Exclude<AllKeysOf<T[number]>, keyof T[K]>>;
}[number];

export type AnyOf<T extends any[]> = OneOf<Permutations<T>>;

type NullKeys<T> = {
  [k in keyof T]: null extends T[k] ? k : never;
}[keyof T];

export type NonNullAsRequired<T> = T extends (infer U)[]
  ? NonNullAsRequired<U>[]
  : T extends object
  ? Pick<T, NullKeys<T>> & {
      [K in Exclude<keyof T, NullKeys<T>>]-?: NonNullAsRequired<T[K]>;
    }
  : T;
