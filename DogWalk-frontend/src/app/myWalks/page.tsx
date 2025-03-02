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

    // const [acceptedWalkOffer, setAcceptedWalkOffer] = useState<IWalkOffer>();

    const loadData = async () => {   
        var response = await MyWalksService.getAll(userInfo!.jwt)

        if (response.data) {
            setWalks(response.data)
        } else if (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            response = await  MyWalksService.getAll(refreshedUserInfo!.data!.jwt);
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
        console.log("VVV", map)
        setDogsInWalks(map);
 
        setIsLoading(false);
    };

    useEffect(() => {loadData()}, [])

    const startTheWalk = async (walk: IWalk) => {
        walk.startedAt = formatDateForOracle(new Date()).replace(" ","T");

        var response = await MyWalksService.updateWalk(userInfo!.jwt, walk)

        if (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            await MyWalksService.updateWalk(refreshedUserInfo!.data!.jwt, walk);
        }
    }

    const finishTheWalk = async (walk: IWalk) => {
        walk.finishedAt = formatDateForOracle(new Date()).replace(" ","T");

        var response = await MyWalksService.updateWalk(userInfo!.jwt, walk)

        if (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            await MyWalksService.updateWalk(refreshedUserInfo!.data!.jwt, walk);
        }
    } 

    if (isLoading){
        return(
            <>My Walks - LOADING</>
        )
    } 

    return(
        <>
            <h1>My Walks</h1>

            <p>
                <Link style={{color: "green"}} href="/myWalks/create">Add new Walk</Link>
            </p>
            <table className="table">
                <thead>
                    <tr>
                        
                        <th>
                            Duration
                        </th>
                        <th>
                            Price
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
                                {walk.finishedAt ? "Closed": (walk.closed && !walk.startedAt ? <span style={{color: "green"}}>Waiting for walker to arrive.</span> : 
                                (!walk.startedAt ? <span style={{color: "purple"}}>Waiting for you to accept walk offers.</span> 
                                : <span style={{color: "blue"}}>Ongoing walk</span>) )}
                            </td>
                            <td>
                                {walk.description}
                            </td>
                            <td>
                                {dogsInWalks.get(walk.id)[0] ? dogsInWalks.get(walk.id).join(", ") : "none"}
                            </td>
                            <td>
                                {walk.finishedAt ? "" : (walk.closed ? (
                                    !walk.startedAt ?
                                    <>  
                                        <div className="form-group">
                                        <button style={{backgroundColor: "#40916c", borderColor: "#40916c"}} onClick={(e) => startTheWalk(walk)} className="w-100 btn btn-sm btn-primary">Walker arrived / start walk</button>
                                        </div>
                                    </>
                                    : <>
                                        <div className="form-group">
                                        <button style={{backgroundColor: "#ef233c", borderColor: "#ef233c"}} onClick={(e) => finishTheWalk(walk)} className="w-100 btn btn-sm btn-primary">Walker arrived back / finish walk</button>
                                        </div>
                                    </>)
                            
                                : (<Link style={{color: "purple"}} href={`/myWalks/walkOffers/${walk.id}`}>Walk offers</Link>))}
                            
                            </td>
                        </tr>
                    )}


                </tbody>
            </table>
        </>

    )

}

function formatDateForOracle(date: Date) {
    const pad = (number: number) => (number < 10 ? '0' : '') + number;
  
    const year = date.getFullYear();
    const month = pad(date.getMonth() + 1); // Months are zero-indexed
    const day = pad(date.getDate());
    const hours = pad(date.getHours());
    const minutes = pad(date.getMinutes());
    const seconds = pad(date.getSeconds());
  
    return `${year}-${month}-${day} ${hours}:${minutes}:${seconds}`;
  }