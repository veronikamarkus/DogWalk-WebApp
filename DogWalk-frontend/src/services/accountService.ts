import axios from "axios";
import { IResultObject } from "./IResultObject";
import { IUserInfo } from "@/state/AppContext";

export default  class AccountService{
    private constructor(){

    }

    private static httpClient = axios.create({
        baseURL: 'http://localhost:5197/api/v1/identity/Account/',
    });

    static async login(email: string, pwd: string): Promise<IResultObject<IUserInfo>>{
        const loginData = {
            email: email,
            password: pwd,
        }

        try{
            const response = await AccountService.httpClient.post<IUserInfo>("Login", loginData);

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
            errors: [JSON.stringify(error)]
            }
        }
    }

    static async refereshUserInfoData(userInfo: IUserInfo): Promise<IResultObject<IUserInfo>>{
        const tokenRefreshInfoDTO = {
            "jwt": userInfo.jwt,
            "refreshToken": userInfo.refreshToken
        }

        try{
            const response = await AccountService.httpClient.post<IUserInfo>("RefreshTokenData", tokenRefreshInfoDTO);

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
            errors: [JSON.stringify(error)]
            }
        }
    }
}