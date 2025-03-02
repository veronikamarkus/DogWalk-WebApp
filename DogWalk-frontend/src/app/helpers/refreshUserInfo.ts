import AccountService from "@/services/accountService";
import { IUserInfo } from "@/state/AppContext";

export default async function RefreshUserInfo (error: Error, userInfo: IUserInfo) {

    if (error.message === "Request failed with status code 401"){
        const refreshedUserInfo = await AccountService.refereshUserInfoData(userInfo!);

        if ( refreshedUserInfo.data) {
            return refreshedUserInfo;
        }
    }
} 
