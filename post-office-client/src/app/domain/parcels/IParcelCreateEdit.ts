export interface IParcelCreateEdit {
    id?: string;
    parcelNumber: string;
    recipientName: string;
    destinationCountry: string;
    weight: number;
    price: number;
    bagWithParcelsId: string;
}
