import { Link } from 'react-router-dom';
import { useQuery } from '../Utils/Utils';

const BadRequest = () => {
    
    // let query = useQuery(),
    // redirectTo = query.get('redirectTo') ?? '/';

    return (
        < >
            <h1>400</h1>
            <h2>Chà yêu cầu không hợp lệ</h2>
            <h3>Có vẻ tham số trong URL của bạn chưa đúng yêu cầu</h3>
            <button>Về trang chủ thôi</button>
        </>
    );
}

export default BadRequest;