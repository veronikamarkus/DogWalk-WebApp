"use client"

import MyDogsService from  "@/services/myDogsService"
import { AppContext } from "@/state/AppContext";
import { useContext, useEffect, useState } from "react";
import { IDog } from "../../../domain/IDog";
import { useRouter } from "next/navigation";
import { Router } from "react-router-dom";
import RefreshUserInfo from "@/app/helpers/refreshUserInfo";

export default function EditDog({params}: {id: string} ) {
    const router = useRouter();

    const [isLoading,  setIsLoading] = useState(true);

    const [dog, setDog] = useState<IDog>();

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const loadData = async () => {   
        if(userInfo != null){
            var response = await MyDogsService.getDog(userInfo!.jwt, params.id)

            if  (response.errors){

                var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
                setUserInfo(refreshedUserInfo!.data!);

                response = await MyDogsService.getDog(refreshedUserInfo!.data!.jwt, params.id);
            }

            if (response.data) {
                setDog(response.data)
            }
        }

        setIsLoading(false);
    };

    useEffect(() => {loadData()}, [userInfo])

    if (isLoading){
        return(
            <>Delete dog - LOADING {JSON.stringify(userInfo, null, 4)}</>
            
        )
    }

    const validateAndDelete = async () => {
        
        // if (email.length < 5 || pwd.length < 6) {
        //     setvalidationError("Invalid input lengths");
        //     return;
        // }

        var response = await MyDogsService.deleteDog(userInfo!.jwt, dog!.id);

        if  (response.errors){

            var refreshedUserInfo = await RefreshUserInfo(response.error!, userInfo!)
            setUserInfo(refreshedUserInfo!.data!);

            response = await MyDogsService.deleteDog(refreshedUserInfo!.data!.jwt, dog!.id);
        }

        if (response.data) {
            router.push("/myDogs");
        }

        if (response.errors && response.errors.length > 0) {
            // setvalidationError(response.errors[0]);\
            console.log(response.errors)
            router.push(`/myDogs/delete/${dog!.id}`)
        }
    }

    return(
        <>
            <h1>Delete</h1>

                <h3>Are you sure you want to delete this?</h3>
                <table>
                    <h4>Dog</h4>
                    <hr />
                    <tr className="row">
                        <td className = "col-sm-2">
                            Name
                        </td>
                        <td className = "col-sm-10">
                            {dog?.dogName}
                        </td>
                        <td className = "col-sm-2">
                            Age
                        </td>
                        <td className = "col-sm-10">
                            {dog?.age}
                        </td>
                        <td className = "col-sm-2">
                            Breed
                        </td>
                        <td className = "col-sm-10">
                            {dog?.breed}
                        </td>
                        <td className = "col-sm-2">
                            Description
                        </td>
                        <td className = "col-sm-10">
                            {dog?.description}
                        </td>
                    </tr>
                    
                        <input type="hidden" data-val="true" data-val-required="The Id field is required." id="Id" name="Id" value="f6e8776d-08ee-410f-a1b5-34d1593435b3" />
                        <input onClick={(e) => validateAndDelete()} type="submit" value="Delete" className="btn btn-danger" /> |
                        <a href="/myDogs">Back to List</a>
                </table>
            </>

    )

}