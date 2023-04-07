import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./CategoriesWidget";
import RandomPostsWidget from "./RandomPostsWidget";
import FeaturedPostsWidget from "./FeaturedPostsWidget";


const Sidebar = () => {
    return (
        <div className='pt-4 ps-2'>
           <SearchForm />

           <CategoriesWidget />    

           <RandomPostsWidget />      

           <FeaturedPostsWidget />   
            <h1>
                Đăng ký nhận tin mới
            </h1>
            <h1>
                Tag cloud
            </h1>
        </div>
    )
}

export default Sidebar;