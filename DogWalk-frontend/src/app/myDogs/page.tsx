"use client"

import MyDogsService from  "@/services/myDogsService"
import { AppContext } from "@/state/AppContext";
import { useRouter } from "next/navigation";
import Link from 'next/link'
import { useContext, useEffect, useState } from "react";
import { IDog } from "../domain/IDog";
import { BrowserRouter } from 'react-router-dom'
import AccountService from "@/services/accountService";
import RefresUserInfo from "../helpers/refreshUserInfo";

export default function MyDogs() {

    const [isLoading,  setIsLoading] = useState(true);

    const [dogs, setDogs] = useState<IDog[]>([]);

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const loadData = async () => {   
        var response = await MyDogsService.getAll(userInfo!.jwt)

        if (response.data) {
            setDogs(response.data)

        } else if (response.errors){

                var refreshedUserInfo = await RefresUserInfo(response.error!, userInfo!)
                setUserInfo(refreshedUserInfo!.data!);

                response = await MyDogsService.getAll(refreshedUserInfo!.data!.jwt);
                setDogs(response.data!)
        }

        setIsLoading(false);
    };

    useEffect(() => {loadData()}, [])

    if (isLoading){
        return(
            <>My Dogs - LOADING</>
        )
    }

    return(
        <>
            <h1>My Dogs</h1>

            <p>
                <Link style={{color: "green"}} href="/myDogs/create">Add new dog</Link>
            </p>
            <table className="table">
                <thead>
                    <tr>
                        <th>
                            Name
                        </th>
                        <th>
                            Age
                        </th>
                        <th>
                            Breed
                        </th>
                        <th>
                            Description
                        </th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    {dogs.map((dog) =>
                        <tr key={dog.id}>
                            <td>
                                {dog.dogName}
                            </td>
                            <td>
                                {dog.age}
                            </td>
                            <td>
                                {dog.breed}
                            </td>
                            <td>
                                {dog.description}
                            </td>
                            <td>
                                <Link href={`/myDogs/edit/${dog.id}`}>Edit</Link> |
                                <Link href={"/myDogs/delete/" + dog.id}>Delete</Link>
                            </td>
                        </tr>
                    )}


                </tbody>
            </table>
            {JSON.stringify(userInfo!.jwt, null, 4)}
        </>

    )

}
