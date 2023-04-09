import { useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { Link } from 'react-router-dom';
import { getArchives } from '../Services/Widgets';
import { getMonthName } from '../Utils/Utils';

const ArchivesWidget = () => {
    const [postsList, setPosts] = useState([]);

    useEffect(() => {
        getArchives(12).then(data => {
            if (data){
                setPosts(data);
                console.log(data);
            }
            else
                setPosts([]);
        });
    }, [])

    return (
        <div className='mb-4'>
            <h3 className='text-success mb-2'>
                Các bài viết theo tháng
            </h3>
            {postsList.length > 0 && 
             <ListGroup>
                    {postsList.map((item, index) => {
                        return (
                            <ListGroup.Item key={index}>
                            <Link to={`/blog/archive/${item.year}/${item.month}`}>
                                    {`${getMonthName(item.month)} ${item.year} (${item.postCount})`}
                                </Link>
                        </ListGroup.Item>
                        );
                    })}
                </ListGroup>
            }
        </div>
        );
    }

export default ArchivesWidget;
