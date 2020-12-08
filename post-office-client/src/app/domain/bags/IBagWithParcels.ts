import { IParcel } from "../parcels/IParcel";

export interface IBagWithParcels {
    id: string;
    bagNumber: string;
    shipmentId: string;
    parcels: IParcel[];
}
