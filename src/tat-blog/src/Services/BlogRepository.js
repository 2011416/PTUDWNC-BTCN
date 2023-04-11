import axios from 'axios';
import { get_api, post_api } from './Methods';

export async function getPosts (keyword = '',pageSize = 10, pageNumber = 1, sortColumm ='', sortOrder = '') {
    try {
    const response =await
    axios.get (`https://localhost:7245/api/posts?Keyword=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumm=${sortColumm}&SortOrder=${sortOrder}`);
        
    const data = response.data;
        if (data.isSuccess) 
          return data.result;
        else 
            return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}

export function getFilter() {
    return get_api('https:://localhost:7245/api/posts/get-filter');
}

export function getPostsFilter(keyword ='', authorId = '', categoryId = '',
year ='', month='', pageSize = 10, pageNumber= 1, sortColumm = '',
sortOrder = '') {
    let url = new URL ('https://localhost:7245/api/posts/get-posts-filter');
    keyword !== '' && url.searchParams.append('keyword', keyword);
    authorId !== '' && url.searchParams.append('AuthorId', authorId);
    categoryId !== '' && url.searchParams.append('CategoryId', categoryId);
    year !== '' && url.searchParams.append('Year', year);
    month !== '' && url.searchParams.append('Month', month);
    sortColumm !== '' && url.searchParams.append('SortColumm', sortColumm);
    sortOrder !== '' && url.searchParams.append('SortOrder', sortOrder);
    url.searchParams.append('PageSize', pageSize);
    url.searchParams.append('PageNumber', pageNumber);
    return get_api(url.href);
} 

export async function getPostById(id = 0) {
    if (id > 0)
     return get_api(`https://localhost:7245/api/posts/${id}`);
    return null;
}

export function addOrUpdatePost(formData){
    return post_api('https://localhost:7245/api/posts', formData);
}



