"use client"

import MyDogsService from  "@/services/myDogsService"
import { AppContext } from "@/state/AppContext";
import { useContext, useEffect, useState } from "react";
import { IDog } from "../../../domain/IDog";
import { useRouter } from "next/navigation";
import RefreshUserInfo from "@/app/helpers/refreshUserInfo";

export default function EditDog({params}: {id: string} ) {
    const router = useRouter();

    const [isLoading,  setIsLoading] = useState(true);

    const [dog, setDog] = useState<IDog>();

    const [name, setName] = useState<string>();
    const [age, setAge] = useState<number>();
    const [breed, setBreed] = useState<string>();
    const [description, setDescription] = useState<string>();

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const loadData = async () => {   
        if(userInfo != null && !name){
            var response = await MyDogsService.getDog(userInfo!.jwt, params.id)

            if  (response.errors){

                var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
                setUserInfo(refreshedUserInfo!.data!);
    
                response = await MyDogsService.getDog(refreshedUserInfo!.data!.jwt.jwt, params.id);
            }

            if (response.data) {
                setDog(response.data)
                setName(response.data.dogName)
                setAge(response.data.age)
                setBreed(response.data.breed)
                setDescription(response.data.description)
            }
        }

        setIsLoading(false);
    };

    useEffect(() => {loadData()}, [userInfo])

    if (isLoading){
        return(
            <>Edit dog - LOADING {JSON.stringify(userInfo, null, 4)}</>
            
        )
    }

    const validateAndUpdate = async () => {
        
        // if (email.length < 5 || pwd.length < 6) {
        //     setvalidationError("Invalid input lengths");
        //     return;
        // }

        const updatedDog: IDog = {
            "id": dog!.id ,
            "dogName": name!,
            "age": age!,
            "breed": breed!,
            "description": description!
        }

        var response = await MyDogsService.updateDog(userInfo!.jwt, updatedDog);

        if  (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            response = await MyDogsService.updateDog(refreshedUserInfo!.data!.jwt, updatedDog);
        }

        if (response.data) {
            router.push("/myDogs");
        }

        if (response.errors && response.errors.length > 0) {
            console.log(response.errors)
            router.push(`/myDogs/edit/${dog!.id}`)
        }
    }

    return(
        <>
            <h1>Edit</h1>

                <h4>Dog {dog?.dogName} {dog?.id}</h4>
                <hr />
                <div className="row">
                    <div className="col-md-4">
                            
                            <div className="form-group">
                                <label className="control-label" htmlFor="DogName">DogName</label>
                                <input onChange={(e) => { setName(e.target.value); }}
                                 className="form-control" 
                                 type="text" 
                                 id="DogName" 
                                 maxLength={128} 
                                 name="DogName" 
                                 value={name} />
                                <span className="text-danger field-validation-valid" ></span>
                            </div>

                            <div className="form-group">
                                <label className="control-label" htmlFor="Age">Age</label>
                                <input onChange={(e) => { setAge(Number(e.target.value)); }} className="form-control" type="number" id="Age" name="Age" value={age} /><input name="__Invariant" type="hidden" value={age} />
                                <span className="text-danger field-validation-valid" ></span>
                            </div>

                            <div className="form-group">
                                <label className="control-label" htmlFor="Breed">Breed</label>
                                <input onChange={(e) => { setBreed(e.target.value); }} className="form-control" type="text"  id="Breed" maxLength={128} name="Breed" value={breed} />
                                <span className="text-danger field-validation-valid" ></span>
                            </div>

                            <div className="form-group">
                                <label className="control-label" htmlFor="Description">Description</label>
                                <input onChange={(e) => { setDescription(e.target.value); }} className="form-control" type="text" id="Description" maxLength={1024} name="Description" value={description} />
                                <span className="text-danger field-validation-valid"></span>
                            </div>

                            <input type="hidden" id="Id" name="Id" value={dog?.id} />
                            <div className="form-group">
                            <button onClick={(e) => validateAndUpdate()} className="w-100 btn btn-lg btn-primary">Submit</button> 
                            </div>
                            {JSON.stringify(age, null, 4)}
                            {JSON.stringify(userInfo, null, 4)}
                            {JSON.stringify(dog, null, 4)}
                    </div>
                </div>

                <div>
                    <a href="/myDogs">Back to Dogs list</a>
                </div>
        </>

    )

}