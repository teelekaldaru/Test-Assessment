import { IDate } from "../domain/common/IDate";
import { ITime } from "../domain/common/ITime";

export function getDateFromString(str: string): IDate {
    return {
        year: Number(str.split("T")[0].split("-")[0]),
        month: Number(str.split("T")[0].split("-")[1]),
        day: Number(str.split("T")[0].split("-")[2])
    };
}

export function getTimeFromString(str: string): ITime {
    return {
        hour: Number(str.split("T")[1].split(":")[0]),
        minute: Number(str.split("T")[1].split(":")[1])
    };
}

const pad = (i: number): string => i < 10 ? `0${i}` : `${i}`;

// Formats date and time to yyyy-mm-ddThh:MM:ss
export function getStringFromDateTime(date: IDate, time: ITime) {
    return date.year + "-" + pad(date.month) + "-" + pad(date.day) + "T" +
        pad(time.hour) + ":" + pad(time.minute) + ":00";
}

export function formatDateTime(dateTime: string): string {
    const date = dateTime.split("T")[0];
    const time = dateTime.split("T")[1];
    return date + " " + time.split(":")[0] + ":" + time.split(":")[1];
}
