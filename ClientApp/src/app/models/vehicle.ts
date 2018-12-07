import { Contact } from './contact';
import { KeyVeluePair } from './keyValuePair';

export interface Vehicle {
  id: number;
  model: KeyVeluePair;
  make: KeyVeluePair;
  isRegistered: boolean;
  feature: {featureId: number, featureName: string}[];
  contact: Contact;
  lastUpdate: string;
}
