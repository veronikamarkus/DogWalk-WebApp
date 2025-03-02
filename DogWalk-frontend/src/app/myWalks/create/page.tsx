"use client"

import MyWalksService from  "@/services/myWalksService"
import MyDogsService from  "@/services/myDogsService"
import { AppContext } from "@/state/AppContext";
import { useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";
import { IDog } from "../../domain/IDog";
import { IWalk } from "../../domain/IWalk";
import { ILocation } from "../../domain/ILocation";
import MyLocationsService from "@/services/myLocationService";
import RefreshUserInfo from "@/app/helpers/refreshUserInfo";



export default function CreateDog() {


    const router = useRouter();

    const [isLoading,  setIsLoading] = useState(true);
    const [dogs, setDogs] = useState<IDog[]>([]);
    const [choosedDogId, setChoosedDogId] = useState<string>();
    //const [walkId, setWalkId] = useState<string>();
    const [price, setPrice] = useState<number>();
    const [targetStartingTime, setTargetStartingTime] = useState<string>();
    const [targetDurationMinutes, setTargetDurationMinutes] = useState<number>();
    const [walkDescription, setWalkDescription] = useState<string>();
   // const [locationId, setLocationId] = useState<string>();
    const [city, setCity] = useState<string>();
    const [district, setDistrict] = useState<string>();
    const [address, setAddress] = useState<string>();

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const loadData = async () => {   
        var response = await MyDogsService.getAll(userInfo!.jwt)

        if (response.data) {
            setDogs(response.data)

        } else if (response.error){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)

            setUserInfo(refreshedUserInfo!.data!);

            response = await MyDogsService.getAll(refreshedUserInfo!.data!.jwt);
        }

        setIsLoading(false);
    };

    useEffect(() => {loadData()}, [])

    if (isLoading){
        return(
            <>Create Walk - LOADING</>
        )
    }

    const validateAndCreate= async () => {
        var locationId = crypto.randomUUID();
        var walkId = crypto.randomUUID();
        // if (email.length < 5 || pwd.length < 6) {
        //     setvalidationError("Invalid input lengths");
        //     return;
        // }

        const walk: IWalk = {
            "id": walkId,
            "locationId": locationId,
            "targetStartingTime": targetStartingTime!,
            "targetDurationMinutes": targetDurationMinutes!,
            "price": price!,
            "startedAt": null,
            "finishedAt": null,
            "closed": false,
            "description": walkDescription!
        }

        const location: ILocation = {
            "id": locationId,
            "city": city!,
            "district": district!,
            "startingAddress": address!,
        }

        var response = await MyLocationsService.createLocation(userInfo!.jwt, location);

        if  (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            await  MyLocationsService.createLocation(refreshedUserInfo!.data!.jwt, location);
        }

        response = await MyWalksService.createWalk(userInfo!.jwt, walk, choosedDogId!);

        if  (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            response = await MyWalksService.createWalk(refreshedUserInfo!.data!.jwt, walk, choosedDogId!);
        }

        if (response.data) {
            router.push("/myWalks");
        }

        if (response.errors && response.errors.length > 0) {
            console.log(response.errors)
            router.push(`/myWalks/create`)
        }
    }

    return(
    <>
        <h1>Create Walk</h1>

            <h4>Walk</h4>
            <hr />
            <div className="row">
                <div className="col-md-4">
                        
                        <div className="form-group">
                            <label className="control-label" htmlFor="DogName">Dog</label>
                            <select onChange={(e) => { setChoosedDogId(e.target.value); }} className="form-select" aria-label="Default select example">
                                <option selected>Choose dog</option>
                                {dogs.map((dog) =>
                                    <option key={dog.id} value={dog.id}>{dog.dogName}</option>
                                )}

                            </select>
                        </div>

                        <div className="form-group">
                            <label className="control-label" htmlFor="walkDescription">Description</label>
                            <input onChange={(e) => { setWalkDescription(e.target.value); }} className="form-control" placeholder="write your description..." type="text"  id="walkDescription" maxLength={1024} name="Description" value={walkDescription} />
                        </div>

                        <div className="form-group">
                            <label className="control-label" htmlFor="targetStartingTime">Starting time</label>
                            <input onChange={(e) => { setTargetStartingTime(e.target.value); }} className="form-control" type="datetime-local" id="targetStartingTime" name="targetStartingTime" value={targetStartingTime}/> 
                            {/* <DateTimeField onChange={(e) => { setTargetStartingTime(e.target.value); }} value={targetStartingTime} /> */}
                        </div>

                        <div className="form-group">
                            <label className="control-label" htmlFor="targetDurationMinutes">Duration</label>
                            <select onChange={(e) => { setTargetDurationMinutes(Number(e.target.value)); }} className="form-select" aria-label="Default select example">
                                <option selected>Choose duration time</option>
                                <option value={30}>30min</option>
                                <option value={60}>1h</option>
                                <option value={90}>1h 30min</option>
                                <option value={120}>2h</option>
                            </select>
                        </div>

                        <div className="form-group">
                            <label className="control-label" htmlFor="Price">Payment</label>
                            <input onChange={(e) => { setPrice(Number(e.target.value)); }} placeholder="write your payment amount..." className="form-control" type="number" id="price" name="price" value={price} /><input name="__Invariant" type="hidden" value={price} />
                        </div>

                        <div className="form-group">
                            <label className="control-label">City</label>
                            <select onChange={(e) => { setCity(e.target.value); }} className="form-select" aria-label="Default select example">
                                <option selected>Choose city</option>
                                <option value="Tallinn">Tallinn</option>
                            </select>
                        </div>

                        <div className="form-group">
                        <label className="control-label">District</label>
                            <select onChange={(e) => { setDistrict(e.target.value); }} className="form-select" aria-label="Default select example">
                                <option selected>Choose district</option>
                                <option value="Kesklinn">Kesklinn</option>
                                <option value="Mustamäe">Mustamäe</option>
                                <option value="Lasnamäe">Lasnamäe</option>
                                <option value="Pirita">Pirita</option>
                                <option value="Põhja-Tallinn">Põhja-Tallinn</option>
                            </select>
                     
                        </div>

                        <div className="form-group">
                            <label className="control-label" htmlFor="Address">Address</label>
                            <input onChange={(e) => { setAddress(e.target.value); }} placeholder="Write your address..." className="form-control" type="text" id="address" name="address" value={address} />
                        </div>

                        <div className="form-group">
                        <button onClick={(e) => validateAndCreate()} className="w-100 btn btn-lg btn-primary">Submit</button>
                        </div>
                        userinfo: {userInfo?.jwt}
                        {/* location: {city}, {district}, {address} */}
                        {/* dogId: {choosedDogId} */}
                        walk: {walkDescription}, {targetDurationMinutes}, {targetStartingTime}, {price}, {}

                </div>
            </div>

            <div>
                <a href="/myWalks">Back to List</a>
            </div>
    </>)

}