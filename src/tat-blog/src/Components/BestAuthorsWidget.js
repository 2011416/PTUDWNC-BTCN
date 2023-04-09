import { useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { Link } from 'react-router-dom';
import { getBestAuthors } from '../Services/Widgets';

const BestAuthorsWidget = () => {
    const [authorsList, setBestAuthorsList] = useState([]);

    useEffect(() => {
        getBestAuthors(4).then(data => {
            if (data){
                setBestAuthorsList(data);
            }
            else
                setBestAuthorsList([]);
        });
    }, [])

    return (
        <div className='mb-4'>
            <h3 className='text-success mb-2'>
                Các tác giả nổi bật
            </h3>
            {authorsList.length > 0 && 
             <ListGroup>
                    {authorsList.map((item, index) => {
                        return (
                            <ListGroup.Item key={index}>
                                <Link to={`/blog/post/?slug=${item.urlSlug}`} key={index}>
                                    {item.fullName}
                                </Link>
                            </ListGroup.Item>
                        );
                    })}
                </ListGroup>
            }
        </div>
        );
    }

export default BestAuthorsWidget;
