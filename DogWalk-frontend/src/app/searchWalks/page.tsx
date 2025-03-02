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
import walkOffersService from "@/services/walkOffersService";
import MyLocationsService from "@/services/myLocationService";
import RefreshUserInfo from "../helpers/refreshUserInfo";

export default function MyWalks() {

    const router = useRouter();

    const [isLoading,  setIsLoading] = useState(false);

    const [walks, setWalks] = useState<IWalk[]>([]);

    const [searchLocation, setSearchLocation] = useState<string>("");

    const [dogsInWalks, setDogsInWalks] = useState(new Map());

    const [walkLocations, setWalkLocations] = useState(new Map());

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const loadData = async () => {   
        setIsLoading(true)
    
        var response = await MyWalksService.getWalksByLocation(userInfo!.jwt, searchLocation)

        if (response.data) {
            
            setWalks(response.data!)
        } else if  (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            response = await MyWalksService.getWalksByLocation(refreshedUserInfo!.data!.jwt, searchLocation);
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

        var locationsMap = new Map();
        for (const w of response.data!) {
            var location = await MyLocationsService.getLocation(userInfo!.jwt, w.locationId)

            if (location.error){

                var refreshedUserInfo = await RefreshUserInfo(location.error!, userInfo!)
                setUserInfo(refreshedUserInfo!.data!);
        
                location = await MyLocationsService.getLocation(refreshedUserInfo!.data!.jwt, w.locationId);
                
            }
 
            locationsMap.set(w.id, location.data!)
            
        }

        setWalkLocations(locationsMap);
 
        setIsLoading(false);
    };

    const sendWalkOffer = async (walkId: string) => {

        const walkOffer: IWalkOffer = {
            "id": crypto.randomUUID(),
            "appUserId": "00000000-0000-0000-0000-000000000000", // correct users Guid will be added at server(current users Guid)
            "walkId": walkId,
            "comment": "",
            "accepted": false
        }

        var walkOfferResponse = await walkOffersService.createWalkOffer(userInfo!.jwt, walkOffer);

        if  (walkOfferResponse.errors){

            var refreshedUserInfo = await RefreshUserInfo(walkOfferResponse.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            walkOfferResponse = await walkOffersService.createWalkOffer(refreshedUserInfo!.data!.jwt, walkOffer);
        }

        if (walkOfferResponse.data) {
            router.push("/walkersWalkOffers");
        }

        if (walkOfferResponse.errors && walkOfferResponse.errors.length > 0
         ) {

            console.log(walkOfferResponse.errors)

            router.push(`/`)
        }
    }

    //useEffect(() => {loadData()}, [])

    if (isLoading){
        return(
            <>Search - LOADING</>
        )
    } 

    return(
        <>
            <input style={{marginBottom: 10}} onChange={(e) => { setSearchLocation(e.target.value); }} className="form-control mr-sm-2" type="search" placeholder="Search" aria-label="Search"/>
            <button onClick={(e) => {loadData();}} className="btn btn-outline-success my-2 my-sm-0" type="submit">Search</button>

            <h1>Walks</h1>

            <table className="table">
                <thead>
                    <tr>
                        <th>
                            Location
                        </th>
                        <th>
                            Starting time
                        </th>
                        <th>
                            Duration
                        </th>
                        <th>
                            Payment
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
                                {walkLocations.get(walk.id).city}, {walkLocations.get(walk.id).district}
                            </td>
                            <td>
                                {walk.targetStartingTime.replace("T", " ")}
                            </td>
                            <td>
                                {walk.targetDurationMinutes} min
                            </td>
                            <td>
                                {walk.price} eur
                            </td>
                            <td>
                                {walk.description}
                            </td>
                            <td>
                                {dogsInWalks.get(walk.id)[0] ? dogsInWalks.get(walk.id).join(", ") : "none"}
                            </td>
                            <td>
                            <button onClick={(e) => sendWalkOffer(walk.id)} className="w-100 btn btn-lg btn-primary">Send Walk Offer</button>
                            
                            </td>
                        </tr>
                    )}


                </tbody>
            </table>
        </>

    )

}
