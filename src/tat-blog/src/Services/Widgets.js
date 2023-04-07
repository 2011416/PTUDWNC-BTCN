import axios from 'axios';

export async function getCategories (){
    try {
    const response =await
    axios.get (`https://localhost:7245/api/categories?PageSize=10&PageNumber=1`);
        
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

export async function getRandomPosts(limit) {
    try {
        const response = await 
        axios.get (`https://localhost:7245/api/posts/random/${limit}`);
        
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

export async function getFeaturedPosts(limit) {
    try {
        const response = await 
        axios.get (`https://localhost:7245/api/posts/featured/${limit}`);
            
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

export async function getTagCloud() {
    try {
        const respone = await 
        axios.get(`https://localhost:7245/api/tags?PageSize=10&PageNumber=1`);
        
        const data = respone.data;
        if (data.isSuccess) 
            return data.result;
            
        else return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}

export async function getBestAuthors(limit) {
    try {
        const respone = await 
        axios.get(`https://localhost:7245/api/authors/best/${limit}`);
        
        const data = respone.data;
        if (data.isSuccess) 
            return data.result;

        else return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}

export async function getArchives(limit) {
    try {
        const respone = await 
        axios.get(`https://localhost:7245/api/posts/archive/${limit}`);
        const data = respone.data;
        console.log(data);
        if (data.isSuccess) 
            return data.result;

        else return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}