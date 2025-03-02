"use client"

import myWalksService from  "@/services/myWalksService";
import { AppContext } from "@/state/AppContext";
import { useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";
import { IWalkOffer } from "../../../domain/IWalkOffer";
import { IWalk } from "/../../domain/IWalk";
import walkOffersService from "@/services/walkOffersService";
import MyDogsService from "@/services/myDogsService";
import MyWalksService from "@/services/myWalksService";
import MyLocationsService from "@/services/myLocationService";
import RefreshUserInfo from "../helpers/refreshUserInfo";


export default function WalkOffer() {

    const router = useRouter();

    const [isLoading,  setIsLoading] = useState(true);

    const [walkOffers, setWalkOffers] = useState<IWalkOffer[]>([]);

    const [walks, setWalks] = useState<Map<string, IWalk>>(new Map());
    const [dogsInWalks, setDogsInWalks] = useState(new Map());
    const [locations, setLocations] = useState(new Map());

    // to change walk offer when submitting
    // const [walkOfferId, setWalkOfferId] = useState<string>();
    // const [appUserId, setAppUserId] = useState<string>();
    // const [associatedWalkId, setAssociatedWalkId] = useState<string>();
    // const [comment, setComment] = useState<string>();
    // const [accepted, setAccepted] = useState<string>();

    // info about walk
    const [walkId, setWalkId] = useState<string>();
    const [price, setPrice] = useState<number>();
    const [targetStartingTime, setTargetStartingTime] = useState<string>();
    const [targetDurationMinutes, setTargetDurationMinutes] = useState<number>();
    const [walkDescription, setWalkDescription] = useState<string>();
    const [locationId, setLocationId] = useState<string>();
    
    //info about user through user profile

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const loadData = async () => {   
        
        var walkOfferResponse = await walkOffersService.getAll(userInfo!.jwt)

        if (walkOfferResponse.data) {
        
            setWalkOffers(walkOfferResponse.data)
        } else if (walkOfferResponse.error){

            var refreshedUserInfo = await RefreshUserInfo(walkOfferResponse.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);
    
            walkOfferResponse = await walkOffersService.getAll(refreshedUserInfo!.data!.jwt);
            setWalkOffers(walkOfferResponse.data!)
        }

        var walksMap = new Map();
        for (const w of walkOfferResponse.data!) {
            var walk = await MyWalksService.getWalk(userInfo!.jwt, w.walkId)

            if (walk.error){

                var refreshedUserInfo = await RefreshUserInfo(walk.error!, userInfo!)
                setUserInfo(refreshedUserInfo!.data!);
        
                walk = await MyWalksService.getWalk(refreshedUserInfo!.data!.jwt, w.walkId);
            }

            walk.data!.targetStartingTime = walk.data!.targetStartingTime.replace("T", " ")
 
            walksMap.set(w.id, walk)
            
        }

        setWalks(walksMap)

        var dogsMap = new Map();

        for (const w of walkOfferResponse.data!) {
            var dogs = await MyDogsService.getDogsInWalkNames(userInfo!.jwt, w.walkId)
            
            if (dogs.error){
              
                var refreshedUserInfo = await RefreshUserInfo(dogs.error!, userInfo!)
                setUserInfo(refreshedUserInfo!.data!);
        
                dogs = await MyDogsService.getDogsInWalkNames(refreshedUserInfo!.data!.jwt, w.walkId);
                
            }
 
            dogsMap.set(w.id, dogs)
            
        }

        setDogsInWalks(dogsMap);

        var locationsMap = new Map();
        for (const w of walkOfferResponse.data!) {
            var locationId = walksMap.get(w.id).data.locationId
            var location = await MyLocationsService.getLocation(userInfo!.jwt, locationId)

            if (location.error){

                var refreshedUserInfo = await RefreshUserInfo(location.error!, userInfo!)
                setUserInfo(refreshedUserInfo!.data!);
        
                location = await MyLocationsService.getLocation(refreshedUserInfo!.data!.jwt, locationId);
                
            }
 
            locationsMap.set(w.id, location)
            
        }

        setLocations(locationsMap);

        setIsLoading(false);
    };

    useEffect(() => {loadData()}, [])

    if (isLoading){
        return(
            <>Walk Offers - LOADING</>
        )
    }

    return(
        <>
        <h1>Your walk offers: </h1>

        <table className="table">
            <thead>
                <tr>
                    <th>
                        Walk Offer
                    </th>
                    <th>
                        {/* Your comment */}
                    </th>
                    <th>
                        Status
                    </th>
                </tr>
            </thead>
            <tbody>
                {walkOffers.map((walkOffer) =>
                    <tr key={walkOffer.id}>
                        <td>
                        <p style={{color: "purple"}}><em>Walk#<strong>{walkOffer.walkId}</strong></em></p>
                            <div className="accordion" id="accordionExample">
                                <h2 className="accordion-header" id={"heading" + walkOffer.id}>
                                <button className="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target={"#" + walkOffer.id} aria-expanded="true" aria-controls={walkOffer.id}>
                                    Walk info
                                </button>
                                </h2>
                                <div id={walkOffer.id} className="accordion-collapse collapse" aria-labelledby={"heading" + walkOffer.id} data-bs-parent="#accordionExample">
                                    <div className="accordion-body" >
                                            Walk payment: <span style={{color: "green"}}> {walks.get(walkOffer.id).data.price} eur </span> <br/>
                                            Walk duration: <span style={{color: "green"}}>{walks.get(walkOffer.id).data.targetDurationMinutes} min </span> <br/>
                                            Starting address: <span style={{color: "green"}}>{locations.get(walkOffer.id).data.startingAddress} </span> <br/>
                                            Starting time: <span style={{color: "green"}}>{(walks.get(walkOffer.id).data.targetStartingTime)} </span> <br/>
                                            Dogs: <span style={{color: "green"}}>{dogsInWalks.get(walkOffer.id).data[0] ? dogsInWalks.get(walkOffer.id).data.join(", ") : "none"} </span> <br/>
                                            Walk description: <span style={{color: "green"}}>{walks.get(walkOffer.id).data.description} </span>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            {/* {walkOffer.comment} */}
                        </td>
                        <td>
                        {/* <span style={{border: '2px solid rgba(0, 0, 0, 0.1)', padding: 2, borderColor:  "blue" , color: "blue"}}>Offer sent</span> */}
                            {walkOffer.accepted ? <span style={{color: "green"}}>Accepted</span> :
                             ((walks.get(walkOffer.id).data.closed) ? <span style={{color: "red"}}>Declined</span>
                             :  <span style={{color: "blue"}}>Offer sent</span>)}
                        </td>
                    </tr>
                )}


            </tbody>
        </table>
    </>)

}