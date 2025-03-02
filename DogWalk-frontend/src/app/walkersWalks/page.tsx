"use client"

import MyWalksService from  "@/services/myWalksService"
import { AppContext } from "@/state/AppContext";
import { useRouter } from "next/navigation";
import Link from 'next/link'
import { useContext, useEffect, useState } from "react";
import { IWalk } from "../domain/IWalk";
import { BrowserRouter } from 'react-router-dom'
import MyDogsService from "@/services/myDogsService";
import { IWalkOffer } from "../domain/IWalkOffer";
import RefreshUserInfo from "../helpers/refreshUserInfo";

export default function MyWalks() {

    const [isLoading,  setIsLoading] = useState(true);

    const [walks, setWalks] = useState<IWalk[]>([]);

    const [dogsInWalks, setDogsInWalks] = useState(new Map());

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const loadData = async () => {   
        var response = await MyWalksService.getAll(userInfo!.jwt)

        if (response.data) {
    
            setWalks(response.data)

        } else if (response.error){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);
    
            response = await MyWalksService.getAll(refreshedUserInfo!.data!.jwt);
            setWalks(response.data!)
        }

        var map = new Map();
        for (const w of response.data!) {
            var dogs = await MyDogsService.getDogsInWalkNames(userInfo!.jwt, w.id)

            if (dogs.error){

                var refreshedUserInfo = await RefreshUserInfo(dogs.error!, userInfo!)
                setUserInfo(refreshedUserInfo!.data!);
        
                dogs = await MyDogsService.getDogsInWalkNames(refreshedUserInfo!.data!.jwt, w.id);
                
            }
            
            map.set(w.id, dogs.data!)
            
        }

        setDogsInWalks(map);
 
        setIsLoading(false);
    };

    useEffect(() => {loadData()}, [])

    if (isLoading){
        return(
            <>My Walks - LOADING</>
        )
    } 

    return(
        <>
            <h1>My Walks</h1>

            <table className="table">
                <thead>
                    <tr>
                        
                        <th>
                            Duration
                        </th>
                        <th>
                            Payment
                        </th>
                        <th>
                            Started at
                        </th>
                        <th>
                            Finished at
                        </th>
                        <th>
                            Status
                        </th>
                        <th>
                            Description
                        </th>
                        <th>
                            Dogs
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    
                    {walks.map((walk) =>
                        <tr key={walk.id}>
                            
                            <td>
                                {walk.targetDurationMinutes} min
                            </td>
                            <td>
                                {walk.price} eur
                            </td>
                            <td>
                                {walk.startedAt ? walk.startedAt.replace("T", " ") : "-"}
                            </td>
                            <td>
                                {walk.finishedAt ? walk.finishedAt.replace("T", " ") : "-"}
                            </td>
                            <td>
                                {walk.finishedAt ? "Closed": (!walk.startedAt ? "Waiting for you to arrive." : "Ongoing walk." )}
                            </td>
                            <td>
                                {walk.description}
                            </td>
                            <td>
                                {dogsInWalks.get(walk.id)[0] ? dogsInWalks.get(walk.id).join(", ") : "none"}
                            </td>
                            <td>
                            
                            </td>
                        </tr>
                    )}


                </tbody>
            </table>
        </>

    )

}

async function getDogsNames(jwt:string, walkId: string){
    const dogs = await MyDogsService.getDogsInWalkNames(jwt, walkId)
    return dogs.data
}
