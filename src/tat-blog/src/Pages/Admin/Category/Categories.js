import React, {useEffect, useState } from "react";
import Table from 'react-bootstrap/Table';
import { Link, useParams, Navigate } from 'react-router-dom';
import { getCategories } from "../../../Services/Widgets";
import Loading from "../../../Components/Loading";
import { isInteger } from "../../../Utils/Utils";
import CategoryFilterPane from "../../../Components/Admin/CategoryFilterPane";

const Categories = () => {
    const [categoriesList, setCategoriesList] = useState([]);
    const [isVisibleLoading, setIsVisibleLoading] = useState(true);

    useEffect(() => {
        document.title = 'Danh sách chủ đề';

        getCategories().then((data) => {
            if (data) 
                setCategoriesList(data.items);
            else 
                setCategoriesList([]);
            setIsVisibleLoading(false);
        });
    }, []);

    return (
        <>
            <h1>Danh sách chủ đề </h1>
            <CategoryFilterPane />
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
                        {categoriesList.length > 0 ? categoriesList.map((item, index) => 
                                <tr key={index}>
                                    <td>
                                        <Link to={`/admin/categories/edit/${item.id}`} 
                                        className='text-bold'>
                                            {item.name}
                                        </Link>
                                        <p className="text-muted">{item.shortDescription}</p>
                                    </td>
                                    <td>{item.urlSlug}</td>
                                    <td>{item.description}</td>
                                    <td>{item.showOnMenu}</td>
                                </tr>
                            ) : 
                            <tr>
                                <td colSpan={4}>
                                    <h4 className="text-danger text-center">Không tìm thấy chủ đề nào</h4>
                                </td>
                            </tr>}
                    </tbody>
                </Table>
            }
        </>
    );
}

export default Categories;