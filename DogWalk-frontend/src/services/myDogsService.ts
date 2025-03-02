import axios from "axios";
import { IResultObject } from "./IResultObject";
import { IUserInfo } from "@/state/AppContext";
import { IDog } from "@/app/domain/IDog";



export default  class MyDogsService{
    
    private constructor(){

    }

    private static httpClient = axios.create({
        baseURL: 'http://localhost:5197/api/v1/Dogs',
    });

    static async getAll(jwt: string): Promise<IResultObject<IDog[]>>{

        try{
            const response = await MyDogsService.httpClient.get<IDog[]>("", {
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
                errors: [response.status.toString() + " " + response.statusText],
                status: response.status
            }

        } catch (error: any){
            console.log("HERE XXX")
           return {
            errors: [JSON.stringify(error)],
            error: error
            }
        }
    }


    static async getDog(jwt: string, dogId: string): Promise<IResultObject<IDog>>{


        try{
            const response = await MyDogsService.httpClient.get<IDog>(dogId, {
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

    static async updateDog(jwt: string, dog: IDog): Promise<IResultObject<boolean>>{


        try{
            const response = await MyDogsService.httpClient.put<IDog>(dog.id, dog, {
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

    static async deleteDog(jwt: string, dogId: string): Promise<IResultObject<boolean>>{


        try{
            const response = await MyDogsService.httpClient.delete<IDog>(dogId, {
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

    static async createDog(jwt: string, dog: IDog): Promise<IResultObject<boolean>>{


        try{
            const response = await MyDogsService.httpClient.post<IDog>("", dog, {
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

    static async getDogsInWalkNames(jwt: string, walkId: string): Promise<IResultObject<string[]>>{

        try{
            const response = await MyDogsService.httpClient.get<IDog[]>(`DogsInWalk/${walkId}` , {
                headers: {
                    "Authorization": "Bearer " + jwt
                }
            });

            if (response.status < 300) {
                var result = []
                for(const data of response.data){
                    result.push(data.dogName);
                }
                return  {
                    data: result
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