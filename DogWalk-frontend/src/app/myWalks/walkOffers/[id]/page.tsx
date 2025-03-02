"use client"

import myWalksService from  "@/services/myWalksService";
import { AppContext } from "@/state/AppContext";
import { useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";
import { IWalkOffer } from "../../../domain/IWalkOffer";
import { IWalk } from "../../../domain/IWalk";
import walkOffersService from "@/services/walkOffersService";
import RefreshUserInfo from "@/app/helpers/refreshUserInfo";


export default function WalkOffer({params}: {id: string}) {

    const router = useRouter();

    const [isLoading,  setIsLoading] = useState(true);

    const [walkOffers, setWalkOffers] = useState<IWalkOffer[]>([]);

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
    
        var walkResponse = await myWalksService.getWalk(userInfo!.jwt, params.id)

        if (walkResponse.error){

            var refreshedUserInfo = await RefreshUserInfo(walkResponse.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);
    
            walkResponse = await  myWalksService.getWalk(refreshedUserInfo!.data!.jwt, params.id);
            
        }
        

        if (walkResponse.data) {
            setWalkId(walkResponse.data.id)
            setPrice(walkResponse.data.price)
            setTargetStartingTime(walkResponse.data.targetStartingTime)
            setTargetDurationMinutes(walkResponse.data.targetDurationMinutes)
            setWalkDescription(walkResponse.data.description)
            setLocationId(walkResponse.data.locationId)
        }
        
        var walkOfferResponse = await walkOffersService.getWalkOffersByWalk(userInfo!.jwt, params.id)

        if (walkOfferResponse.error){

            var refreshedUserInfo = await RefreshUserInfo(walkOfferResponse.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);
    
            walkOfferResponse = await  walkOffersService.getWalkOffersByWalk(refreshedUserInfo!.data!.jwt, params.id);
            
        }

        if (walkOfferResponse.data) {
            var walkOffersFiltered = walkOfferResponse.data?.filter(wo => wo.accepted == false)
            setWalkOffers(walkOffersFiltered)
        }

        setIsLoading(false);
    };

    useEffect(() => {loadData()}, [])

    if (isLoading){
        return(
            <>Walk Offers - LOADING</>
        )
    }

    const accept = async (walkOffer: IWalkOffer) => {

        walkOffer.accepted = true;

        const walk: IWalk = {
            "id": walkId!,
            "locationId": locationId!,
            "targetStartingTime": targetStartingTime!,
            "targetDurationMinutes": targetDurationMinutes!,
            "price": price!,
            "startedAt": null,
            "finishedAt": null,
            "closed": true, //means that waler is found and walk is not open for offers
            "description": walkDescription!
        }

        const walkResponse = await myWalksService.updateWalk(userInfo!.jwt, walk);
        const walkOfferResponse = await walkOffersService.updateWalkOffer(userInfo!.jwt, walkOffer);

        if (walkResponse.data && walkOfferResponse.data) {
            const addUserInWalkResponse = await myWalksService.addUserInWalk(userInfo!.jwt, walk.id, walkOffer.appUserId)
            if (addUserInWalkResponse.data) {
                router.push("/myWalks");
            }
        }

        if (walkResponse.errors && walkResponse.errors.length > 0 ||
            walkOfferResponse.errors && walkOfferResponse.errors.length > 0
         ) {

            console.log(walkResponse.errors)
            console.log(walkOfferResponse.errors)

            router.push(`/`)
        }
    }

    //var DateTimeField = require('react-bootstrap-datetimepicker');


    return(
        <>
        <h1>Walk Offers</h1>

        <table className="table">
            <thead>
                <tr>
                    <th>
                        Walker id
                    </th>
                    <th>
                        Walk id
                    </th>
                    <th>
                        Comment
                    </th>
                    <th>
                        Status
                    </th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                {walkOffers.map((walkOffer) =>
                    <tr key={walkOffer.id}>
                        <td>
                            {walkOffer.appUserId}
                        </td>
                        <td>
                            {walkOffer.walkId}
                        </td>
                        <td>
                            {walkOffer.comment}
                        </td>
                        <td>
                            {walkOffer.accepted ? "Accepted" : "Not accepted"}
                        </td>
                        <td>
                        <button onClick={(e) => accept(walkOffer)} className="w-100 btn btn-lg btn-primary">Accept</button> 
                        </td>
                    </tr>
                )}


            </tbody>
        </table>
    </>)

}