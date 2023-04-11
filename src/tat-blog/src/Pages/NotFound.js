import { Link } from 'react-router-dom';

const NotFound = () => {
    return (
        < >
            ...<h1>404</h1>
            <h2>Chà không tìm thấy trang rồi</h2>
            <h3>Trang mà bạn tìm không tồn tại</h3>
            <Link to="/" className="btn btn-info btn-rounded">
                    Về trang chủ thôi
            </Link>
        </>
    );
}

export default NotFound;