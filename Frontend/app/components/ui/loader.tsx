import * as React from 'react';

import * as ReactLoader from 'react-loader-spinner';


export interface LoaderProps {
  type: 'Audio' | 'BallTriangle' | 'Bars' | 'Circles' | 'Grid' | 'Hearts' | 'Oval' | 'Puff' | 'Rings' | 'TailSpin' | 'ThreeDots';
  color: string;
  height: number;
  width: number;
}