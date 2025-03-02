import axios from "axios";
import { IResultObject } from "./IResultObject";
import { IUserInfo } from "@/state/AppContext";
import { IWalkOffer } from "@/app/domain/IWalkOffer";


export default class walkOffersService{
    
    private constructor(){

    }

    private static httpClient = axios.create({
        baseURL: 'http://localhost:5197/api/v1/WalkOffers',
    });

    static async getAll(jwt: string): Promise<IResultObject<IWalkOffer[]>>{

        try{
            const response = await walkOffersService.httpClient.get<IWalkOffer[]>("", {
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


    static async getWalkOffer(jwt: string, WalkOfferId: string): Promise<IResultObject<IWalkOffer>>{


        try{
            const response = await walkOffersService.httpClient.get<IWalkOffer>(WalkOfferId, {
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

    static async updateWalkOffer(jwt: string, WalkOffer: IWalkOffer): Promise<IResultObject<boolean>>{


        try{
            const response = await walkOffersService.httpClient.put<IWalkOffer>(WalkOffer.id, WalkOffer, {
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

    static async createWalkOffer(jwt: string, WalkOffer: IWalkOffer): Promise<IResultObject<boolean>>{


        try{
            const response = await walkOffersService.httpClient.post<IWalkOffer>("", WalkOffer, {
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

    static async getWalkOffersByWalk(jwt: string, walkId: string): Promise<IResultObject<IWalkOffer[]>>{

        try{
            const response = await walkOffersService.httpClient.get<IWalkOffer[]>(`WalkOffersByWalk/${walkId}` , {
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