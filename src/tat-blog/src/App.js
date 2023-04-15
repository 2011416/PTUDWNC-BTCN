import './App.css';
import Footer from './Components/Footer';
import Index from './Pages/Index';
import Layout from './Pages/Layout';
import About from './Pages/About';
import Contact from './Pages/Contact';
import RSS from './Pages/Rss';
import AdminLayout from './Pages/Admin/Layout';
import * as AdminIndex from './Pages/Admin/Index';
import Authors from './Pages/Admin/Authors';
import Categories from './Pages/Admin/Category/Categories';
import Comments from './Pages/Admin/Comments';
import Posts from './Pages/Admin/Post/Posts';
import Tags from './Pages/Admin/Tag/Tags';
import NotFound from './Pages/NotFound';
import Edit from './Pages/Admin/Post/Edit';

import {
  BrowserRouter as Router,
  Routes,
  Route,
} from 'react-router-dom';
import BadRequest from './Pages/BadRequest';

function App() {
  return (
    <Router>
      <Routes>
        <Route path='/' element={<Layout />}>
          <Route path='/' element={<Index />} />
          <Route path='blog' element={<Index />} />
          <Route path='blog/Contact' element={<Contact />} />
          <Route path='blog/About' element={<About />} />
          <Route path='blog/RSS' element={<RSS />} />
        </Route>
        
        <Route path='/admin' element={<AdminLayout />} >
          <Route path='/admin' element={<AdminIndex.default />} />
          <Route path='/admin/authors' element={<Authors />} />
          <Route path='/admin/categories' element={<Categories />} />
          <Route path='/admin/comments' element={<Comments />} />
          <Route path='/admin/posts' element={<Posts />} />
          <Route path='/admin/posts/edit' element={<Edit />} />
          <Route path='/admin/posts/edit/:id' element={<Edit />} />
          <Route path='/admin/tags' element={<Tags />} />
        </Route>

        <Route path='/400' element={<BadRequest />} />
        <Route path='*' element={<NotFound />} />
      </Routes>
      <Footer />
    </Router>
  );
}


export default App;
