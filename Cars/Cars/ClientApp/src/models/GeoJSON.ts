export interface IGeometry {
    type: string;
    coordinates: number[];
}

export interface IGeoJson {
    type: string;
    geometry: IGeometry;
    bbox?: number[];
    properties?: any;
}


export class GeoJson implements IGeoJson {
  constructor(public type, public geometry, properties?, bbox?) {}
}