import { IParcel } from "../parcels/IParcel";

export interface IBagWithParcelsView {
    id: string;
    bagNumber: string;
    shipmentId: string;
    parcelsCount: number;
    totalPrice: number;
    totalWeight: number;
    parcels: IParcel[];
}
