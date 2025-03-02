import axios from "axios";
import { IResultObject } from "./IResultObject";
import { IUserInfo } from "@/state/AppContext";
import { IWalk } from "@/app/domain/IWalk";



export default  class MyWalksService{
    
    private constructor(){

    }

    private static httpClient = axios.create({
        baseURL: 'http://localhost:5197/api/v1/Walks',
    });

    static async getAll(jwt: string): Promise<IResultObject<IWalk[]>>{

        try{
            const response = await MyWalksService.httpClient.get<IWalk[]>("", {
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


    static async getWalk(jwt: string, walkId: string): Promise<IResultObject<IWalk>>{


        try{
            const response = await MyWalksService.httpClient.get<IWalk>(walkId, {
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

    static async updateWalk(jwt: string, walk: IWalk): Promise<IResultObject<boolean>>{


        try{
            const response = await MyWalksService.httpClient.put<IWalk>(walk.id, walk, {
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

    static async createWalk(jwt: string, walk: IWalk, dogId: string): Promise<IResultObject<boolean>>{


        try{
            const response = await MyWalksService.httpClient.post<IWalk>(dogId, walk, {
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

    static async addUserInWalk(jwt: string, walkId: string, appUserId: string): Promise<IResultObject<boolean>>{

        try{
            
            const response = await MyWalksService.httpClient.get(`AddUserInWalk/${walkId}/${appUserId}`, {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });
            console.log("here", response.status.toString() + " " + response.statusText)

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

    static async getWalksByLocation(jwt: string, location: string): Promise<IResultObject<IWalk[]>>{

        try{
            const response = await MyWalksService.httpClient.get<IWalk[]>(`WalksByLocation/${location}`, {
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
}