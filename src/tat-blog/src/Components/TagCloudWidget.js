import { useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import { Link } from 'react-router-dom';
import { getTagCloud } from '../Services/Widgets';

const TagCloudWidget = () => {
    const [tagList, setTagCloudList] = useState([]);

    useEffect(() => {
        getTagCloud().then(data => {
            if (data){
                setTagCloudList(data.items);
            }
            else
                setTagCloudList([]);
        });
    }, [])

    return (
        <div className='mb-4'>
            <h3 className='text-success mb-2'>
                Các thẻ
            </h3>
            {tagList.length > 0 && 
                <ListGroup>
                {tagList.map((item, index) => {
                    return (
                        <ListGroup.Item key={index}>
                          <Link to={`/blog/tag/?slug=${item.urlSlug}`}
                              className="btn btn-default btn-outline-dark">
                            {item.name} &nbsp;({item.postCount})
                          </Link>
                        </ListGroup.Item>
                    );
                })}
            </ListGroup>
            }
        </div>
        );
    }

export default TagCloudWidget;
