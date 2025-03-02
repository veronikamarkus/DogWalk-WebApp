"use client"

import MyProfileService from  "@/services/myProfileService"
import { AppContext } from "@/state/AppContext";
import Link from 'next/link'
import { useContext, useEffect, useState } from "react";
import { IProfile } from "../domain/IProfile";
import RefreshUserInfo from "../helpers/refreshUserInfo";

export default function MyProfile() {

    const [isLoading,  setIsLoading] = useState(true);

    const [Profile, setProfile] = useState<IProfile>();

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const loadData = async () => {   
        var response = await MyProfileService.getAll(userInfo!.jwt)

        if (response.data) {
            setProfile(response.data[0])
        } else if (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            response = await MyProfileService.getAll(refreshedUserInfo!.data!.jwt);
            
            setProfile(response.data![0])
        }

        setIsLoading(false);
    };

    useEffect(() => {loadData()}, [])

    if (isLoading){
        return(
            <>My Profile - LOADING</>
        )
    }

    return(
        <>
            <h2 style={{color: "#40916c"}}>{userInfo!.firstName}&apos;s Profile</h2>

            <div className="container mt-5">
    
                <div className="row d-flex justify-content-center">
                    
                    <div className="col-md-7">
                        
                        <div className="card p-3 py-4">
                            
                            <div className="text-center">
                                <img src="https://i.imgur.com/gr0S0QQ.png" width="100" className="rounded-circle"/>
                            </div>
                            
                            <div className="text-center mt-3">
                                <span className="bg-secondary p-1 px-4 rounded text-white">{Profile.verified ? "verified ✅" : "not verified"}</span>
                                <h5 className="mt-2 mb-0">{userInfo!.firstName} {userInfo!.lastName}</h5>
                                <span>{userInfo!.role}</span>
                                
                                <div className="px-4 mt-1">
                                    <p className="fonts">{Profile.description}</p>
                                
                                </div>
                                
                                {/* <ul className="social-list">
                                    <li><i className="fa fa-facebook"></i></li>
                                    <li><i className="fa fa-dribbble"></i></li>
                                    <li><i className="fa fa-instagram"></i></li>
                                    <li><i className="fa fa-linkedin"></i></li>
                                    <li><i className="fa fa-google"></i></li>
                                </ul> */}
                                
                                {/* <div className="buttons">
                                    
                                    <button className="btn btn-outline-primary px-4">Message</button>
                                    <button className="btn btn-primary px-4 ms-3">Contact</button>
                                </div>
                                 */}
                                
                            </div>
                            
                        
                            
                            
                        </div>
                        
                    </div>
                    
                </div>
                
            </div>
        </>

    )

}
