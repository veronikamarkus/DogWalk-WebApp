import axios from "axios";
import { IResultObject } from "./IResultObject";
import { IUserInfo } from "@/state/AppContext";
import { ILocation } from "@/app/domain/ILocation";



export default  class MyLocationsService{
    
    private constructor(){

    }

    private static httpClient = axios.create({
        baseURL: 'http://localhost:5197/api/v1/Locations',
    });

    static async getAll(jwt: string): Promise<IResultObject<ILocation[]>>{

        try{
            const response = await MyLocationsService.httpClient.get<ILocation[]>("", {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });

            if (response.status < 300) {
                return  {
                    data: response.data
                }
            }

            return {
                errors: [response.status.toString() + " " + response.statusText]
            }

        } catch (error: any){
           return {
            errors: [JSON.stringify(error)],
            error: error
            }
        }
    }


    static async getLocation(jwt: string, LocationId: string): Promise<IResultObject<ILocation>>{


        try{
            const response = await MyLocationsService.httpClient.get<ILocation>(LocationId, {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });

            if (response.status < 300) {
                return  {
                    data: response.data
                }
            }

            return {
                errors: [response.status.toString() + " " + response.statusText]
            }

        } catch (error: any){
           return {
            errors: [JSON.stringify(error)],
            error: error
            }
        }
    }

    static async updateLocation(jwt: string, Location: ILocation): Promise<IResultObject<boolean>>{


        try{
            const response = await MyLocationsService.httpClient.put<ILocation>(Location.id, Location, {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });

            if (response.status < 300) {
                return  {
                    data: true
                }
            }

            return {
                errors: [response.status.toString() + " " + response.statusText]
            }

        } catch (error: any){
           return {
            errors: [JSON.stringify(error)],
            error: error
            }
        }
    }

    static async deleteLocation(jwt: string, LocationId: string): Promise<IResultObject<boolean>>{


        try{
            const response = await MyLocationsService.httpClient.delete<ILocation>(LocationId, {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });

            if (response.status < 300) {
                return  {
                    data: true
                }
            }

            return {
                errors: [response.status.toString() + " " + response.statusText]
            }

        } catch (error: any){
           return {
            errors: [JSON.stringify(error)],
            error: error
            }
        }
    }

    static async createLocation(jwt: string, Location: ILocation): Promise<IResultObject<boolean>>{


        try{
            const response = await MyLocationsService.httpClient.post<ILocation>("", Location, {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });

            if (response.status < 300) {
                return  {
                    data: true
                }
            }

            return {
                errors: [response.status.toString() + " " + response.statusText]
            }

        } catch (error: any){
           return {
            errors: [JSON.stringify(error)],
            error: error
            }
        }
    }
}