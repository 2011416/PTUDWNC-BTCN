import React, {useEffect, useState } from "react";
import Table from 'react-bootstrap/Table';
import { Link, useParams, Navigate } from 'react-router-dom';
import { getTags } from "../../../Services/Widgets";
import Loading from "../../../Components/Loading";
import { isInteger } from "../../../Utils/Utils";
import PostFilterPane from "../../../Components/Admin/PostFilterPane";

const Tags = () => {
    const [tagsList, setTagsList] = useState([]);
    const [isVisibleLoading, setIsVisibleLoading] = useState(true);

    useEffect(() => {
        document.title = 'Danh sách thẻ';

        getTags().then((data) => {
            if (data) 
                setTagsList(data.items);
            else 
                setTagsList([]);
            setIsVisibleLoading(false);
        });
    }, []);

    return (
        <>
            <h1>Danh sách thẻ </h1>
            <PostFilterPane />
            {isVisibleLoading ? <Loading /> :
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Tiêu đề</th>
                            <th>Slug</th>
                            <th>Mô tả</th>
                            <th>Hiển thị</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tagsList.length > 0 ? tagsList.map((item, index) => 
                                <tr key={index}>
                                    <td>
                                        <Link to={`/admin/tags/edit/${item.id}`} 
                                        className='text-bold'>
                                            {item.name}
                                        </Link>
                                        <p className="text-muted">{item.shortDescription}</p>
                                    </td>
                                    <td>{item.name}</td>
                                    <td>{item.urlSlug}</td>
                                    <td>{item.description}</td>
                                </tr>
                            ) : 
                            <tr>
                                <td colSpan={4}>
                                    <h4 className="text-danger text-center">Không tìm thấy thẻ nào</h4>
                                </td>
                            </tr>}
                    </tbody>
                </Table>
            }
        </>
    );
}

export default Tags;