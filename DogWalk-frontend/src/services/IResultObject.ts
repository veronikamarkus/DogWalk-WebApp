export interface IResultObject<TResponseData> {
    errors?: string[]
    error?: Error
    status?: number
    data?: TResponseData
}
