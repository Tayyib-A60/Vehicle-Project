import { SaveVehicle } from './saveVehicle';

export interface QueryResult {
  totalItems: number;
  vehicles: SaveVehicle[];
}
