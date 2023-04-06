import React from "react";
import SearchForm from "./SearchForm";
import CategoriesWidget from "./CategoriesWidget";
import RandomPostsWidget from "./RandomPostsWidget";


const Sidebar = () => {
    return (
        <div className='pt-4 ps-2'>
           <SearchForm />

           <CategoriesWidget />    

           <RandomPostsWidget />        
            <h1>
                Bài viết nổi bật
            </h1>
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