export interface IWalk {
    "id": string,
    "locationId": string,
    "targetStartingTime": string,
    "targetDurationMinutes": number,
    "price": number,
    "startedAt": string | null,
    "finishedAt": string | null,
    "closed": boolean,
    "description": string
}