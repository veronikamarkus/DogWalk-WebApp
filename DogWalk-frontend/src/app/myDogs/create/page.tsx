"use client"

import MyDogsService from  "@/services/myDogsService"
import { AppContext } from "@/state/AppContext";
import { useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";
import { IDog } from "../../domain/IDog";
import RefreshUserInfo from "@/app/helpers/refreshUserInfo";

export default function CreateDog() {


    const router = useRouter();

    // const [isLoading,  setIsLoading] = useState(true);

    const [name, setName] = useState<string>();
    const [age, setAge] = useState<number>();
    const [breed, setBreed] = useState<string>();
    const [description, setDescription] = useState<string>();

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    // const loadData = async () => {   
    //     if(userInfo != null && !name){
    //         const response = await MyDogsService.getDog(userInfo!.jwt, params.id)

    //         if (response.data) {
    //             setId(response.data.id)
    //             setName(response.data.dogName)
    //             setAge(response.data.age)
    //             setBreed(response.data.breed)
    //             setDescription(response.data.description)
    //         }
    //     }

    //     setIsLoading(false);
    // };

    // useEffect(() => {loadData()}, [userInfo])

    // if (isLoading){
    //     return(
    //         <>Edit dog - LOADING {JSON.stringify(userInfo, null, 4)}</>
            
    //     )
    // }

    const validateAndCreate= async () => {
        
        // if (email.length < 5 || pwd.length < 6) {
        //     setvalidationError("Invalid input lengths");
        //     return;
        // }

        const dog: IDog = {
            "id": crypto.randomUUID(),
            "dogName": name!,
            "age": age!,
            "breed": breed!,
            "description": description!
        }

        var response = await MyDogsService.createDog(userInfo!.jwt, dog);

        if  (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            response = await MyDogsService.createDog(refreshedUserInfo!.data!.jwt, dog);
        }


        if (response.data) {
            router.push("/myDogs");
        }

        if (response.errors && response.errors.length > 0) {
            // setvalidationError(response.errors[0]);\
            console.log(response.errors)
            router.push(`/myDogs/create`)
        }
    }


    return(
    <>
        <h1>Create</h1>

            <h4>Dog</h4>
            <hr />
            <div className="row">
                <div className="col-md-4">
                        
                        <div className="form-group">
                            <label className="control-label" htmlFor="DogName">DogName</label>
                            <input onChange={(e) => { setName(e.target.value); }} className="form-control" type="text" id="DogName" maxLength={128} name="DogName" value={name} />
                        </div>

                        <div className="form-group">
                            <label className="control-label" htmlFor="Age">Age</label>
                            <input onChange={(e) => { setAge(Number(e.target.value)); }} className="form-control" type="number" id="Age" name="Age" value={age} /><input name="__Invariant" type="hidden" value={age} />
                        </div>

                        <div className="form-group">
                            <label className="control-label" htmlFor="Breed">Breed</label>
                            <input onChange={(e) => { setBreed(e.target.value); }} className="form-control" type="text" id="Breed" maxLength={128} name="Breed" value={breed} />
                        </div>

                        <div className="form-group">
                            <label className="control-label" htmlFor="Description">Description</label>
                            <input onChange={(e) => { setDescription(e.target.value); }} className="form-control" type="text"  id="Description" maxLength={1024} name="Description" value={description} />
                        </div>

                        <div className="form-group">
                        <button onClick={(e) => validateAndCreate()} className="w-100 btn btn-lg btn-primary">Submit</button>
                        </div>
                        userinfo: {userInfo?.jwt}

                </div>
            </div>

            <div>
                <a href="/myDogs">Back to List</a>
            </div>
    </>)

}