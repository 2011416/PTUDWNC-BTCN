import React, {useEffect, useState } from "react";
import Table from 'react-bootstrap/Table';
import { Link, useParams, Navigate } from 'react-router-dom';
import { getAuthors } from "../../../Services/Widgets";
import Loading from "../../../Components/Loading";
import { isInteger } from "../../../Utils/Utils";
import AuthorFilterPane from "../../../Components/Admin/AuthorFilterPane";

const Authors = () => {
    const [authorsList, setAuthorsList] = useState([]);
    const [isVisibleLoading, setIsVisibleLoading] = useState(true);

    useEffect(() => {
        document.title = 'Danh sách tác giả';

        getAuthors().then((data) => {
            if (data) 
                setAuthorsList(data.items);
            else 
                setAuthorsList([]);
            setIsVisibleLoading(false);
        });
    }, []);

    return (
        <>
            <h1>Danh sách tác giả </h1>
            <AuthorFilterPane />
            {isVisibleLoading ? <Loading /> :
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Tên</th>
                            <th>Slug</th>
                            <th>Email</th>
                            <th>Bài viết</th>
                        </tr>
                    </thead>
                    <tbody>
                        {authorsList.length > 0 ? authorsList.map((item, index) => 
                                <tr key={index}>
                                    <td>
                                        <Link to={`/admin/authors/edit/${item.id}`} 
                                        className='text-bold'>
                                            {item.fullName}
                                        </Link>
                                        <p className="text-muted">{item.shortDescription}</p>
                                    </td>
                                    <td>{item.urlSlug}</td>
                                    <td>{item.email}</td>
                                    <td>{item.postCount}</td>
                                </tr>
                            ) : 
                            <tr>
                                <td colSpan={4}>
                                    <h4 className="text-danger text-center">Không tìm thấy tác giả nào</h4>
                                </td>
                            </tr>}
                    </tbody>
                </Table>
            }
        </>
    );
}

export default Authors;