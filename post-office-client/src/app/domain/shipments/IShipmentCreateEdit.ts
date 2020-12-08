export interface IShipmentCreateEdit {
    id?: string;
    shipmentNumber: string;
    airport: string;
    flightNumber: string;
    flightDate: string;
    isFinalized: boolean;
}
