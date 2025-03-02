import axios from "axios";
import { IResultObject } from "./IResultObject";
import { IUserInfo } from "@/state/AppContext";
import { IProfile } from "@/app/domain/IProfile";



export default  class MyprofilesService{
    
    private constructor(){

    }

    private static httpClient = axios.create({
        baseURL: 'http://localhost:5197/api/v1/Profiles',
    });

    static async getAll(jwt: string): Promise<IResultObject<IProfile[]>>{

        try{
            const response = await MyprofilesService.httpClient.get<IProfile[]>("", {
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


    static async getProfile(jwt: string, profileId: string): Promise<IResultObject<IProfile>>{


        try{
            const response = await MyprofilesService.httpClient.get<IProfile>(profileId, {
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

    static async updateProfile(jwt: string, profile: IProfile): Promise<IResultObject<boolean>>{


        try{
            const response = await MyprofilesService.httpClient.put<IProfile>(profile.id, profile, {
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

    static async deleteProfile(jwt: string, profileId: string): Promise<IResultObject<boolean>>{


        try{
            const response = await MyprofilesService.httpClient.delete<IProfile>(profileId, {
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

    static async createProfile(jwt: string, profile: IProfile): Promise<IResultObject<boolean>>{


        try{
            const response = await MyprofilesService.httpClient.post<IProfile>("", profile, {
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