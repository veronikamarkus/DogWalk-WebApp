"use client"

import Link from "next/link";
import Identity from "./Identity";
import { useContext } from "react";
import { AppContext } from "@/state/AppContext";

export default function Header() {
    return (
        <header>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container">
                    <Link style={{color: "#7b2cbf"}} href="/" className="navbar-brand">Dog Walking App</Link>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                        aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                        <ul className="navbar-nav flex-grow-1">
                            {/* <li className="nav-item">
                                <Link href="/" className="nav-link text-dark">Home</Link>
                            </li> */}
                            
                            <HeaderForRole />
                        </ul>
                        <Identity />
                    </div>
                </div>
            </nav>
        </header>
    );
}

const HeaderForRole = () => {
    const { userInfo, setUserInfo } = useContext(AppContext)!;

    if (userInfo){
    
        if (userInfo.role == 'Dog Owner'){
            return (
                <>
                <ul className="navbar-nav flex-grow-1">
                <li className="nav-item">
                 <Link href="/myProfile/" className="nav-link text-dark">My Profile</Link>
                </li>
                <li className="nav-item">
                    <Link href="/myDogs/" className="nav-link text-dark">My Dogs</Link>
                </li>
                <li className="nav-item">
                 <Link href="/myWalks/" className="nav-link text-dark">My Walks</Link>
                </li>
                </ul>
                </>
            );
        } else if (userInfo.role == 'Dog Walker') {
            return (
                <>
                <ul className="navbar-nav flex-grow-1">
                <li className="nav-item">
                 <Link href="/myProfile/" className="nav-link text-dark">My Profile</Link>
                </li>
                <li className="nav-item">
                <Link href="/walkersWalks/" className="nav-link text-dark">My Walks</Link>
                </li>
                <li className="nav-item">
                <Link href="/walkersWalkOffers/" className="nav-link text-dark">My Walk Offers</Link>
                </li>
                <li className="nav-item">
                <Link href="/searchWalks/" className="nav-link text-dark">Search for a Walk</Link>
                </li>
                </ul>
                </>
            )
        }
    }
    return (<></>)

}
